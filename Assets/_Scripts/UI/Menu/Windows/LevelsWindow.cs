using System;
using Common.Context;
using Common.Event;
using Common.UI;
using Common.UI.LayoutGroup;
using Common.UI.Scroll;
using Common.UI.Window;
using Common.UI.Window.Event;
using Game.Gameplay.Event;
using LevelLoad;
using UnityEngine;

namespace UI.Menu.Windows
{
    public class LevelsWindow : Window
    {

        [SerializeField] private LevelEntryView _LevelEntryViewPrefab;
        [SerializeField] private RMVerticalLayoutGroup _VerticalLayoutGroup;
        [SerializeField] private RMVerticalScrollView _ScrollView;
        [SerializeField] private UIScaler _UIScaler;
        [SerializeField] private LockView _LockViewPrefab;

        private LockView _instantiatedLockView;

        public override void Initialize()
        {
            // Instantiate in as sibling, so that when window is closed, lock view continues to be visible
            _instantiatedLockView = Instantiate(_LockViewPrefab, transform.parent);
            _instantiatedLockView.gameObject.SetActive(false);
            
            var levelLoadController = ProjectContext.GetInstance<LevelLoadController>();
            foreach (var levelModel in levelLoadController.Levels)
            {
                var levelView = Instantiate(_LevelEntryViewPrefab, Vector3.zero, Quaternion.identity, _ScrollView.Content);
                levelView.Initialize(levelModel);
            }
            _VerticalLayoutGroup.Initialize();
            _ScrollView.Initialize();
            _UIScaler.Rebuild();
        }

        public override void OnPreAppear(object data)
        {
            base.OnPreAppear(data);

            if (data is GameEndEvent gameEndEvent)
            {
                var child = _VerticalLayoutGroup.transform.GetChild(gameEndEvent.LevelModel.LevelNumber - 1);
                _ScrollView.TeleportToPosition(-child.localPosition.y);
                if (gameEndEvent.PreviousHighScore <= 0 && gameEndEvent.HighScoreReached)
                {
                    var levelEntryTransform = _VerticalLayoutGroup.transform.GetChild(gameEndEvent.LevelModel.LevelNumber);
                    if (levelEntryTransform != null && levelEntryTransform.TryGetComponent<LevelEntryView>(out var levelEntryView))
                    {
                        _instantiatedLockView.AnimateUnlock(levelEntryView);
                    }
                }
            }
            else
            {
                _ScrollView.TeleportToPosition(0f);
            }
            
        }

        public void _Hide()
        {
            EventManager.Send(CloseWindowEvent.Create<LevelsWindow>());
        }
    }
}