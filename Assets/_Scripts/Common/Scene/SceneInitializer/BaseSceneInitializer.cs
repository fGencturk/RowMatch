using System.Collections.Generic;
using Common.Context;
using UnityEngine;

namespace Common.Scene.SceneInitializer
{
    public abstract class BaseSceneInitializer : MonoBehaviour
    {
        protected List<IInitializable> _initializables = new List<IInitializable>();
        
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

        // TODO keep record of binded instances and remove them OnDestroy
        protected void BindInstance<T>(T instance)
        {
            ProjectContext.BindInstance(instance);
            if (instance is IInitializable initializable)
            {
                _initializables.Add(initializable);
            }
        }
    }
}