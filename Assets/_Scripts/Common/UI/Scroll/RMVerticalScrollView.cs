using System.Collections.Generic;
using Common.UI.Element;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;

namespace Common.UI.Scroll
{
    public class RMVerticalScrollView : MonoBehaviour
    {

        [SerializeField] private SpriteMask _SpriteMask;
        [SerializeField] private Transform _Content;

        [Header("Animation Properties")] 
        [SerializeField] private float _SnapDuration = .2f;
        [SerializeField] private float _ScrollDuration = .5f;
        [SerializeField] private float _InitialScrollMultiplier = .3f;
        [SerializeField] private AnimationCurve _GravityCurve = AnimationCurve.Linear(0f, 1f, 0f, 1f);

        private List<IRMUIElement> _elements;
        private Sequence _snapSequence;
        private Vector3 _lastMousePosition;
        private Camera _camera;
        private IRMUIElement _contentUIElement;

        #region Properties

        private float ContentHeight => _contentUIElement.Size.y;
        private float ViewportHeight => _SpriteMask.bounds.max.y;

        public float MaxHeight => Mathf.Max(0f, ContentHeight - ViewportHeight);

        #endregion

        private void Awake()
        {
            _contentUIElement = _Content.GetComponent<IRMUIElement>();
            _Content.transform.localPosition = new Vector3(0, ViewportHeight);
            _camera = Camera.main;
        }

        private void OnMouseDown()
        {
            _snapSequence?.Kill();
            _lastMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);;
        }

        private void OnMouseDrag()
        {
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var delta = _lastMousePosition - mouseWorldPosition;
            _Content.localPosition = new Vector3(0, _Content.localPosition.y - delta.y);
            _lastMousePosition = mouseWorldPosition;
        }

        private void OnMouseUp()
        {
            //if (ClampContentPosition()) return;
            var mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var delta = _lastMousePosition - mouseWorldPosition;
            ContinueScrolling(delta.y);
        }

        private bool ClampContentPosition()
        {
            if (_Content.localPosition.y < 0 || _Content.localPosition.y > MaxHeight)
            {
                SnapToPosition(Mathf.Clamp(_Content.localPosition.y, 0f, MaxHeight));
                return true;
            }

            return false;
        }

        public void SnapToPosition(float position)
        {
            _snapSequence?.Kill();
            _snapSequence = DOTween.Sequence()
                .Append(_Content.DOLocalMoveY(position, _SnapDuration).SetEase(Ease.OutSine));
        }

        public async void ContinueScrolling(float lastYDelta)
        {
            lastYDelta *= _InitialScrollMultiplier;
            _snapSequence?.Kill();
            var startTime = Time.time;
            var velocity = lastYDelta;
            
            while (startTime + _ScrollDuration > Time.time)
            {
                _Content.localPosition = new Vector3(0, _Content.localPosition.y - velocity);
                Debug.Log(velocity);

                var normalizedTime = (Time.time - startTime) / _ScrollDuration;
                var velocityLost = _GravityCurve.Evaluate(normalizedTime) * lastYDelta;
                velocity = lastYDelta - velocityLost;
                
                await Task.Yield();
            }

            ClampContentPosition();

        }
        
    }
}