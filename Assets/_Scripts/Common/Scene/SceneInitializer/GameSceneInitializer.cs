using Game.Gameplay;
using UnityEngine;

namespace Common.Scene.SceneInitializer
{
    public class GameSceneInitializer : BaseSceneInitializer
    {

        [SerializeField] private GameScenePrefabsInstantiator _GameScenePrefabsInstantiator;
        [SerializeField] private InputManager _InputManager;
        
        protected override void InstallBindings()
        { 
            _GameScenePrefabsInstantiator.InstantiatePrefabs(out var gameplayManager, out var gameplayUI);
            BindInstance(gameplayManager);
            BindInstance(_InputManager);
            BindInstance(new ScoreManager());
            BindInstance(new EndGameController());
        }

        protected override void Initialize() { }
    }
}