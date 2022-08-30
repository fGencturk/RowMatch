using System;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Common.Context
{
    public class ProjectContext : MonoBehaviour
    {

        private Dictionary<Type, object> _context;

        private static ProjectContext _singleton;

        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
                return;
            }
            _singleton = this;
            _context = new Dictionary<Type, object>();
        }

        private void OnDestroy()
        {
            _singleton = null;
        }

        public static void BindInstance<T>(T instance)
        {
            BindInstanceTo<T>(instance);
        }
        
        public static void BindInstanceTo<T>(object instance)
        {
            _singleton._context[typeof(T)] = instance;
            if (instance is IInitializable initializable)
            {
                initializable.Initialize();
            }
        }

        public static T GetInstance<T>()
        {
            return (T)_singleton.GetInstance(typeof(T));
        }

        private object GetInstance(Type type)
        {
            if (_context.ContainsKey(type))
            {
                return _context[type];
            }

            return null;
        }
    }
}