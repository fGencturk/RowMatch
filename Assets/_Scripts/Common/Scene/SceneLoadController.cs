﻿using System.Threading.Tasks;
using Common.Context;
using Common.Event;
using Common.Scene.Event;
using Common.Scene.SceneInitializer;
using Game.Gameplay;
using UI.Loader;
using UnityEngine.SceneManagement;
using Utilities;

namespace Common.Scene
{
    public class SceneLoadController : IInitializable
    {
        private LoadingView _loadingView;
        private string _lastSceneName;

        public void Initialize()
        {
            _loadingView = ProjectContext.GetInstance<LoadingView>();
            EventManager.Register<OpenGameSceneEvent>(LoadGame);
            EventManager.Register<OpenMenuSceneEvent>(LoadMenu);
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