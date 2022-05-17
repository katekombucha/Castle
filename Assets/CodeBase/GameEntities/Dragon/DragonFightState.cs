using CodeBase.Hero;
using CodeBase.Logic.StateMachine;
using CodeBase.Services.Formation;
using UnityEngine;

namespace CodeBase.GameEntities.Dragon
{
    public class DragonFightState : State
    {
        private readonly DragonController _dragonController;
        private readonly StateMachine _stateMachine;
        private readonly IFormationService _formationService;
        private readonly CharacterLogic _characterLogic;
        private DragonBattle _dragonBattle;
        private static readonly int Fight = Animator.StringToHash("Fight");

        public DragonFightState(StateMachine stateMachine, DragonController dragonController,
            IFormationService formationService, CharacterLogic characterLogic) : base(stateMachine)
        {
            _characterLogic = characterLogic;
            _formationService = formationService;
            _stateMachine = stateMachine;
            _dragonController = dragonController;
        }

        public override void Enter()
        {
            _dragonBattle = new DragonBattle(_characterLogic, _dragonController, _formationService);
            _dragonBattle.StartBattle();
            _dragonController.Animator.SetBool(Fight, true);
        }
    
        public override void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<CharacterLogic>())
            {
                _stateMachine.ChangeState(_dragonController.IdleState);
            }
        }

        public override void Exit() => 
            _dragonController.Animator.SetBool(Fight, false);
    }
}