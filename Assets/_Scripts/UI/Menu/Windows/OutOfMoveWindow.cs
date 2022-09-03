using Common.Event;
using Common.UI;
using Common.UI.Window;
using Common.UI.Window.Event;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Menu.Windows
{
    public class OutOfMoveWindow : Window
    {
        public override bool CanBlockerTriggerHide => false;

        private const float AnimationDuration = 1f;
        private const float InitialScale = .3f;
        private const float EndScale = 1f;
        
        [SerializeField] private TextMeshPro _Text;
        [SerializeField] private UIScaler _UIScaler;

        public override void Initialize()
        {
            _UIScaler.Rebuild();
        }

        public override void OnPreAppear(object data)
        {
            _Text.color = new Color(1f, 1f, 1f, 0);
            var textTransform = _Text.transform;
            
            var baseScale = textTransform.localScale;
            textTransform.localScale = baseScale * InitialScale;
            
            var seq = DOTween.Sequence()
                .Append(_Text.DOFade(1f, AnimationDuration))
                .Join(_Text.transform.DOScale(baseScale * EndScale, AnimationDuration))
                .AppendInterval(1.5f)
                .OnComplete(Hide);
        }

        private void Hide()
        {
            EventManager.Send(CloseWindowEvent.Create<OutOfMoveWindow>());
        }
    }
}