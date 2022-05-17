using CodeBase.Infrastructure.Services;
using CodeBase.Logic.StateMachine;
using CodeBase.Services.Formation;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.GameEntities.Villager
{
    public class FollowingState : State
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly IFormationService _formationService;
        private readonly Unit _unit;

        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Idle = Animator.StringToHash("Idle");

        public FollowingState(Unit unit, StateMachine stateMachine, IFormationService formationService) : base(stateMachine)
        {
            _unit = unit;
            _formationService = formationService;
            _navMeshAgent = unit.NavMeshAgent;
        }

        public override void Enter()
        {
            if (_unit.Id == -1)
            {
                _unit.Id = _formationService.AddFollower(_unit);
                _unit.HeroCollideDetector.enabled = false;
                _unit.Speed = _formationService.Speed;
            }
        }

        public override void PhysicsUpdate()
        {
            Vector3 movePos = _formationService.GetPositionOffset(_unit.Id);
            _navMeshAgent.SetDestination(movePos);

            if (_navMeshAgent.velocity == Vector3.zero)
                _unit.Animator.SetTrigger(Idle);
            else
                _unit.Animator.SetTrigger(Walk);
        }
    }
}