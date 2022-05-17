using System;
using CodeBase.GameEntities;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    public class CharacterMove : MonoBehaviour
    {
        public CharacterController CharacterController;
        public float MovementSpeed;
        private IInputService _inputService;
        
        private Action _onTargetReached;
        private SimpleMove _simpleMove;
        private MoveTowardTarget _moveTowardTarget;
        private RotateMove _rotateMove;
        private IMove _currentMoveType;
        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void Start()
        {
            _simpleMove = new SimpleMove(_inputService);
            _rotateMove = new RotateMove(_inputService);
            _currentMoveType = _simpleMove;
        }

        private void Update()
        {
            _currentMoveType.Execute(CharacterController, this, MovementSpeed);
        }

        public void MoveTo(Vector3 position, float speedMultiplier, Action onPositionReached = null)
        {
            _onTargetReached = onPositionReached;
            _moveTowardTarget = new MoveTowardTarget(position, speedMultiplier);
            _currentMoveType = _moveTowardTarget;
        }

        public void ResetControl() => 
            _currentMoveType = _simpleMove;

        public void TargetReached() => 
            _onTargetReached?.Invoke();

        public void BattleRotate() => 
            _currentMoveType = _rotateMove;
    }
}