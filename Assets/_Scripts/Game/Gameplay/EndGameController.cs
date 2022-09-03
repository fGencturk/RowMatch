using Common.Event;
using Common.Scene.Event;
using Common.Scene.SceneInitializer.Bindings;
using Common.UI.Window.Event;
using Game.Gameplay.Event;
using UI.Menu.Windows;

namespace Game.Gameplay
{
    public class EndGameController : IInitializable
    {

        private GameEndEvent _gameEndEvent;
        
        public void Initialize()
        {
            EventManager.Register<GameEndEvent>(OnGameEnd);
        }

        private void OnGameEnd(GameEndEvent data)
        {
            _gameEndEvent = data;
            EventManager.Unregister<GameEndEvent>(OnGameEnd);
            EventManager.Register<CloseWindowEvent>(OnCloseWindowEvent);
            EventManager.Send(OpenWindowEvent.Create<OutOfMoveWindow>());
        }

        private void OnCloseWindowEvent(CloseWindowEvent data)
        {
            EventManager.Unregister<CloseWindowEvent>(OnCloseWindowEvent);
            EventManager.Send(OpenMenuSceneEvent.Create(_gameEndEvent));
        }
    }
}