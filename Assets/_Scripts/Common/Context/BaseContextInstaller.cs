using UnityEngine;

namespace Common.Context
{
    public abstract class BaseContextInstaller : MonoBehaviour
    {
        private void Awake()
        {
            Install();
        }

        protected abstract void Install();
    }
}