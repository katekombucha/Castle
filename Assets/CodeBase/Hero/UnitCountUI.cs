using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.Hero
{
    public class UnitCountUI : MonoBehaviour
    {
        public TextMeshProUGUI CountText;
        public CanvasGroup Panel;

        private float _transformDuration = 0.2f;
        
        public void UpdateCount(int newCount)
        {
            switch (newCount.CompareTo(0))
            {
                case -1:
                    newCount = 0;
                    Panel.DOFade(0, 2).SetEase(Ease.InQuint);
                    break;
                case 0:
                    Panel.DOFade(0, 2).SetEase(Ease.InQuint);
                    break;
                case 1:
                    Panel.alpha = 1;
                    break;
            }

            CountText.text = newCount.ToString();
        }

        public void BattleState(Transform relatedTransform, float scale = 1)
        {
            transform.SetParent(relatedTransform.parent);
            transform.DOLocalMove(relatedTransform.localPosition - new Vector3(1.75f, 0, 0), _transformDuration);
            transform.DOScale(scale, _transformDuration);
        }
        
        public void MoveLocalX(float xPosition)
        {
            transform.DOLocalMoveX(xPosition, 1);
        }

        public void NormalState(Transform parent, Vector3 position, float scale = 1)
        {
            transform.SetParent(parent);
            transform.DOLocalMove(position, 1);
            transform.DOScale(scale, 1);
        }

        private void OnDisable() => 
            DOTween.KillAll();
    }
}