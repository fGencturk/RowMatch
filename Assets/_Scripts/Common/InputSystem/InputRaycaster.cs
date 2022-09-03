using Common.Context;
using Common.InputSystem.Handlers;
using UnityEngine;

namespace Common.InputSystem
{
    public class InputRaycaster : MonoBehaviour
    {
        private RaycastHit2D[] _results = new RaycastHit2D[10];
        private EventData _eventData = new EventData();
        private Camera _camera;
        private int _count;

        private void Start()
        {
            _camera = ProjectContext.GetInstance<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _eventData.ReInitialize(Input.mousePosition);
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                _count = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, _results);
                for (var i = 0; i < _count; i++)
                {
                    if (!_results[i].collider.TryGetComponent<IClickable>(out var clickable)) continue;
                    
                    // EventData is automatically consumed, clickable decides if we should delegate below
                    _eventData.Consume();
                    clickable.OnTouchDown(_eventData);
                    if (_eventData.IsConsumed) return;
                }
            } 
            else if (Input.GetMouseButton(0))
            {
                _eventData.ReInitialize(Input.mousePosition);
                for (var i = 0; i < _count; i++)
                {
                    if (!_results[i].collider.TryGetComponent<IDraggable>(out var clickable)) continue;
                
                    // EventData is automatically consumed, clickable decides if we should delegate below
                    _eventData.Consume();
                    clickable.OnDrag(_eventData);
                    if (_eventData.IsConsumed) return;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _eventData.ReInitialize(Input.mousePosition);
                for (var i = 0; i < _count; i++)
                {
                    if (!_results[i].collider.TryGetComponent<IClickable>(out var clickable)) continue;
                    
                    // EventData is automatically consumed, clickable decides if we should delegate below
                    _eventData.Consume();
                    clickable.OnTouchUp(_eventData);
                    if (_eventData.IsConsumed) return;
                }
            }
        }
    }
}