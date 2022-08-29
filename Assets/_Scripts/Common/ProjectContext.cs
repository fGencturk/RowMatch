using System;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Common
{
    public class ProjectContext : MonoBehaviour
    {
        [SerializeField] private BoardItemSpriteCatalog _BoardItemSpriteCatalog;

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
            // TODO bind to interface?
            // TODO create installer
            _BoardItemSpriteCatalog.Initialize();
            _context[_BoardItemSpriteCatalog.GetType()] = _BoardItemSpriteCatalog;
        }

        private void OnDestroy()
        {
            _singleton = null;
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