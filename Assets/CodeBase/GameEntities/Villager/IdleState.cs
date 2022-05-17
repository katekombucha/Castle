using CodeBase.Hero;
using CodeBase.Logic.StateMachine;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.GameEntities.Villager
{
    public class IdleState : State
    {
        private readonly Unit _unit;
        private readonly Village _village;
        private bool _heroInsideTrigger;
        private Sequence _releaseSequence;
        private float _secondsToRelease = 0.8f;

        public IdleState(Unit unit, StateMachine stateMachine, Village village) : base(stateMachine)
        {
            _unit = unit;
            _village = village;
        }
        
        public override void OnTriggerEnter(Collider other)
        {
                if (other.GetComponent<CharacterLogic>())
                {
                    _heroInsideTrigger = true;
                    _unit.Timer.EnableTimer(_secondsToRelease);
                    _releaseSequence = DOTween.Sequence()
                        .AppendInterval(_secondsToRelease)
                        .AppendCallback(ReleaseVillager);
                }
        }

        public override void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<CharacterLogic>())
            {
                _heroInsideTrigger = false;
                _unit.Timer.StopTimer();
                _releaseSequence.Kill();
            }
        }
        
        private void ReleaseVillager()
        {
            if (_heroInsideTrigger)
            {
                _village.ReleaseVillager(_unit);
                _unit.StartFollowing();
            }
        }
    }
}