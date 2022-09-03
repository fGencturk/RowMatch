using Common.UI.Element;
using UnityEngine;
using Utilities;

namespace Game.Gameplay
{
    public class BoardSizeProvider : BaseUISizeProvider
    {
        public Vector2 GridSize { get; set; }
        public override Vector2 BaseSize => Vector2.Scale(GridSize, Constants.Gameplay.BoardSlotSize);
    }
}