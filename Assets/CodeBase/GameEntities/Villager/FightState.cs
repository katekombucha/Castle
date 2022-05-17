using CodeBase.Logic.StateMachine;
using CodeBase.Services.Formation;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.GameEntities.Villager
{
    public class FightState : State
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly IFormationService _formationService;
        private readonly Unit _unit;
        private static readonly int Attack = Animator.StringToHash("Attack");

        public FightState(Unit unit, StateMachine stateMachine, IFormationService formationService) : base(stateMachine)
        {
            _unit = unit;
            _formationService = formationService;
            _navMeshAgent = unit.NavMeshAgent;
        }

        public override void Enter() => 
            _unit.Animator.SetBool(Attack, true);

        public override void PhysicsUpdate() => 
            _navMeshAgent.SetDestination(_formationService.Target);

        public override void Exit() => 
            _unit.Animator.SetBool(Attack, false);
    }
}