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

        [SerializeField] private LevelEntryView _LevelEntryViewPrefab;
        [SerializeField] private RMVerticalLayoutGroup _VerticalLayoutGroup;
        [SerializeField] private RMVerticalScrollView _ScrollView;
        [SerializeField] private UIScaler _UIScaler;
        [SerializeField] private LockView _LockViewPrefab;

        private LockView _instantiatedLockView;
        private Vector3 _initialScale;

        private const float InitialScaleMultiplier = .98f;
        private const float OverShootScaleMultiplier = 1.02f;
        private const float OneWayScaleDuration = .1f;

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
            _initialScale = transform.localScale;
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