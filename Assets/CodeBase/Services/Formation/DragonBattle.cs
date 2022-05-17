using System.Collections;
using CodeBase.GameEntities;
using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.Services.Formation
{
    public class DragonBattle : Battle
    {
        private Coroutine fightCoroutine;
        private float _attackDelay = 0.7f;
        private float _currentHealth;

        public DragonBattle(CharacterLogic characterLogic, IBattleParticipant battleParticipant, IFormationService formationService) 
            : base(characterLogic, battleParticipant, formationService) { }

        public override void StartBattle()
        {
            if (_formationService.NumberOfFollowers == 0)
            {
                MoveCharacterOut(true);
                return;
            }
        
            _currentHealth = _battleParticipant.FightUnits;
            _formationService.DragonFight(true);
            _formationService.SetNewTarget(_battleParticipant.Transform);
            fightCoroutine = _battleParticipant.StartCoroutine(DecreaseArmyCount());
        }

        protected override IEnumerator DecreaseArmyCount()
        {
            GetArmyParameters(out float armyDamage, out int incomeArmyCount);
        
            _battleParticipant.BattleFeedbacks.PlayFeedbacks();
        
            while (incomeArmyCount > 0 & _currentHealth > 0)
            {
                yield return new WaitForSeconds(_attackDelay);
                incomeArmyCount -= 1;
                _currentHealth -= armyDamage;

                UpdateUI(incomeArmyCount, (int)_currentHealth);
                _formationService.KillFollower(true);
            }
        
            if(_currentHealth <= 0)
                HeroWin();
            else
                HeroLose();
        }

        protected override void HeroWin()
        {
            StopBattle();
            _battleParticipant.LoseBattle();
            _characterLogic.RideDragon();
            _formationService.SpeedUpArmy();
        }

        protected override void HeroLose()
        {
            StopBattle();
            _battleParticipant.WinBattle();
            _battleParticipant.FightUnits = (int) _currentHealth;
            _battleParticipant.UpdateFightUnitsUI((int)_currentHealth);
            MoveCharacterOut(false);
        }

        private void MoveCharacterOut(bool withoutArmy)
        {
            float rotation =_battleParticipant.Transform.rotation.y;
            Vector3 loseOffset =
                rotation < 0.8f ? new Vector3(8, 0, 0) : new Vector3(0, 0, -8);
            _characterLogic.MoveOutOfBattle(_battleParticipant.Transform.position + loseOffset, withoutArmy);
        }

        private void StopBattle()
        {
            if (fightCoroutine != null) _battleParticipant.StopCoroutine(fightCoroutine);
            _battleParticipant.BattleFeedbacks.StopFeedbacks();
            Delay.Instance.Invoke(0.1f, () => _formationService.DragonFight(false));
            _formationService.SetNewTarget(_characterLogic.transform);
        }
    }
}