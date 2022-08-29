using Common;
using Game.Data;
using Game.Gameplay.Item;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Game.Gameplay
{
    public class BoardSlot : MonoBehaviour, IBeginDragHandler, IDragHandler
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

        #region IBeginDragHandler
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragStartPosition = eventData.position;
        }

        #endregion

        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
            var deltaSinceBegin = eventData.position - _dragStartPosition;
            var threshold = _ItemSprite.bounds.size.x / 2f;
            if (deltaSinceBegin.x > threshold)
            {
                GameplayManager.Instance.MoveItem(Index, Vector2Int.right);
                eventData.pointerDrag = null;
            } else if (deltaSinceBegin.x < -threshold)
            {
                GameplayManager.Instance.MoveItem(Index, Vector2Int.left);
                eventData.pointerDrag = null;
            } else if (deltaSinceBegin.y > threshold)
            {
                GameplayManager.Instance.MoveItem(Index, Vector2Int.up);
                eventData.pointerDrag = null;
            } else if (deltaSinceBegin.y < -threshold)
            {
                GameplayManager.Instance.MoveItem(Index, Vector2Int.down);
                eventData.pointerDrag = null;
            }
        }

        #endregion

    }
}