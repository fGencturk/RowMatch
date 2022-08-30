using Common.Context;
using Game.Data;
using LevelLoad.Data;
using UI.Loader;
using UnityEngine;

namespace Common.Scene.SceneEntry
{
    public class LoaderSceneInitializer : BaseSceneInitializer
    {
        [SerializeField] private BoardItemSpriteCatalog _BoardItemSpriteCatalog;
        [SerializeField] private LevelLoadCatalog _LevelLoadCatalog;
        [SerializeField] private LoadingView _LoadingView;

        protected override void InstallBindings()
        {
            BindInstance(new SceneLoadController());
            BindInstance(_BoardItemSpriteCatalog);
            BindInstance(_LevelLoadCatalog);
            BindInstance(_LoadingView);
        }

        protected override void Initialize()
        {
            ProjectContext.GetInstance<SceneLoadController>().LoadMenu();
        }
    }
}