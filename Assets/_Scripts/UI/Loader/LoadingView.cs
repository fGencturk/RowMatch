using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utilities.Extensions;

namespace UI.Loader
{
    public class LoadingView : MonoBehaviour
    {
        private const float FadeDuration = .2f;
        
        [SerializeField] private SpriteRenderer _Background;
        [SerializeField] private TextMeshPro _Title;

        public async Task Show()
        {
            transform.DOKill();
            var seq = DOTween.Sequence()
                .Append(_Background.DOFade(1f, FadeDuration))
                .Join(_Title.DOFade(1f, FadeDuration))
                .SetTarget(transform);
            await seq.ToTask();
        }

        public async Task Hide()
        {
            transform.DOKill();
            var seq = DOTween.Sequence()
                .Append(_Background.DOFade(0f, FadeDuration))
                .Join(_Title.DOFade(0f, FadeDuration))
                .SetTarget(transform);
            await seq.ToTask();
        }
        
    }
}