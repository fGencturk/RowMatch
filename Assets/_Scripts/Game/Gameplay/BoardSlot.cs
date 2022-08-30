using System.Threading.Tasks;
using DG.Tweening;
using Game.Gameplay.Item;
using UnityEngine;
using Utilities;
using Utilities.Extensions;

namespace Game.Gameplay
{
    public class BoardSlot : MonoBehaviour
    {
        
        private const float SwipeDuration = .2f;
        private const float ScaleDuration = .2f;

        [SerializeField] private GameObject _CompletedIcon;
        
        private Vector2 _dragStartPosition;

        #region Properties
        
        public Vector2Int Index { get; private set; }
        public bool IsCompleted { get; set; }
        public bool IsAnimating { get; private set; }
        public BoardItem BoardItem { get; private set; }
        public bool CanSwipe => !IsCompleted && !IsAnimating;
        public ItemType ItemType => BoardItem.ItemType;

        #endregion

        public void Initialize(Vector2Int indexes, BoardItem boardItem)
        {
            Index = indexes;
            BoardItem = boardItem;
            
            var slotSize = Constants.Gameplay.BoardSlotSize;
            slotSize.Scale(indexes);
            
            var myTransform = transform;
            myTransform.position = slotSize;
            
            var boardItemTransform = BoardItem.transform;
            boardItemTransform.SetParent(myTransform, true);
            boardItemTransform.localPosition = Vector3.zero;
            
            _CompletedIcon.transform.localScale = Vector3.zero;
            _CompletedIcon.gameObject.SetActive(false);
        }

        public void SetOwnedBoardItem(BoardItem boardItem)
        {
            BoardItem = boardItem;
            BoardItem.transform.SetParent(transform, true);
        }

        public async Task AnimateItemToOrigin()
        {
            IsAnimating = true;
            var anim = BoardItem.transform.DOLocalMove(Vector3.zero, SwipeDuration).SetEase(Ease.InOutCubic);
            await anim.ToTask();
            IsAnimating = false;
        }

        public async Task AnimateComplete()
        {
            var seq = DOTween.Sequence()
                .Append(BoardItem.transform.DOScale(Vector3.zero, ScaleDuration).SetEase(Ease.InSine))
                .AppendCallback(() =>
                {
                    BoardItem.gameObject.SetActive(false);
                    _CompletedIcon.gameObject.SetActive(true);
                })
                .Append(_CompletedIcon.transform.DOScale(Vector3.one, ScaleDuration).SetEase(Ease.OutSine));

            IsAnimating = true;
            await seq.ToTask();
            IsAnimating = false;
        }
    }
}