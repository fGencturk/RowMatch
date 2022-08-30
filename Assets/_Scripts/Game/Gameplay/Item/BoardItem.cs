using Common;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay.Item
{
    public class BoardItem : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer _ItemSprite;

        #region Properties

        public ItemType ItemType { get; private set; }

        #endregion

        public void Initialize(ItemType itemType)
        {
            ItemType = itemType;
            var spriteCatalog = ProjectContext.GetInstance<BoardItemSpriteCatalog>();
            _ItemSprite.sprite = spriteCatalog.GetSprite(itemType);
        }
        
    }
}