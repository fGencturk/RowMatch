using System.Collections.Generic;
using System.Threading;
using Common.UI.Element;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;
using Common.Context;
using Common.InputSystem;
using Common.InputSystem.Handlers;

namespace Common.UI.Scroll
{
    public class RMVerticalScrollView : MonoBehaviour, IClickable, IDraggable
    {

        [SerializeField] private BaseUISizeProvider _ViewPortSizeProvider;
        [SerializeField] private Transform _Content;

        [Header("Animation Properties")] 
        [SerializeField] private float _SnapDuration = .2f;
        [SerializeField] private float _ScrollDuration = .5f;
        [SerializeField] private float _InitialScrollMultiplier = .3f;
        [SerializeField] private AnimationCurve _GravityCurve = AnimationCurve.Linear(0f, 1f, 0f, 1f);
        [Header("Overshoot")]
        [SerializeField] private float _MaxOvershoot = 1.5f;

        private List<BaseUISizeProvider> _elements;
        private Vector3 _lastMousePosition;
        private Camera _camera;
        private BaseUISizeProvider _contentUIElement;
        private float _velocity;

        private Sequence _snapSequence;
        private CancellationTokenSource _scrollCancellationTokenSource;
        
        #region Properties

        private float ContentHeight => _contentUIElement.BaseSize.y;
        private float ViewportHeight => _ViewPortSizeProvider.BaseSize.y;

        public float MaxHeight => Mathf.Max(0f, ContentHeight - ViewportHeight);
        public Transform Content => _Content;

        #endregion

        public void Initialize()
        {
            _contentUIElement = _Content.GetComponent<BaseUISizeProvider>();
            _Content.transform.localPosition = new Vector3(0, ViewportHeight);
            _camera = ProjectContext.GetInstance<Camera>();
        }

        private void SetPosition(float velocity)
        {
            _velocity = velocity;
            ApplyGravityToVelocityIfOvershoot();

            var localPosition = _Content.localPosition;
            _Content.localPosition = new Vector3(localPosition.x, localPosition.y + _velocity);
        }

        private void ApplyGravityToVelocityIfOvershoot()
        {
            var velocity = _velocity;
            float GetSlowedVelocityDueToOvershoot(float remainingOvershoot)
            {
                var velocityMultiplier = remainingOvershoot / _MaxOvershoot;
                return velocity * velocityMultiplier;
            }
            
            var localPosition = _Content.localPosition;
            
            // If already overshoot and also scrolling in overshoot direction
            if (localPosition.y < 0 && velocity < 0)
            {
                var remainingOvershoot = Mathf.Max(0f, _MaxOvershoot - Mathf.Abs(localPosition.y));
                _velocity = GetSlowedVelocityDueToOvershoot(remainingOvershoot);
            } else if (localPosition.y > MaxHeight && velocity > 0)
            {
                var remainingOvershoot = Mathf.Max(0f, _MaxOvershoot - Mathf.Abs(localPosition.y - MaxHeight));
                _velocity = GetSlowedVelocityDueToOvershoot(remainingOvershoot);
            }
        }

        #region Input Events

        public void OnTouchDown(EventData eventData)
        {
            KillAllAnimations();
            _lastMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);;
        }

        public void OnTouchUp(EventData eventData)
        {
            //if (ClampContentPosition()) return;
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var delta = _lastMousePosition - mouseWorldPosition;
            ContinueScrolling(delta.y);
        }

        public void OnDrag(EventData eventData)
        {
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var delta = _lastMousePosition - mouseWorldPosition;
            SetPosition(-delta.y);
            _lastMousePosition = mouseWorldPosition;
        }

        #endregion

        private bool IsOutsideVisibleArea()
        {
            var localPosition = _Content.localPosition;
            return localPosition.y < 0 || localPosition.y > MaxHeight;
        }
        private bool ClampContentPosition()
        {
            if (IsOutsideVisibleArea())
            {
                SnapToPosition(Mathf.Clamp(_Content.localPosition.y, 0f, MaxHeight));
                return true;
            }

            return false;
        }
        
        public void TeleportToPosition(float position)
        {
            KillAllAnimations();
            position = Mathf.Clamp(position, 0f, MaxHeight);
            _Content.localPosition = new Vector3(_Content.localPosition.x, position);
        }

        #region Animations

        private void KillAllAnimations()
        {
            _snapSequence?.Kill();
            _snapSequence = null;
            _scrollCancellationTokenSource?.Cancel();
            _scrollCancellationTokenSource = null;
        }
        
        public void SnapToPosition(float position)
        {
            KillAllAnimations();
            _snapSequence = DOTween.Sequence()
                .Append(_Content.DOLocalMoveY(position, _SnapDuration).SetEase(Ease.OutSine));
        }
        
        private async void ContinueScrolling(float last)
        {
            KillAllAnimations();
            _scrollCancellationTokenSource = new CancellationTokenSource();
            var token = _scrollCancellationTokenSource.Token;
            
            _snapSequence?.Kill();
            var startTime = Time.time;

            _velocity = -last * _InitialScrollMultiplier;
            var initialVelocity = _velocity;
            var totalVelocityLostDueToOvershoot = 0f;
            
            while (startTime + _ScrollDuration > Time.time)
            {
                var requestedVelocity = _velocity;
                SetPosition(requestedVelocity);
                var appliedVelocity = _velocity;
                
                var normalizedTime = (Time.time - startTime) / _ScrollDuration;
                var totalVelocityLostDueToTime = -_GravityCurve.Evaluate(normalizedTime) * initialVelocity;
                totalVelocityLostDueToOvershoot += appliedVelocity - requestedVelocity;

                var totalVelocityLost = totalVelocityLostDueToTime + totalVelocityLostDueToOvershoot;
                if (Mathf.Abs(totalVelocityLost) > Mathf.Abs(_velocity)) break;
                
                _velocity = initialVelocity + totalVelocityLost;
                
                if (Mathf.Abs(_velocity) < float.Epsilon) break;

                await Task.Yield();
                if (token.IsCancellationRequested) return;
            }

            ClampContentPosition();

        }

        #endregion
    }
}