using System;
using System.Collections.Generic;
using Common.Context;
using Common.Scene.SceneInitializer.Bindings;
using UnityEngine;
using IDisposable = Common.Scene.SceneInitializer.Bindings.IDisposable;

namespace Common.Scene.SceneInitializer
{
    public abstract class BaseSceneInitializer : MonoBehaviour
    {
        private List<IInitializable> _initializables = new List<IInitializable>();
        private List<IDisposable> _disposables = new List<IDisposable>();
        private List<object> _bindObjects = new List<object>();
        
        protected abstract void InstallBindings();
        protected abstract void Initialize();

        public void Awake()
        {
            InstallBindings();
            foreach (var initializable in _initializables)
            {
                initializable.Initialize();
            }
            Initialize();
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            foreach (var bindObject in _bindObjects)
            {
                ProjectContext.RemoveInstance(bindObject);
            }
        }

        protected void BindInstance<T>(T instance)
        {
            ProjectContext.BindInstance(instance);
            _bindObjects.Add(instance);
            if (instance is IInitializable initializable)
            {
                _initializables.Add(initializable);
            }

            if (instance is IDisposable disposable)
            {
                _disposables.Add(disposable);
            }
        }
    }
}