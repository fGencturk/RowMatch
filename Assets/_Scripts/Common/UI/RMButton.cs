using Common.InputSystem;
using Common.InputSystem.Handlers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Common.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class RMButton : MonoBehaviour, IClickable
    {

        private const float ScaleDuration = .1f;
        private const float ClickScale = .95f;

        [Header("When false, other below colliders can also be clicked & dragged.")]
        [SerializeField] private bool _ConsumeEvents = true;
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
        
        public void OnTouchDown(EventData eventData)
        {
            _Container.DOComplete();
            _Container.DOScale(ClickScale, ScaleDuration);
            _holdingClick = true;
            if (_ConsumeEvents) return;
            eventData.DelegateBelow();
        }

        public void OnTouchUp(EventData eventData)
        {
            if (_holdingClick)
            {
                _OnClick.Invoke();
            }
            ResetView();
            if (_ConsumeEvents) return;
            eventData.DelegateBelow();
        }

        private void OnMouseExit()
        {
            if (_holdingClick)
            {
                ResetView();
            }
        }

        private void ResetView()
        {
            _holdingClick = false;
            _Container.DOComplete();
            _Container.DOScale(1f, ScaleDuration);
        }
    }
}