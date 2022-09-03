using UnityEngine;

namespace Common.UI.Window
{
    public class Window : MonoBehaviour
    {
        public virtual bool CanBlockerTriggerHide => true;
        
        public virtual void Initialize() { }
        public virtual void OnPreAppear(object data) { }
    }
}