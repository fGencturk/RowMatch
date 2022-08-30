using Common.Context;
using Game.Data;
using LevelLoad.Data;
using UnityEngine;

namespace Common.Scene.Installer
{
    public class LoaderSceneInstaller : BaseContextInstaller
    {
        [SerializeField] private BoardItemSpriteCatalog _BoardItemSpriteCatalog;
        [SerializeField] private LevelLoadCatalog _LevelLoadCatalog;

        protected override void Install()
        {
            ProjectContext.BindInstance(_BoardItemSpriteCatalog);
            ProjectContext.BindInstance(_LevelLoadCatalog);
        }
    }
}