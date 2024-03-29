﻿using System;
using System.Collections.Generic;
using Common.Event;
using Common.UI.Window.Event;
using UnityEngine;

namespace Common.UI.Window
{
    public class WindowManager : MonoBehaviour
    {

        [SerializeField] private Window[] _WindowPrefabs;
        [SerializeField] private WindowBlocker _Blocker;
        [SerializeField] private Transform _Content;

        private Dictionary<Type, Window> _typeToWindow;
        public Window ActiveWindow { get; private set; }

        public void Awake()
        {
            EventManager.Register<OpenWindowEvent>(OnOpenWindowRequested);
            EventManager.Register<CloseWindowEvent>(OnCloseWindowRequested);
            
            _typeToWindow = new Dictionary<Type, Window>();
            // Eagerly instantiate one instance for each window
            foreach (var windowPrefab in _WindowPrefabs)
            {
                var window = Instantiate(windowPrefab, _Content);
                _typeToWindow[windowPrefab.GetType()] = window;
                window.Initialize();
                window.gameObject.SetActive(false);
            }
            
            _Blocker.OnClick.AddListener(CloseActiveWindow);
        }

        private void OnDestroy()
        {
            EventManager.Unregister<OpenWindowEvent>(OnOpenWindowRequested);
            EventManager.Unregister<CloseWindowEvent>(OnCloseWindowRequested);
        }

        private void OnOpenWindowRequested(OpenWindowEvent data)
        {
            if (!_typeToWindow.ContainsKey(data.Type))
            {
                Debug.LogError($"{data.Type} could not found in windows list.");
                return;
            }

            var window = _typeToWindow[data.Type];
            _Blocker.Interactable = window.CanBlockerTriggerHide;
            _Blocker.Show();
            window.OnPreAppear(data.Data);
            window.gameObject.SetActive(true);
            ActiveWindow = window;
        }

        private void OnCloseWindowRequested(CloseWindowEvent data)
        {
            if (ActiveWindow == null || ActiveWindow.GetType() != data.Type)
            {
                Debug.LogError($"{data.Type} is not active window.");
                return;
            }
            CloseActiveWindow();
        }

        private void CloseActiveWindow()
        {
            _Blocker.Hide();
            _Blocker.Interactable = false;
            ActiveWindow.gameObject.SetActive(false);
            ActiveWindow = null;
        }
    }
}