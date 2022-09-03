using Game.Gameplay;
using UnityEngine;

namespace Common.Scene.SceneInitializer
{
    public class GameSceneInitializer : BaseSceneInitializer
    {

        [SerializeField] private GameScenePrefabsInstantiator _GameScenePrefabsInstantiator;
        [SerializeField] private GameplayInputManager _GameplayInputManager;
        
        protected override void InstallBindings()
        { 
            _GameScenePrefabsInstantiator.InstantiatePrefabs(out var gameplayManager, out var gameplayUI);
            BindInstance(gameplayManager);
            BindInstance(_GameplayInputManager);
            BindInstance(new ScoreManager());
            BindInstance(new EndGameController());
        }

        protected override void Initialize() { }
    }
}