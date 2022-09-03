using UnityEngine;

namespace Common.UI
{
    public class UIHelper2D
    {
        private static Vector2 _min;
        private static Vector2 _max;
        private static Vector2 _diff;

        public static void Initialize()
        {
            var camera = Camera.main;
            _min = camera.ScreenToWorldPoint(Vector3.zero);
            _max = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            _diff = _max - _min;
        }

        public static Vector2 AnchorToWorldPoint(float anchorX, float anchorY)
        {
            return _min + new Vector2(_diff.x * anchorX, _diff.y * anchorY);
        }

        public static Vector2 ScreenPercentageToWorldSize(float percentageX, float percentageY)
        {
            return new Vector2(percentageX * _diff.x, percentageY * _diff.y);
        }
    }
}