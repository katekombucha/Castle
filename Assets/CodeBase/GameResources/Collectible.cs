using CodeBase.GameEntities;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.GameResources
{
    public enum ResourceType
    {
        Coin = 0,
        None = 1
    }
    
    public class Collectible : MonoBehaviour
    {
        public ParticleSystem Trail;
        public ParticleSystem Shine;
        public BoxCollider Collider;
        public ResourceType ResourceType;
        private bool _collected;

        public void CreateInStack(ResourceStack resourceStack)
        {
            Trail.Stop();
            Collect(resourceStack,false);
        }

        public void Jump(Vector3 origin, float coinsOffset)
        {
            Collider.enabled = false;
            
            float angle = Random.Range(60, 300);
            float radius = coinsOffset;
            
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float z = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            Vector3 jumpPosition = new Vector3(origin.x + x, 0, origin.z + z);

            transform.DOJump(jumpPosition, 5, 1, 1.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() => EnableComponents());
        }

        private void EnableComponents()
        {
            Collider.enabled = true;
            Collider.size = new Vector3(2, 1, 2);
            Trail.Stop();
            Shine.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<ResourceStack>()) 
                Collect(other.GetComponent<ResourceStack>(), true);

            if (other.GetComponent<Water>() && !_collected)
                Drawn();
        }

        private void Drawn() => 
            transform.DOMoveY(-1f, 0.2f);

        public void Collect(ResourceStack stack, bool feedback)
        {
            if (_collected)
                return;

            Shine.Stop();
            _collected = true;
            stack.AddResource(transform, ResourceType, feedback);
        }
    }
}
