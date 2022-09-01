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
            _SpriteRenderer.color = new Color(1f, 1f, 1f, 0f);
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