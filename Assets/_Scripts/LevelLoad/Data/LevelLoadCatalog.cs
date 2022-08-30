using System.Collections.Generic;
using System.Linq;
using Common.Enum;
using UnityEngine;

namespace LevelLoad.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelLoadCatalog", order = 1)]
    public class LevelLoadCatalog : ScriptableObject, IInitializable
    {
        [SerializeField] private List<LevelCatalogEntry> _LevelCatalogEntries;

        private Dictionary<ContentType, List<LevelCatalogEntry>> _contentTypeToLevelEntry = new Dictionary<ContentType, List<LevelCatalogEntry>>();

        public List<LevelCatalogEntry> GetCatalogEntriesOfType(ContentType contentType)
        {
            return _contentTypeToLevelEntry[contentType];
        }
        
        public void Initialize()
        {
            _contentTypeToLevelEntry[ContentType.Local] = _LevelCatalogEntries.Where(entry => entry.ContentType == ContentType.Local).ToList();
            _contentTypeToLevelEntry[ContentType.Remote] = _LevelCatalogEntries.Where(entry => entry.ContentType == ContentType.Remote).ToList();
        }

    }
}