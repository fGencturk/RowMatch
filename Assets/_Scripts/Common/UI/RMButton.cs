using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Common.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class RMButton : MonoBehaviour
    {

        private const float ScaleDuration = .1f;
        private const float ClickScale = .95f;

        [SerializeField] private Transform _Container;
        [SerializeField] private UnityEvent _OnClick = new UnityEvent();

        private bool _holdingClick;
        private bool _interactable;
        private Collider2D _collider;

        #region Properties

        public UnityEvent OnClick => _OnClick;

        public bool Interactable
        {
            get => _interactable;
            set => SetInteractable(value);
        }

        #endregion

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        
        private void SetInteractable(bool value)
        {
            _interactable = value;
            _collider.enabled = _interactable;
        }
        private void OnMouseDown()
        {
            _Container.DOComplete();
            _Container.DOScale(ClickScale, ScaleDuration);
            _holdingClick = true;
        }

        private void OnMouseExit()
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

        private void ResetView()
        {
            _holdingClick = false;
            _Container.DOComplete();
            _Container.DOScale(1f, ScaleDuration);
        }
    }
}