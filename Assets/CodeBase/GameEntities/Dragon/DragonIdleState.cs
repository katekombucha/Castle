using CodeBase.Hero;
using CodeBase.Logic.StateMachine;
using UnityEngine;

namespace CodeBase.GameEntities.Dragon
{
    public class DragonIdleState : State
    {
        private readonly StateMachine _stateMachine;
        private readonly DragonController _dragonController;

        public DragonIdleState(StateMachine stateMachine, DragonController dragonController) : base(stateMachine)
        {
            _dragonController = dragonController;
            _stateMachine = stateMachine;
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterLogic>())
            {
                _stateMachine.ChangeState(_dragonController.FightState);
            }
        }
    }
}