using Common.Event;
using Game.Gameplay.Event;
using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class MoveCountView : MonoBehaviour
    {

        [SerializeField] private TextMeshPro _MoveCount;
        
        private void Awake()
        {
            EventManager.Register<LevelStartedEvent>(OnLevelStarted);
            EventManager.Register<PreSwapPerformedEvent>(OnPreSwapPerformed);
        }

        private void OnDestroy()
        {
            EventManager.Unregister<LevelStartedEvent>(OnLevelStarted);
            EventManager.Unregister<PreSwapPerformedEvent>(OnPreSwapPerformed);
        }

        private void OnLevelStarted(LevelStartedEvent data)
        {
            _MoveCount.text = $"{data.LevelModel.MoveCount}";
        }

        private void OnPreSwapPerformed(PreSwapPerformedEvent data)
        {
            _MoveCount.text = $"{data.RemainingMoveCount}";
        }
    }
}