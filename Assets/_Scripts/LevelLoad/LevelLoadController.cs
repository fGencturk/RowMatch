using System.Collections.Generic;
using Common.Context;
using Common.Enum;
using Game.Model;
using LevelLoad.Data;
using LevelLoad.RemoteCache;
using UnityEngine;
using UnityEngine.Networking;
using Utilities;

namespace LevelLoad
{
    public class LevelLoadController : MonoBehaviour
    {
        public List<LevelModel> Levels = new List<LevelModel>();
        
        private RemoteLevelsCache _remoteLevelsCache;

        public void Awake()
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
            await TaskUtilities.WaitUntil(() => webRequest.isDone);
            var fileContent = webRequest.downloadHandler.text;
            AddLevelFromString(fileContent);
            _remoteLevelsCache.Save(levelCatalogEntry, fileContent);
            // TODO maybe fire an event here so that we can create view for recently downloaded level while player views levels popup
        }

    }
}