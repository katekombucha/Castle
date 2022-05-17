using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.GameEntities.Villager;
using UnityEngine;

namespace CodeBase.Services.Formation
{
    public class FormationService : IFormationService
    {
        private float _offsetFromCenter;
        private float _distanceBetweenPairs;
        private Transform _target;
        private List<Unit> _units;
        private float _currentArmyLevel;
        private float _sumOfLevels;
        private float _speed;
        private int _normalSpeed = 7;
        private int _maxSpeed = 11;
        public bool FollowEachOther { get; set; }
        public int NumberOfFollowers => _units.Count;
        public float ArmyLevel => _currentArmyLevel;

        public Action<int> OnArmyNumberChange { get; set; }
        public Action <float> OnArmyLevelChange { get; set; }
        public Action OnUpgradableExist { get; set; }
        public Vector3 Target => _target.position;
        public float Speed => _speed;

        public FormationService()
        {
            _offsetFromCenter = 0.6f;
            _distanceBetweenPairs = 1.1f;
            _speed = _normalSpeed;
        }

        public void SetNewTarget(Transform target) => 
            _target = target;

        public int AddFollower(Unit follower)
        {
            _units.Add(follower);
            OnArmyNumberChange?.Invoke(_units.Count);
            UpdateArmyLevel();
            return _units.Count - 1;
        }

        public void KillFollower(bool withParticles)
        {
            if(_units.Count == 0)
                return;
            
            _units[0].Die(withParticles);
            _units.RemoveAt(0);
            OnArmyNumberChange?.Invoke(_units.Count);
            UpdateFollowersPositions();
            UpdateArmyLevel();
        }

        public void SpeedUpArmy()
        {
            _speed = _maxSpeed;
            _units.ForEach(x => x.Speed = _speed);
        }

        public void SlowDownArmy()
        {
            _speed = _normalSpeed;
            _units.ForEach(x => x.Speed = _speed);
        }

        public void Clear()
        {
            _currentArmyLevel = 0;
            _units = new List<Unit>();
        }

        public Vector3 GetPositionOffset(int id) => 
            id > 1 ? FollowUnit(id) : FollowHero(id);

        public void UpdateArmyLevel()
        {
            _sumOfLevels = _units.Sum(x => x.UnitLevel);

            if (_sumOfLevels == 0)
            {
                OnArmyLevelChange?.Invoke(0);
                return;
            }

            _currentArmyLevel = _sumOfLevels / _units.Count;
            OnArmyLevelChange?.Invoke(_currentArmyLevel);
        }

        public void DragonFight(bool active)
        {
            foreach (Unit unit in _units)
            {
                unit.DragonFight(active);
            }
        }

        private Vector3 FollowUnit(int id) => 
            _units[id-2].transform.TransformPoint(new Vector3(0, 0, -_distanceBetweenPairs));

        private Vector3 FollowHero(int id)
        {
            float xOffset = id % 2 == 0 ? 1 : -1;
            int rowNum = id / 2 + 1;
            float zOffset = _distanceBetweenPairs * rowNum;
            return _target.TransformPoint(new Vector3(_offsetFromCenter * xOffset, 0, -zOffset));
        }

        private void UpdateFollowersPositions()
        {
            foreach (Unit villager in _units) 
                villager.Id--;
        }

        public void UpgradableExists(int gatesLevel)
        {
            if (_units.Any(villager => villager.UnitEquipment.CanUpgrade(gatesLevel)))
                OnUpgradableExist?.Invoke();
        }

        public void LoadProgress(PlayerProgress progress, string sceneName) { }

        public void UpdateProgress(PlayerProgress progress, string levelName)
        {
            int[] unitLevels = new int[_units.Count];
            
            for (int i = 0; i < unitLevels.Length - 1; i++)
            {
                unitLevels[i] = _units[i].UnitLevel;
            }
            
            progress.DataOnLevel[levelName].UnitsLevels = unitLevels;
        }
    }
}