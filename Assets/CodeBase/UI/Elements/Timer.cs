using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class Timer : MonoBehaviour
    {
        public Image TimerImage;
        private bool _counting;
        private Coroutine _coroutine;
        public void EnableTimer(float secondsToRespawn)
        {
            if(_counting)
                return;

            _counting = true;
            TimerImage.gameObject.SetActive(true);
            TimerImage.fillAmount = 0;
            TimerImage.DOFillAmount(1, secondsToRespawn)
                .SetEase(Ease.Linear)
                .OnComplete(() => TimerImage.gameObject.SetActive(false));
            _coroutine = StartCoroutine(CountDown(secondsToRespawn));
        }

        public void StopTimer()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            
            DOTween.Kill(TimerImage);
            _counting = false;
            TimerImage.gameObject.SetActive(false);
        }
        private IEnumerator CountDown(float secondsToRespawn)
        {
            while (secondsToRespawn > 0)
            {
                secondsToRespawn = ReduceTime(secondsToRespawn);
                yield return new WaitForSeconds(1);
            }

            _counting = false;
        }

        protected virtual float ReduceTime(float secondsToRespawn)
        {
            secondsToRespawn -= 1;
            return secondsToRespawn;
        }
    }
}