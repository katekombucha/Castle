using CodeBase.Hero;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.GameEntities
{
    public class AnimalController : MonoBehaviour
    {
        public MMFeedbacks CollideFeedback;
        public Collider Collider;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<AnimalCollider>())
            {
                CollideFeedback.PlayFeedbacks();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Water>())
            {
                Collider.enabled = false;
                Destroy(gameObject, 2);
            }
        }
    }
}