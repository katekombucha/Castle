using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Hero
{
    public class BattleUI : MonoBehaviour
    {
        public Image SwordsImage;

        public void Activate()
        {
            SwordsImage.DOFade(1, 0.5f);
            SwordsImage.transform.DOScale(1, 0.5f).SetEase(Ease.InOutBack).OnComplete(
                () => SwordsImage.transform.DOShakePosition(5, 0.5f).Loops());
        }

        public void Deactivate()
        {
            SwordsImage.DOFade(0, 1f);
            SwordsImage.transform.DOLocalMoveY(1, 1f);
        }
    }
}