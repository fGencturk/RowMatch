using Common.Context;
using UI.Loader;
using UnityEngine.SceneManagement;
using Utilities;

namespace Common.Scene
{
    public class SceneLoadController
    {
        private string _lastSceneName;

        public void LoadGame()
        {
            LoadScene(Constants.Scene.Gameplay);
        }
        
        public void LoadMenu()
        {
            LoadScene(Constants.Scene.Menu);
        }
        
        private async void LoadScene(string sceneName)
        {
            var loadingView = ProjectContext.GetInstance<LoadingView>();
            
            if (_lastSceneName != null)
            {
                await loadingView.Show();
                var unloadOperation = SceneManager.UnloadSceneAsync(_lastSceneName);
                await TaskUtilities.WaitUntil(() => unloadOperation.isDone);
            }

            var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await TaskUtilities.WaitUntil(() => loadOperation.isDone);
            
            var loadedScene = SceneManager.GetSceneByName(sceneName);
            _lastSceneName = sceneName;
            SceneManager.SetActiveScene(loadedScene);
            
            await loadingView.Hide();

        }
    }
}