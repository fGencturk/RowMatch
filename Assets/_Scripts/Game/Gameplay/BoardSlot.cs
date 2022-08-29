using Common;
using Game.Data;
using Game.Gameplay.Item;
using UnityEngine;

namespace Game.Gameplay
{
    public class BoardSlot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _ItemSprite;
        
        public IBoardItem BoardItem;
        [HideInInspector] 
        public bool IsLocked;
        
        public string ItemType { get; private set; }

        public void Initialize(int row, int column, ItemType itemType)
        {
            transform.position = new Vector3(column, row);
            var spriteCatalog = ProjectContext.GetInstance<BoardItemSpriteCatalog>();
            _ItemSprite.sprite = spriteCatalog.GetSprite(itemType);
        }
    }
}