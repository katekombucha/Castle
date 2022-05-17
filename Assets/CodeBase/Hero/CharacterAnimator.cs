using System;
using CodeBase.Logic;
using UnityEngine;
using AnimatorState = CodeBase.Logic.AnimatorState;

namespace CodeBase.Hero
{
    public class CharacterAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int MoveHash = Animator.StringToHash("Walking");
        private static readonly int CheeringHash = Animator.StringToHash("Cheering");
        private static readonly int KickedHash = Animator.StringToHash("Kicked");
        private static readonly int SwimHash = Animator.StringToHash("Swim");
        private static readonly int FightHash = Animator.StringToHash("Fight");
        private static readonly int RideHash = Animator.StringToHash("Ride");

        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _cheeringStateHash = Animator.StringToHash("Cheering");
        private readonly int _walkingStateHash = Animator.StringToHash("Walk");
        private readonly int _kickedStateHash = Animator.StringToHash("Kicked");
        private readonly int _swimmingStateHash = Animator.StringToHash("Swimming");

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }

        public Animator Animator;
        public CharacterController CharacterController;
        
        private void Update()
        {
            Animator.SetFloat(MoveHash, CharacterController.velocity.magnitude, 0.1f, Time.deltaTime);
        }
        
        public void PlayKicked() => 
            Animator.SetTrigger(KickedHash);
        
        public void PlaySwim()=> 
            Animator.SetBool(SwimHash, true);
        public void StopSwim()=> 
            Animator.SetBool(SwimHash, false);

        public void PlayFight() =>
            Animator.SetBool(FightHash, true);
        
        public void StopFight() =>
            Animator.SetBool(FightHash, false);

        public void PlayRide() =>
            Animator.SetBool(RideHash, true);
        
        public void StopRide() =>
            Animator.SetBool(RideHash, false);
        
        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash) =>
            StateExited?.Invoke(StateFor(stateHash));

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == _cheeringStateHash)
                state = AnimatorState.Cheering;
            else if (stateHash == _kickedStateHash)
                state = AnimatorState.Kicked;
            else if (stateHash == _swimmingStateHash)
                state = AnimatorState.Swim;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}