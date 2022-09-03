﻿using Common.Context;
using Common.Scene.SceneInitializer.Bindings;
using UnityEngine;
using Utilities;

namespace Game.Gameplay
{
    public class GameplayInputManager : MonoBehaviour, IInitializable
    {
        private BoardSlot _draggingBoardSlot;
        private Camera _camera;
        private int _boardSlotLayerMask;
        private Vector2 _initialTouchPosition;
        private GameplayManager _gameplayManager;

        private void Awake()
        {
            _camera = ProjectContext.GetInstance<Camera>();
            _boardSlotLayerMask = LayerMask.GetMask(Constants.Layers.BoardSlot);
        }

        public void Initialize()
        {
            _gameplayManager = ProjectContext.GetInstance<GameplayManager>();
        }

        private void Update()
        {
            // TODO maybe use Unity's new input system
            if (Input.GetMouseButtonDown(0))
            {
                OnBeginDrag(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                OnDrag(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnEndDrag();
            }
        }

        private void SwipeDraggingBoardSlot(Vector2Int direction)
        {
            _gameplayManager.RequestSwipe(_draggingBoardSlot.Index, direction);
            OnEndDrag();
        }

        private void OnBeginDrag(Vector2 screenPosition)
        {
            var ray = _camera.ScreenPointToRay(screenPosition);
            var raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, _boardSlotLayerMask);
            if (raycastHit2D.collider == null) return;
            if (!raycastHit2D.collider.TryGetComponent<BoardSlot>(out var boardSlot)) return;

            _draggingBoardSlot = boardSlot;
            _initialTouchPosition = screenPosition;
        }

        private void OnDrag(Vector2 screenPosition)
        {
            if (_draggingBoardSlot == null) return;

            var deltaSinceBegin = screenPosition - _initialTouchPosition;
            var threshold = Constants.Gameplay.SwipeThreshold;
            if (deltaSinceBegin.x > threshold)
            {
                SwipeDraggingBoardSlot(Vector2Int.right);
            }
            else if (deltaSinceBegin.x < -threshold)
            {
                SwipeDraggingBoardSlot(Vector2Int.left);
            }
            else if (deltaSinceBegin.y > threshold)
            {
                SwipeDraggingBoardSlot(Vector2Int.up);
            }
            else if (deltaSinceBegin.y < -threshold)
            {
                SwipeDraggingBoardSlot(Vector2Int.down);
            }
        }

        private void OnEndDrag()
        {
            _draggingBoardSlot = null;
        }
    }
}