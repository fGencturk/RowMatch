using Common.Event;
using Common.UI.Window;
using Common.UI.Window.Event;
using DG.Tweening;
using Game.Gameplay.Event;
using TMPro;
using UnityEngine;

namespace UI.Menu.Windows
{
    public class CelebrationWindow : Window
    {

        private const float StarInitialRotation = 90;
        private const float StarEndRotation = 0f;

        private const float StarScaleDownDuration = .3f;
        private const float StarScaleUpDuration = .15f;

        private const float StarInitialScale = 3f;
        private const float StarIntermediaryScale = .6f;
        private const float StartFinalScale = 1f;

        private const float TextInitialScale = .6f;
        private const float TextEndScale = 1f;

        private const float CloseAfterSeconds = 2.5f;
        
        private const string HighScorePrefix = "Highest Score\n";

        [SerializeField] private SpriteRenderer _Star;
        [SerializeField] private ParticleSystem _Particle;
        [SerializeField] private TextMeshPro _Score;

        private object _data;


        public override void OnPreAppear(object data)
        {
            _data = data;
            
            _Particle.Stop();
            _Particle.gameObject.SetActive(false);
            
            _Score.transform.localScale = Vector3.one * TextInitialScale;
            var color = _Score.color;
            color.a = 0f;
            _Score.color = color;
            
            var starTransform = _Star.transform;
            starTransform.rotation = Quaternion.Euler(0, 0, StarInitialRotation);
            starTransform.localScale = Vector3.one * StarInitialScale;
            var seq = DOTween.Sequence()
                .Append(starTransform.DOScale(StarIntermediaryScale, StarScaleDownDuration))
                .Join(starTransform.DORotate(new Vector3(0, 0, StarEndRotation), StarScaleDownDuration))
                .AppendCallback(() =>
                {
                    _Particle.gameObject.SetActive(true);
                    _Particle.Play();
                })
                .Append(starTransform.DOScale(StartFinalScale, StarScaleUpDuration))
                .Join(_Score.transform.DOScale(TextEndScale, StarScaleUpDuration))
                .Join(_Score.DOFade(1f, StarScaleUpDuration))
                .AppendInterval(CloseAfterSeconds)
                .OnComplete(Hide);

            if (data is GameEndEvent gameEndEvent)
            {
                _Score.text = $"{HighScorePrefix}{gameEndEvent.Score}";
            }
        }

        private void Hide()
        {
            EventManager.Send(CloseWindowEvent.Create<CelebrationWindow>());
            EventManager.Send(OpenWindowEvent.Create<LevelsWindow>(_data));
        }
    }
}