using CodeBase.GameEntities.Castle;
using CodeBase.Hero;
using CodeBase.Logic.StateMachine;
using CodeBase.Services.Formation;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.GameEntities.Dragon
{
    public class DragonController : MonoBehaviour, IBattleParticipant, ISwimmer
    {
        public Animator Animator;
        public HpBar HpBar;
        public MMFeedbacks BattleFeedback;
        public MMFeedbacks LoseFeedbacks;
        public Transform DragonModelTransform;

        public DragonIdleState IdleState;
        public DragonFightState FightState;
        private StateMachine _stateMachine;
        private DragonRideState _rideState;

        private int _health = 10;
        private IFormationService _formationService;
        private CharacterLogic _character;
        private CharacterBlock _block;
        private DragonSpawnInfo _dragonSpawnInfo;

        public MMFeedbacks BattleFeedbacks => BattleFeedback;
        public Transform Transform => transform;

        public int FightUnits
        {
            get => _health;
            set => _health = value;
        }

        public void Construct(IFormationService formationService, CharacterLogic character, CharacterBlock block,
            DragonSpawnInfo dragonSpawnInfo)
        {
            _dragonSpawnInfo = dragonSpawnInfo;
            _character = character;
            _formationService = formationService;
            _block = block;
        }

        private void Start()
        {
            _stateMachine = new StateMachine();
            IdleState = new DragonIdleState(_stateMachine, this);
            FightState = new DragonFightState(_stateMachine, this, _formationService, _character);
            _rideState = new DragonRideState(_stateMachine, this, _character, _dragonSpawnInfo);
            _stateMachine.Initialize(IdleState);
        }

        private void OnTriggerEnter(Collider other) => 
            _stateMachine.CurrentState.OnTriggerEnter(other);

        private void OnTriggerExit(Collider other) => 
            _stateMachine.CurrentState.OnTriggerExit(other);

        private void Update() => 
            _stateMachine.CurrentState.LogicUpdate();

        public void ResetToIdle()
        {
            Animator.SetFloat("Move", 0);
            Animator.SetBool("Fly", false);
            Animator.SetTrigger("Idle");
            HpBar.gameObject.SetActive(true);
            _health = 10;
            UpdateFightUnitsUI(_health);
        }

        public void UpdateFightUnitsUI(int currentValue) => 
            HpBar.SetValue(currentValue, 10);

        public void LoseBattle()
        {
            LoseFeedbacks.PlayFeedbacks();
            HpBar.gameObject.SetActive(false);
            _stateMachine.ChangeState(_rideState);
        }

        public void WinBattle() => 
            Delay.Instance.Invoke(1, BlockEnter);

        private void BlockEnter() => 
            _block.ActivateBlock();

        public bool SwimCondition => true;
        public void StopSwim()
        {
            _rideState.StopFly();
        }
    }
}