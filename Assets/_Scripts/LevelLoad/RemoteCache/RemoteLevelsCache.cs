using System.IO;
using LevelLoad.Data;
using Utilities;

namespace LevelLoad.RemoteCache
{
    public class RemoteLevelsCache
    {

        public RemoteLevelsCache()
        {
            var rootPath = Constants.Path.Level.RemoteCachePath;
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
        }
        
        public bool Exists(LevelCatalogEntry levelCatalogEntry)
        {
            var cachePath = GetCachePath(levelCatalogEntry);
            return File.Exists(cachePath);
        }

        public string GetContent(LevelCatalogEntry levelCatalogEntry)
        {
            var cachePath = GetCachePath(levelCatalogEntry);
            return File.ReadAllText(cachePath);
        }

        public bool TryGetContent(LevelCatalogEntry levelCatalogEntry, out string content)
        {
            if (!Exists(levelCatalogEntry))
            {
                content = null;
                return false;
            }

            content = GetContent(levelCatalogEntry);
            return true;
        }

        public void Save(LevelCatalogEntry levelCatalogEntry, string fileContent)
        {
            var cachePath = GetCachePath(levelCatalogEntry);
            File.WriteAllText(cachePath, fileContent);
        }

        public void DeleteCache(LevelCatalogEntry levelCatalogEntry)
        {
            var cachePath = GetCachePath(levelCatalogEntry);
            File.Delete(cachePath);
        }

        private string GetCachePath(LevelCatalogEntry levelCatalogEntry)
        {
            return $"{Constants.Path.Level.RemoteCachePath}{levelCatalogEntry.LevelName}";
        }
    }
}