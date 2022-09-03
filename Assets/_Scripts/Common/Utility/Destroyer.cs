using UnityEngine;

namespace Common.Utility
{
    public class Destroyer : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}