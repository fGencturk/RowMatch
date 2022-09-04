using Common.Context;
using DG.Tweening;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay.Item
{
    public class BoardItem : MonoBehaviour
    {
        private const float HighlightAnimDuration = .15f;
        private static readonly int ColorId = Shader.PropertyToID("_AdditiveColor");
        private static readonly Color HighlightAdditiveColor = new Color(.2f, .2f, .2f);
        private static readonly Color InitialColor = new Color(0, 0, 0);

        private float NotPossibleSwipeMove => .15f * _ItemSprite.bounds.size.x;
        private const float NotPossibleSwipeDuration = .1f;

        [SerializeField] private SpriteRenderer _ItemSprite;
        
        private Sequence _highlightSequence;

        #region Properties

        public ItemType ItemType { get; private set; }

        #endregion

        public void Initialize(ItemType itemType)
        {
            ItemType = itemType;
            var spriteCatalog = ProjectContext.GetInstance<BoardItemSpriteCatalog>();
            _ItemSprite.sprite = spriteCatalog.GetSprite(itemType);
            SetColor(InitialColor);
        }

        public void Highlight()
        {
            _highlightSequence?.Kill();
            _highlightSequence = DOTween.Sequence(DOTween.To(GetColor, SetColor, HighlightAdditiveColor, HighlightAnimDuration));
        }

        public void DisableHighlight()
        {
            _highlightSequence?.Kill();
            _highlightSequence = DOTween.Sequence(DOTween.To(GetColor, SetColor, InitialColor, HighlightAnimDuration));
        }

        private Color GetColor()
        {
            return _ItemSprite.material.GetColor(ColorId);
        }

        private void SetColor(Color color)
        {
            _ItemSprite.material.SetColor(ColorId, color);
        }

        public void AnimateNotPossibleSwipe(Vector2Int direction)
        {
            transform.DOKill();
            var seq = DOTween.Sequence()
                .Append(transform.DOLocalMove((Vector2)direction * NotPossibleSwipeMove, NotPossibleSwipeDuration))
                .Append(transform.DOLocalMove(Vector3.zero, NotPossibleSwipeDuration))
                .SetTarget(transform);
        }
    }
}