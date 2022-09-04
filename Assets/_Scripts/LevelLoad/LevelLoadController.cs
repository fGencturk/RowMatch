using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Context;
using Common.Enum;
using Common.Scene.SceneInitializer.Bindings;
using Game.Model;
using LevelLoad.Data;
using LevelLoad.RemoteCache;
using UnityEngine;
using UnityEngine.Networking;
using Utilities;

namespace LevelLoad
{
    public class LevelLoadController : IInitializable
    {
        private const int RetryAfterSeconds = 60;
        
        public List<LevelModel> Levels = new List<LevelModel>();
        
        private RemoteLevelsCache _remoteLevelsCache;

        public void Initialize()
        {
            _remoteLevelsCache = new RemoteLevelsCache();
            // Load local levels
            var levelLoadCatalog = ProjectContext.GetInstance<LevelLoadCatalog>();
            var localLevels = levelLoadCatalog.GetCatalogEntriesOfType(ContentType.Local);
            foreach (var levelEntry in localLevels)
            {
                var textAsset = Resources.Load<TextAsset>(levelEntry.ContentPath);
                AddLevelFromString(textAsset.text);
            }
            
            // Load remote levels or start downloading them
            var remoteLevels = levelLoadCatalog.GetCatalogEntriesOfType(ContentType.Remote);
            foreach (var levelEntry in remoteLevels)
            {
                if (_remoteLevelsCache.TryGetContent(levelEntry, out var fileContent))
                {
                    AddLevelFromString(fileContent);
                }
                else
                {
                    DownloadRemoteContent(levelEntry);
                }
            }
        }

        private void AddLevelFromString(string levelString)
        {
            var levelModel = LevelDataDeserializer.LoadFromString(levelString);
            Levels.Add(levelModel);
        }

        private async void DownloadRemoteContent(LevelCatalogEntry levelCatalogEntry)
        {
            var webRequest = new UnityWebRequest(levelCatalogEntry.ContentPath);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SendWebRequest();
            Debug.Log($"Downloading remote level {levelCatalogEntry.LevelName}");
            await TaskUtilities.WaitUntil(() => webRequest.isDone);

            if (webRequest.error != null)
            {
                Debug.LogError($"Downloading remote level {levelCatalogEntry.LevelName} failed, retying after {RetryAfterSeconds} seconds.\n{webRequest.error}");
                RetryDownloadAfterDelay(levelCatalogEntry);
                return;
            }
            
            var fileContent = webRequest.downloadHandler.text;
            AddLevelFromString(fileContent);
            _remoteLevelsCache.Save(levelCatalogEntry, fileContent);
            // TODO maybe fire an event here so that we can create view for recently downloaded level while player views levels popup
        }

        private async void RetryDownloadAfterDelay(LevelCatalogEntry levelCatalogEntry)
        {
            await Task.Delay(RetryAfterSeconds * 1000);
            DownloadRemoteContent(levelCatalogEntry);
        }
    }
}