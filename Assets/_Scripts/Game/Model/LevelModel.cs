using System;
using Game.Gameplay.Item;

namespace Game.Model
{
	public class LevelModel
	{
		public int LevelNumber;
		public int GridWidth;
		public int GridHeight;
		public int MoveCount;
		public ItemType[] Grid;
	}
}
