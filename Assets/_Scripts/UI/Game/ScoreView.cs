using Common.Event;
using Game.Gameplay.Event;
using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class ScoreView : MonoBehaviour
    {

        [SerializeField] private TextMeshPro _CurrentScore;
        
        private void Awake()
        {
            EventManager.Register<ScoreGainedEvent>(OnScoreGained);
            _CurrentScore.text = "0";
        }

        private void OnDestroy()
        {
            EventManager.Unregister<ScoreGainedEvent>(OnScoreGained);
        }

        private void OnScoreGained(ScoreGainedEvent data)
        {
            _CurrentScore.text = $"{data.CurrentScore}";
        }
    }
}