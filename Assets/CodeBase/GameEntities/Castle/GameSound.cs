using DG.Tweening;
using UnityEngine;

namespace CodeBase.GameEntities.Castle
{
    public class GameSound : MonoBehaviour
    {
        public AudioSource AudioSource;
        public AudioClip BattleClip;
        public AudioClip Ambience;
    
        public void PlayBattle()
        {
            PlayClip(BattleClip, 0.5f);
        }

        private void PlayClip(AudioClip clip, float duration)
        {
            AudioSource.clip = clip;
            AudioSource.volume = 0;
            AudioSource.Play();
            AudioSource.DOFade(1, duration);
        }

        public void StopBattle()
        {
            Sequence audioSequence = DOTween.Sequence();
            audioSequence
                .Append(AudioSource.DOFade(0f, 1f))
                .AppendCallback(() => PlayClip(Ambience, 3));
        }
    }
}