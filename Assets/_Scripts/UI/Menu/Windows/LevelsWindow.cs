using System.Collections.Generic;
using Common.Context;
using Common.Event;
using Common.UI;
using Common.UI.LayoutGroup;
using Common.UI.Scroll;
using Common.UI.Window;
using Common.UI.Window.Event;
using DG.Tweening;
using Game.Gameplay.Event;
using LevelLoad;
using UnityEngine;

namespace UI.Menu.Windows
{
    public class LevelsWindow : Window
    {
        private const float InitialScaleMultiplier = .98f;
        private const float OverShootScaleMultiplier = 1.02f;
        private const float OneWayScaleDuration = .1f;

        [SerializeField] private LevelEntryView _LevelEntryViewPrefab;
        [SerializeField] private RMVerticalLayoutGroup _VerticalLayoutGroup;
        [SerializeField] private RMVerticalScrollView _ScrollView;
        [SerializeField] private UIScaler _UIScaler;
        [SerializeField] private LockView _LockViewPrefab;

        private LockView _instantiatedLockView;
        private Vector3 _initialScale;
        private Dictionary<int, LevelEntryView> _levelEntryViews;
        private int _lastInstantiatedLevelNumber = 0;

        public override void Initialize()
        {
            // Instantiate in as sibling, so that when window is closed, lock view continues to be visible
            _instantiatedLockView = Instantiate(_LockViewPrefab, transform.parent);
            _instantiatedLockView.gameObject.SetActive(false);
            _levelEntryViews = new Dictionary<int, LevelEntryView>();
            
            InstantiateLevelViews();
            _VerticalLayoutGroup.Initialize();
            _ScrollView.Initialize();
            _UIScaler.Rebuild();
            _initialScale = transform.localScale;
        }

        private void InstantiateLevelViews()
        {
            var levelLoadController = ProjectContext.GetInstance<LevelLoadController>();
            var levels = levelLoadController.Levels;

            var nextLevelIndex = _lastInstantiatedLevelNumber + 1;
            if (nextLevelIndex > levels.Count) return;
            
            for (var i = nextLevelIndex; i <= levels.Count; i++)
            {
                if (!levels.ContainsKey(i)) return;
                var levelModel = levels[i];
                
                var levelView = Instantiate(_LevelEntryViewPrefab, Vector3.zero, Quaternion.identity, _ScrollView.Content);
                levelView.Initialize(levelModel);
                _levelEntryViews.Add(levelModel.LevelNumber, levelView);
                _lastInstantiatedLevelNumber++;
            }
            _VerticalLayoutGroup.Initialize();
        }

        public override void OnPreAppear(object data)
        {
            // If new levels are downloaded in the background, try instantiate new views
            InstantiateLevelViews();
            
            base.OnPreAppear(data);

            if (data is GameEndEvent gameEndEvent)
            {
                var finishedLevelEntry = _levelEntryViews[gameEndEvent.LevelModel.LevelNumber];
                _ScrollView.TeleportToPosition(-finishedLevelEntry.transform.localPosition.y);
                if (gameEndEvent.PreviousHighScore <= 0 && gameEndEvent.HighScoreReached)
                {
                    var unlockedLevelNumber = gameEndEvent.LevelModel.LevelNumber + 1;
                    if (_levelEntryViews.ContainsKey(unlockedLevelNumber))
                    {
                        var unlockedLevelEntryView = _levelEntryViews[unlockedLevelNumber];
                        _instantiatedLockView.AnimateUnlock(unlockedLevelEntryView);
                    }
                }
            }
            else
            {
                _ScrollView.TeleportToPosition(0f);
            }

            AnimateAppear();
        }

        private void AnimateAppear()
        {
            transform.localScale = _initialScale * InitialScaleMultiplier;
            var appearSeq = DOTween.Sequence()
                .Append(transform.DOScale(_initialScale * OverShootScaleMultiplier, OneWayScaleDuration))
                .Append(transform.DOScale(_initialScale * InitialScaleMultiplier, OneWayScaleDuration))
                .Append(transform.DOScale(_initialScale, OneWayScaleDuration))
                .SetTarget(transform);
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void _Hide()
        {
            EventManager.Send(CloseWindowEvent.Create<LevelsWindow>());
        }
    }
}