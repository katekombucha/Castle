using CodeBase.Hero;
using CodeBase.Services.Formation;
using UnityEngine;

namespace CodeBase.GameEntities.Castle
{
    public class CharacterBlock : MonoBehaviour
    {
        public BoxCollider BlockCollider;
        private IFormationService _formationService;
        
        public void Construct(IFormationService formationService, Vector3 colliderSize)
        {
            _formationService = formationService;
            BlockCollider.size = colliderSize;
        }

        private void Start() => 
            _formationService.OnArmyNumberChange += ArmyNumberChange;

        public void ActivateBlock() => 
            ChangeAccess(false);

        private void ArmyNumberChange(int armyNumber)
        {
            switch (armyNumber)
            {
                case 0:
                    ChangeAccess(false);
                    break;
                case 1:
                    ChangeAccess(true);
                    break;
            }
        }
    
        private void ChangeAccess(bool accessible) => 
            BlockCollider.enabled = !accessible;

        private void OnDisable() => 
            _formationService.OnArmyNumberChange -= ArmyNumberChange;
    }
}