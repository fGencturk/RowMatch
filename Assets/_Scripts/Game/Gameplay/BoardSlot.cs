using Common;
using Game.Data;
using Game.Gameplay.Item;
using UnityEngine;
using Utilities;

namespace Game.Gameplay
{
    public class BoardSlot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _ItemSprite;
        
        public IBoardItem BoardItem;
        [HideInInspector] 
        public bool IsLocked;
        
        public Vector2Int Index { get; private set; }
        public ItemType ItemType { get; private set; }

        private Vector2 _dragStartPosition;

        public void Initialize(Vector2Int indexes, ItemType itemType)
        {
            Index = indexes;
            
            var slotSize = Constants.Gameplay.BoardSlotSize;
            slotSize.Scale(indexes);
            transform.position = slotSize;
            
            UpdateVisual(itemType);
        }
        
        // TODO remove this, update with change child system
        public void UpdateVisual(ItemType itemType)
        {
            ItemType = itemType;
            var spriteCatalog = ProjectContext.GetInstance<BoardItemSpriteCatalog>();
            _ItemSprite.sprite = spriteCatalog.GetSprite(itemType);
        }

    }
}