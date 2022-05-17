using CodeBase.Infrastructure;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.GameEntities
{
    public interface IBattleParticipant : ICoroutineRunner
    {
        public MMFeedbacks BattleFeedbacks { get; }
        public Transform Transform { get; }
        public int FightUnits { get; set; }
        void UpdateFightUnitsUI(int currentValue);
        void LoseBattle();
        void WinBattle();
    }
}