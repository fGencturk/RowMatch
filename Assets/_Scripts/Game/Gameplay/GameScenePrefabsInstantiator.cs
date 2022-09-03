using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameScenePrefabsInstantiator : MonoBehaviour
    {
        [Serializable]
        private class GameScenePrefabs
        {
            public GameplayManager GameplayManager;
            public GameObject GameplayUI;
        }

        [SerializeField] private GameScenePrefabs _LandscapePrefabs;
        [SerializeField] private GameScenePrefabs _PortraitPrefabs;

        public void InstantiatePrefabs(out GameplayManager gameplayManager, out GameObject gameplayUI)
        {
            var screenRatio = Screen.width / Screen.height;
            var prefabs = screenRatio > 0 ? _LandscapePrefabs : _PortraitPrefabs;

            gameplayManager = Instantiate(prefabs.GameplayManager, transform);
            gameplayUI = Instantiate(prefabs.GameplayUI, transform);
        }
        
    }
}