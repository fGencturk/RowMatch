using Common.Event;
using Common.Scene.Event;
using Game.Model;
using TMPro;
using UnityEngine;

namespace UI.Menu
{
    public class LevelEntryView : MonoBehaviour
    {

        [SerializeField] private TextMeshPro _LevelText;
        [SerializeField] private TextMeshPro _HighScoreText;
        private LevelModel _levelModel;

        public void Initialize(LevelModel levelModel)
        {
            _levelModel = levelModel;
            _LevelText.text = $"Level {levelModel.LevelNumber} - {levelModel.MoveCount} Moves";
            // TODO get highscore from savedata
            _HighScoreText.text = $"No HighScore";
        }

        #region Handler

        public void _OnPlayButtonClicked()
        {
            EventManager.Send(OpenGameSceneEvent.Create(_levelModel));
        }

        #endregion
    }
}