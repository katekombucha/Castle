using DG.Tweening;
using UnityEngine;

namespace CodeBase.Effects
{
    public class BounceEffect : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public Transform bouncePoint;
        public float dampingDuration = 5;
        public float stretchDistance = 2;
        private float _stretch;
        private Material _material;
        private Tween _bounceTween;

        private void Start()
        {
            _material = meshRenderer.material;
        }

        public void Bounce()
        {
            _bounceTween.Kill();
            _material.SetVector("_pointOfBend", bouncePoint.position);
            _stretch = stretchDistance;
            _bounceTween = DOTween.To (() => _stretch, x => _stretch = x, 0, dampingDuration)
                .OnUpdate(() =>  _material.SetFloat("_stretch", _stretch));
        }
    }
}
