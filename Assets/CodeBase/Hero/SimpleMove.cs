using CodeBase.GameEntities;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    class SimpleMove : IMove
    {
        private readonly IInputService _inputService;
        private readonly Camera _camera;

        public SimpleMove(IInputService inputService)
        {
            _inputService = inputService;
            _camera = Camera.main;
        }
        public void Execute(CharacterController character, CharacterMove characterMove, float speed)
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > 0.01f)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis); 
                movementVector.y = 0;
                movementVector.Normalize();

                character.transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            character.Move(speed * movementVector * Time.deltaTime);
        }
    }
}