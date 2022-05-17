using System.Collections;
using CodeBase.GameEntities;
using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.Services.Formation
{
    public abstract class Battle
    {
        protected readonly CharacterLogic _characterLogic;
        protected readonly IFormationService _formationService;
        protected readonly IBattleParticipant _battleParticipant;

        protected Battle(CharacterLogic characterLogic, IBattleParticipant battleParticipant, IFormationService formationService)
        {
            _characterLogic = characterLogic;
            _formationService = formationService;
            _battleParticipant = battleParticipant;
        }

        public abstract void StartBattle();
        protected abstract void HeroWin();
        protected abstract void HeroLose();
        protected abstract IEnumerator DecreaseArmyCount();

        protected void UpdateUI(int incomeArmyCount, int currentFightUnit)
        {
            _battleParticipant.UpdateFightUnitsUI(currentFightUnit);
            _characterLogic.CountText.UpdateCount(incomeArmyCount);
        }

        protected void GetArmyParameters(out float armyDamage, out int incomeArmyCount)
        {
            incomeArmyCount = _formationService.NumberOfFollowers;
            armyDamage = Mathf.Round(_formationService.ArmyLevel + 1);
        }
    }
}