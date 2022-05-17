using CodeBase.GameResources;
using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.GameEntities
{
    public class Water : MonoBehaviour
    {
        public GameObject SplashParticles;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Collectible>() | other.GetComponent<AnimalController>())
            {
                GameObject splash = Instantiate(SplashParticles, other.transform.position, Quaternion.identity);
                Destroy(splash, 1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out SwimmerDetector swimmer))
            {
                swimmer.LeaveWaterTrigger();
            }
        }
    }
}