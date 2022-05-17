using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.GameEntities.Castle
{
    public class FireInCastle : MonoBehaviour
    {
        private ParticleSystem[] _fireParticles;
        private int _fireStep;
        private int _nextCountToFire;
        private Stack<int> _repairSteps;
        private IBattleParticipant _battleParticipant;

        private void Awake()
        {
            _fireParticles = GetComponentsInChildren<ParticleSystem>();
        }

        public void Construct(int maxCitizens, IBattleParticipant battleParticipant)
        {
            _battleParticipant = battleParticipant;
            CreateStack(maxCitizens);
        }

        public void CheckIfFire()
        {
            if (_battleParticipant.FightUnits <= _nextCountToFire)
            {
                AddFire();
            }
        }
        
        private void CreateStack(int maxCitizens)
        {
            _fireStep = maxCitizens / 3;
            _nextCountToFire = maxCitizens - _fireStep;
            _repairSteps = new Stack<int>();
        }

        private void AddFire()
        {
            if(_repairSteps.Count == _fireParticles.Length)
                return;

            _repairSteps.Push(_nextCountToFire);
            _nextCountToFire -= _fireStep;
            _fireParticles[_repairSteps.Count-1].Play();
        }
        
    }
}