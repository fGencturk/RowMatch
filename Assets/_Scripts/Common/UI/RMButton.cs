using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Common.UI
{
    public class RMButton : MonoBehaviour
    {

        private const float ScaleDuration = .1f;
        private const float ClickScale = .95f;

        [SerializeField] private Transform _Container;
        [SerializeField] private UnityEvent _OnClick = new UnityEvent();

        private bool _holdingClick;

        #region Properties

        public UnityEvent OnClick => _OnClick;

        #endregion

        private void OnMouseDown()
        {
            _Container.DOComplete();
            _Container.DOScale(ClickScale, ScaleDuration);
            _holdingClick = true;
        }

        void OnMouseExit()
        {
            if (_holdingClick)
            {
                ResetView();
            }
        }

        private void OnMouseUp()
        {
            if (_holdingClick)
            {
                _OnClick.Invoke();
            }
            ResetView();
        }

        void ResetView()
        {
            _holdingClick = false;
            _Container.DOComplete();
            _Container.DOScale(1f, ScaleDuration);
        }
    }
}