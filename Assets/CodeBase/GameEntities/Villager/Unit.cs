using System.Timers;
using CodeBase.Equipment;
using CodeBase.Logic.StateMachine;
using CodeBase.Services.Formation;
using CodeBase.StaticData;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;
using Timer = CodeBase.UI.Elements.Timer;

namespace CodeBase.GameEntities.Villager
{
    public class Unit : MonoBehaviour
    {
        public NavMeshAgent NavMeshAgent;
        public Animator Animator;
        public SphereCollider HeroCollideDetector;
        public Timer Timer;
        public MMFeedbacks DeathFeedbacks;
        public UnitEquipment UnitEquipment;
        
        private StateMachine _stateMachine;
        private IdleState _idleState;
        private FightState _fightState;
        private FollowingState _followingState;
        private Village _village;
        private IFormationService _formationService;

        public int UnitLevel => UnitEquipment.CurrentLevel;

        public float Speed
        {
            get => NavMeshAgent.speed;
            set => NavMeshAgent.speed = value;
        }

        public int Id { get; set; } = -1;

        public void CreateInArmy(IFormationService formationService, IStaticDataService staticDataService, int unitLevel)
        {
            _formationService = formationService;
            UnitEquipment.Initialize(staticDataService);
            UnitEquipment.Upgrade(unitLevel, false);
        }
        public void Construct(IFormationService formationService, IStaticDataService staticDataService, Village village)
        {
            _formationService = formationService;
            _village = village;
            UnitEquipment.Initialize(staticDataService);
        }

        private void Start()
        {
            _stateMachine = new StateMachine();
            _idleState = new IdleState(this, _stateMachine, _village);
            _followingState = new FollowingState(this, _stateMachine, _formationService);
            _fightState = new FightState(this, _stateMachine, _formationService);
            
            if(_village == null)
                _stateMachine.Initialize(_followingState);
            else
                _stateMachine.Initialize(_idleState);
        }

        public void StartFollowing() => 
            _stateMachine.ChangeState(_followingState);

        public void DragonFight(bool active)
        {
            State state = active ? (State) _fightState : _followingState;
            _stateMachine.ChangeState(state);
        }
        
        public void Die(bool withParticles)
        {
            int delay = 0;
            if (withParticles)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                DeathFeedbacks.PlayFeedbacks();
                delay = 1;
            }

            Destroy(gameObject, delay);
        }

        private void Update()
        {
            _stateMachine.CurrentState.HandleInput();
            _stateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() => 
            _stateMachine.CurrentState.PhysicsUpdate();

        private void OnTriggerEnter(Collider other) => 
            _stateMachine.CurrentState.OnTriggerEnter(other);

        private void OnTriggerExit(Collider other) => 
            _stateMachine.CurrentState.OnTriggerExit(other);
    }
}