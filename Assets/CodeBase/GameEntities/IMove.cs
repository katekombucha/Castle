using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.GameEntities
{
    public interface IMove
    {
        void Execute(CharacterController character, CharacterMove characterMove, float speed);
    }
}