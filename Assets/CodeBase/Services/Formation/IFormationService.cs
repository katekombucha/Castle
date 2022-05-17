using System;
using CodeBase.GameEntities.Villager;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.Formation
{
    public interface IFormationService : IService, ISavedProgress
    {
        int AddFollower(Unit follower);
        int NumberOfFollowers { get; }
        float ArmyLevel{ get; }
        Action<int> OnArmyNumberChange { get; set; }
        Action<float> OnArmyLevelChange { get; set; }
        bool FollowEachOther { get; set; }
        Vector3 GetPositionOffset(int id);
        Action OnUpgradableExist { get; set; }
        Vector3 Target { get; }
        float Speed { get; }
        void SetNewTarget(Transform target);
        void UpdateArmyLevel();
        void DragonFight(bool active);
        void KillFollower(bool withParticles);
        void SpeedUpArmy();
        void SlowDownArmy();
        void Clear();
    }

}