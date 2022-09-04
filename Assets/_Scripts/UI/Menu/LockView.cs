using Common.UI;
using DG.Tweening;
using UnityEngine;
using Utilities.Extensions;

namespace UI.Menu
{
    public class LockView : MonoBehaviour
    {
        [Header("Lock Sprites")] 
        [SerializeField] private SpriteRenderer _LockRenderer;
        [SerializeField] private Sprite _LockSprite;
        [SerializeField] private Sprite _UnlockSprite;
        
        public float ScaleDuration = .1f;
        public float XScaleDown = .8f;
        public float YScaleUp = 1.2f;

        public float YMoveUp = 2f;
        public float XTranslate = 1.5f;
        public float Rotate = -30;
        public float GoingUpDuration = .3f;
        public float DropDuration = .8f;
        public float FadeOutDuration = .2f;

        public void AnimateUnlock(LevelEntryView levelEntryView)
        {
            ResetLockView(levelEntryView);
            levelEntryView.LockRenderer.gameObject.SetActive(false);
            var lockBackgroundRenderer = levelEntryView.LockBackgroundRenderer;
            lockBackgroundRenderer.gameObject.SetActive(true);
            
            var lockTransform = _LockRenderer.transform;
            
            lockTransform.gameObject.SetActive(true);
            
            var initialScale = lockTransform.localScale;
            var initialPosition = lockTransform.localPosition;

            var yTranslateSeq = DOTween.Sequence()
                .Append(lockTransform.DOMoveY(initialPosition.y + YMoveUp, GoingUpDuration).SetEase(Ease.OutSine))
                .Append(lockTransform.DOMoveY(initialPosition.y - UIHelper2D.ScreenSizeInWorldUnits.y,
                    DropDuration - GoingUpDuration).SetEase(Ease.InSine));

            var backgroundFadeOutSeq = DOTween.Sequence()
                .Append(lockBackgroundRenderer.DOFade(0f, FadeOutDuration))
                .AppendCallback(() =>
                {
                    lockBackgroundRenderer.gameObject.SetActive(false);
                    lockBackgroundRenderer.SetAlpha(1f);
                });

            var seq = DOTween.Sequence()
                .Append(lockTransform.DOScaleX(XScaleDown * initialScale.x, ScaleDuration))
                .Join(lockTransform.DOScaleY(YScaleUp * initialScale.y, ScaleDuration))
                .AppendCallback(() =>
                {
                    _LockRenderer.sprite = _UnlockSprite;
                })
                .Append(lockTransform.DOMoveX(initialPosition.x + XTranslate, DropDuration))
                .Join(lockTransform.DORotate(new Vector3(0, 0, Rotate), DropDuration).SetEase(Ease.OutCubic))
                .Join(lockTransform.DOScale(initialScale, ScaleDuration))
                .Join(yTranslateSeq)
                .Join(backgroundFadeOutSeq)
                .OnComplete(() =>
                {
                    lockTransform.gameObject.SetActive(false);
                });
        }

        private void ResetLockView(LevelEntryView levelEntryView)
        {
            var lockTransform = _LockRenderer.transform;
            var mockingLockTransform = levelEntryView.LockRenderer.transform;
            lockTransform.position = mockingLockTransform.position;
            lockTransform.localScale = mockingLockTransform.lossyScale;
            lockTransform.rotation = mockingLockTransform.rotation;
            _LockRenderer.sprite = _LockSprite;
        }
    }
}