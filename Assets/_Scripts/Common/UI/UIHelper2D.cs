using Common.Context;
using UnityEngine;

namespace Common.UI
{
    public class UIHelper2D
    {
        public static Vector2 ScreenBottomLeftInWorldUnits { get; private set; }
        public static Vector2 ScreenTopRightInWorldUnits { get; private set; }
        public static Vector2 ScreenSizeInWorldUnits { get; private set; }

        public static void Initialize()
        {
            var camera = ProjectContext.GetInstance<Camera>();
            ScreenBottomLeftInWorldUnits = camera.ScreenToWorldPoint(Vector3.zero);
            ScreenTopRightInWorldUnits = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            ScreenSizeInWorldUnits = ScreenTopRightInWorldUnits - ScreenBottomLeftInWorldUnits;
        }

        public static Vector2 AnchorToWorldPoint(float anchorX, float anchorY)
        {
            return ScreenBottomLeftInWorldUnits + new Vector2(ScreenSizeInWorldUnits.x * anchorX, ScreenSizeInWorldUnits.y * anchorY);
        }

        public static Vector2 ScreenPercentageToWorldSize(float percentageX, float percentageY)
        {
            return new Vector2(percentageX * ScreenSizeInWorldUnits.x, percentageY * ScreenSizeInWorldUnits.y);
        }
    }
}