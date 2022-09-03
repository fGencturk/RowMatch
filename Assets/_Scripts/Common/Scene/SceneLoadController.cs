using System.Threading.Tasks;
using Common.Context;
using Common.Event;
using Common.Scene.Event;
using Common.Scene.SceneInitializer;
using Common.Scene.SceneInitializer.Bindings;
using Common.UI.Window.Event;
using Game.Gameplay;
using UI.Loader;
using UI.Menu.Windows;
using UnityEngine.SceneManagement;
using Utilities;

namespace Common.Scene
{
    public class SceneLoadController : IInitializable, IDisposable
    {
        private LoadingView _loadingView;
        private string _lastSceneName;

        public void Initialize()
        {
            _loadingView = ProjectContext.GetInstance<LoadingView>();
            EventManager.Register<OpenGameSceneEvent>(LoadGame);
            EventManager.Register<OpenMenuSceneEvent>(LoadMenu);
        }

        public void Dispose()
        {
            EventManager.Unregister<OpenGameSceneEvent>(LoadGame);
            EventManager.Unregister<OpenMenuSceneEvent>(LoadMenu);
        }
        
        private async void LoadGame(OpenGameSceneEvent data)
        {
            await LoadSceneUnderLoadingScene(Constants.Scene.Gameplay);
            ProjectContext.GetInstance<GameplayManager>().StartLevel(data.LevelModel);
            await _loadingView.Hide();
        }
        
        private async void LoadMenu(OpenMenuSceneEvent data)
        {
            await LoadSceneUnderLoadingScene(Constants.Scene.Menu);
            await _loadingView.Hide();
            if (data.TriggeredFromGame)
            {
                EventManager.Send(OpenWindowEvent.Create<CelebrationWindow>(data.GameEndEvent));
            }
        }

        private async Task LoadSceneUnderLoadingScene(string sceneName)
        {
            if (_lastSceneName != null)
            {
                await _loadingView.Show();
                var unloadOperation = SceneManager.UnloadSceneAsync(_lastSceneName);
                await TaskUtilities.WaitUntil(() => unloadOperation.isDone);
            }
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await TaskUtilities.WaitUntil(() => loadOperation.isDone);
            
            var loadedScene = SceneManager.GetSceneByName(sceneName);
            _lastSceneName = sceneName;
            SceneManager.SetActiveScene(loadedScene);
        }
    }
}