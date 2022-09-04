using Common.Context;
using Common.Event;
using Common.Save;
using Common.Scene.Event;
using Common.Utility;
using Game.Model;
using TMPro;
using UnityEngine;

namespace UI.Menu
{
    public class LevelEntryView : MonoBehaviour
    {

        [SerializeField] private TextMeshPro _LevelText;
        [SerializeField] private TextMeshPro _HighScoreText;

        [Header("Button Containers")] 
        [SerializeField] private Transform _PlayContainer;
        [SerializeField] private Transform _LockedContainer;
        
        [Header("Lock")]
        [SerializeField] private SpriteRenderer _LockRenderer;
        [SerializeField] private SpriteRenderer _LockBackgroundRenderer;

        #region Properties

        public SpriteRenderer LockRenderer => _LockRenderer;
        public SpriteRenderer LockBackgroundRenderer => _LockBackgroundRenderer;

        #endregion
        
        private LevelModel _levelModel;

        public void Initialize(LevelModel levelModel)
        {
            _levelModel = levelModel;
            _LevelText.text = $"Level {levelModel.LevelNumber} - {levelModel.MoveCount} Moves";

            var playerData = ProjectContext.GetInstance<PlayerData>();

            var levelPlayable = LevelUtilities.IsLevelPlayable(levelModel.LevelNumber);
            if (levelPlayable)
            {
                _HighScoreText.text = playerData.TryGetHighScore(levelModel.LevelNumber, out var score) 
                    ? $"Highest Score : {score}" 
                    : "No Score";
            }
            else
            {
                _HighScoreText.text = "Locked Level";
            }
            
            _PlayContainer.gameObject.SetActive(levelPlayable);
            _LockedContainer.gameObject.SetActive(!levelPlayable);
        }

        #region Handler

        public void _OnPlayButtonClicked()
        {
            EventManager.Send(OpenGameSceneEvent.Create(_levelModel));
        }

        #endregion
    }
}