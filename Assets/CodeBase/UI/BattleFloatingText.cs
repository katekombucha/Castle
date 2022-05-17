using UnityEngine;

namespace CodeBase.UI
{
    public class BattleFloatingText : MonoBehaviour
    {
        public ParticleSystem CitizensText;
        public ParticleSystem ArmyText;
        public Material[] NumberMaterials;
        private ParticleSystemRenderer _particleRenderer;

        private void Start() => 
            _particleRenderer = CitizensText.GetComponent<ParticleSystemRenderer>();

        public void Spawn()
        {
            CitizensText.Play();
            ArmyText.Play();
        }

        public void SetMaterial(int armyDamage) => 
            _particleRenderer.material = NumberMaterials[armyDamage - 1];
    }
}