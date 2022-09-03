using DG.Tweening;
using UnityEngine;

namespace Common.UI.Window
{
    public class WindowBlocker : RMButton
    {
        private const float FadeDuration = .2f;

        [SerializeField] private SpriteRenderer _SpriteRenderer;
        private float _initialAlpha;

        private void Start()
        {
            _initialAlpha = _SpriteRenderer.color.a;
            gameObject.SetActive(false);
            var color = _SpriteRenderer.color;
            color.a = 0f;
            _SpriteRenderer.color = color;
        }

        public void Show()
        {
            _SpriteRenderer.DOKill();
            _SpriteRenderer.gameObject.SetActive(true);
            _SpriteRenderer.DOFade(_initialAlpha, FadeDuration);
        }

        public void Hide()
        {
            _SpriteRenderer.DOKill();
            _SpriteRenderer.DOFade(0f, FadeDuration)
                .OnComplete(() => _SpriteRenderer.gameObject.SetActive(false));
        }
    }
}