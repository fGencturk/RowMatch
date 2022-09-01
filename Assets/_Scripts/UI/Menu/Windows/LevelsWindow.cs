using Common.Context;
using Common.UI.LayoutGroup;
using Common.UI.Scroll;
using Common.UI.Window;
using LevelLoad;
using UnityEngine;

namespace UI.Menu.Windows
{
    public class LevelsWindow : Window
    {

        [SerializeField] private LevelEntryView _LevelEntryViewPrefab;
        [SerializeField] private RMVerticalLayoutGroup _VerticalLayoutGroup;
        [SerializeField] private RMVerticalScrollView _ScrollView;

        public override void Initialize()
        {
            var levelLoadController = ProjectContext.GetInstance<LevelLoadController>();
            foreach (var levelModel in levelLoadController.Levels)
            {
                var levelView = Instantiate(_LevelEntryViewPrefab, Vector3.zero, Quaternion.identity, _ScrollView.Content);
                levelView.Initialize(levelModel);
            }
            _VerticalLayoutGroup.Initialize();
            _ScrollView.Initialize();
        }

        public override void OnPreAppear()
        {
            base.OnPreAppear();
            _ScrollView.TeleportToPosition(0f);
        }
    }
}