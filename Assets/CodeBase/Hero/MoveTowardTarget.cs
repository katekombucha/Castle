using CodeBase.GameEntities;
using UnityEngine;

namespace CodeBase.Hero
{
    class MoveTowardTarget : IMove
    {
        private readonly Vector3 _target;
        private readonly float _speedMultiplier;

        public MoveTowardTarget(Vector3 target, float speedMultiplier)
        {
            _target = target;
            _speedMultiplier = speedMultiplier;
        }

        public void Execute(CharacterController character, CharacterMove characterMove, float speed)
        {
            var movementVector = Vector3.zero;

            if (Vector3.Distance(character.transform.position, _target) > .1f)
            {
                movementVector = _target - character.transform.position;
                movementVector.y = 0;

                if(movementVector.sqrMagnitude > .1f) 
                {
                    movementVector = movementVector.normalized * (speed * _speedMultiplier);
                    character.transform.forward = movementVector;
                }
                else
                {
                    characterMove.TargetReached();
                }
            }
            
            character.Move(movementVector * Time.deltaTime);
        }
    }
}