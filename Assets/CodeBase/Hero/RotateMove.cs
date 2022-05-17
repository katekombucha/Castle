using CodeBase.GameEntities;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    class RotateMove : IMove
    {
        private readonly IInputService _inputService;

        public RotateMove(IInputService inputService)
        {
            _inputService = inputService;
        }
        public void Execute(CharacterController character, CharacterMove characterMove, float speed)
        {
            character.transform.Rotate(0, _inputService.AxisY, 0);
        }
    }
}