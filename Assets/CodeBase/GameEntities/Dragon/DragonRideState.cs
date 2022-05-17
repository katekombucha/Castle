using CodeBase.Hero;
using CodeBase.Logic.StateMachine;
using CodeBase.StaticData;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.GameEntities.Dragon
{
    public class DragonRideState : State
    {
        private readonly CharacterLogic _characterLogic;
        private readonly CharacterController _characterController;
        private readonly DragonController _dragonController;
        private readonly DragonSpawnInfo _dragonSpawnInfo;
        private readonly StateMachine _stateMachine;
        private bool _inBattle;
        private static readonly int FlyAnimation = Animator.StringToHash("Fly");
        private static readonly int Move = Animator.StringToHash("Move");

        public DragonRideState(StateMachine stateMachine, DragonController dragonController, CharacterLogic characterLogic,
            DragonSpawnInfo dragonSpawnInfo) : base(stateMachine)
        {
            _stateMachine = stateMachine;
            _dragonSpawnInfo = dragonSpawnInfo;
            _dragonController = dragonController;
            _characterLogic = characterLogic;
            _characterController = _characterLogic.GetComponent<CharacterController>();
        }

        public override void Enter()
        {
            _dragonController.transform.SetParent(_characterLogic.HeroModelTransform.transform);
            _dragonController.transform.DOLocalMove(Vector3.zero - Vector3.up, 0.5f);
            _dragonController.transform.DOLocalRotate(Vector3.zero, 0.5f);
            _characterLogic.OnCastleBattle += CastleBattle;
            _characterLogic.OnLose += ReturnToStartPlace;
            _inBattle = false;
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Water>())
            {
                Fly();
            }
        }

        private void CastleBattle()
        {
            _inBattle = true;
            Fly();
            _dragonController.BattleFeedback.PlayFeedbacks();
        }

        private void Fly()
        {
            _dragonController.DragonModelTransform.DOLocalMoveY(-2, 0.2f);
            _dragonController.Animator.SetBool(FlyAnimation, true);
        }

        public void StopFly()
        {
            if(!_inBattle)
            {
                _dragonController.DragonModelTransform.DOLocalMoveY(0, 0.2f);
                _dragonController.Animator.SetBool(FlyAnimation, false);
            }
        }
        private void ReturnToStartPlace()
        {
            _inBattle = false;
            _dragonController.BattleFeedback.StopFeedbacks();
            _dragonController.transform.SetParent(null);

            DOTween.Sequence()
                .Append(_dragonController.transform.DORotate(Vector3.down * _dragonSpawnInfo.DragonRotation, 0.2f))
                .Append(_dragonController.transform.DOMove(_dragonSpawnInfo.DragonPosition, 5f).SetEase(Ease.Linear))
                .Append(_dragonController.transform.DORotate(Vector3.up * _dragonSpawnInfo.DragonRotation, 0.2f))
                .AppendCallback(() => _stateMachine.ChangeState(_dragonController.IdleState))
                .AppendCallback(_dragonController.ResetToIdle);
        
            Unsubscribe();
        }

        public override void LogicUpdate() => 
            _dragonController.Animator.SetFloat(Move, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);

        private void Unsubscribe()
        {
            _characterLogic.OnCastleBattle -= CastleBattle;
            _characterLogic.OnLose -= ReturnToStartPlace;
        }

        public override void Exit()
        {
            StopFly();
            Unsubscribe();
        }
    }
}