using Game.Gameplay;
using UnityEngine;

namespace Common.Scene.SceneInitializer
{
    public class GameSceneInitializer : BaseSceneInitializer
    {

        [SerializeField] private GameplayManager _GameplayManager;
        [SerializeField] private InputManager _InputManager;
        
        protected override void InstallBindings()
        {
            BindInstance(_GameplayManager);
            BindInstance(_InputManager);
            BindInstance(new ScoreManager());
            BindInstance(new EndGameController());
        }

        protected override void Initialize() { }
    }
}