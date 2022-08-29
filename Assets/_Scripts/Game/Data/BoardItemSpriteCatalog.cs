using System;
using System.Collections.Generic;
using Game.Gameplay.Item;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BoardItemSpriteCatalog", order = 1)]
    public class BoardItemSpriteCatalog : ScriptableObject
    {
        [Serializable]
        private class BoardItemSpriteData
        {
            public ItemType Key;
            public Sprite Sprite;
        }

        [SerializeField] private BoardItemSpriteData[] _SpriteData;

        private Dictionary<ItemType, Sprite> _keyToSprite;

        public void Initialize()
        {
            _keyToSprite = new Dictionary<ItemType, Sprite>();
            foreach (var boardItemSpriteData in _SpriteData)
            {
                var key = boardItemSpriteData.Key;
                var sprite = boardItemSpriteData.Sprite;
                if (_keyToSprite.ContainsKey(key))
                {
                    Debug.LogError($"{key} already exists in BoardItemSpriteCatalog.");
                    continue;
                }

                _keyToSprite[key] = sprite;
            }
        }

        public Sprite GetSprite(ItemType key)
        {
            if (_keyToSprite.ContainsKey(key))
            {
                return _keyToSprite[key];
            }
            
            Debug.LogError($"{key} does not exist in BoardItemSpriteCatalog.");
            return null;
        }
    }
}