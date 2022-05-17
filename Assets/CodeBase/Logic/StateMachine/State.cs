using UnityEngine;

namespace CodeBase.Logic.StateMachine
{
    public class State
    {
        private StateMachine stateMachine;

        protected State(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter()
        {
        }

        public virtual void HandleInput()
        {
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            
        }

        public virtual void OnTriggerExit(Collider collider)
        {

        }
    }
}