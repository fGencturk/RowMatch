using Common.Event;
using Common.Save;
using Common.Scene.Event;
using Common.UI;
using Game.Data;
using LevelLoad;
using LevelLoad.Data;
using UI.Loader;
using UnityEngine;

namespace Common.Scene.SceneInitializer
{
    public class LoaderSceneInitializer : BaseSceneInitializer
    {
        [SerializeField] private BoardItemSpriteCatalog _BoardItemSpriteCatalog;
        [SerializeField] private LevelLoadCatalog _LevelLoadCatalog;
        [SerializeField] private LoadingView _LoadingView;
        [SerializeField] private Camera _Camera;

        protected override void InstallBindings()
        {
            BindInstance(_Camera);
            BindInstance(_BoardItemSpriteCatalog);
            BindInstance(_LevelLoadCatalog);
            BindInstance(_LoadingView);
            BindInstance(new SceneLoadController());
            BindInstance(new LevelLoadController());
            BindInstance(new PlayerData());
            UIHelper2D.Initialize();
            
            // After game starts portrait/landscape lock orientation
            // TODO what about splash?
            if (Screen.width > Screen.height)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
            }
            else
            {
                Screen.orientation = ScreenOrientation.Portrait;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
            }

            Application.targetFrameRate = 60;
        }

        protected override void Initialize()
        {
            EventManager.Send(OpenMenuSceneEvent.Create());
        }
    }
}