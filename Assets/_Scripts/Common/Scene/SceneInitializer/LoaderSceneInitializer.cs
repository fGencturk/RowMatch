using Common.Context;
using Common.Event;
using Common.Scene.Event;
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

        protected override void InstallBindings()
        {
            BindInstance(_BoardItemSpriteCatalog);
            BindInstance(_LevelLoadCatalog);
            BindInstance(_LoadingView);
            BindInstance(new SceneLoadController());
            BindInstance(new LevelLoadController());
        }

        protected override void Initialize()
        {
            EventManager.Send(OpenMenuSceneEvent.Create());
        }
    }
}