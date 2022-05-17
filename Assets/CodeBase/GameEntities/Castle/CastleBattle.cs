using System.Collections;
using System.Collections.Generic;
using CodeBase.GameResources;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Formation;
using CodeBase.UI;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.GameEntities.Castle
{
    public class CastleBattle : Battle
    {
        private readonly BattleFloatingText _battleFloatingText;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private float _currentCitizens;
        private readonly BattleUI _battleUI;
        private readonly float _coinsOffset;
        private List<Collectible> _coins;

        public CastleBattle(CharacterLogic characterLogic, IBattleParticipant battleParticipant,
            IFormationService formationService, BattleFloatingText battleFloatingText,
            BattleUI battleUI, float coinsOffset)
            : base(characterLogic, battleParticipant, formationService) 
        {
            _coinsOffset = coinsOffset;
            _battleUI = battleUI;
            _battleFloatingText = battleFloatingText;
            _uiFactory = AllServices.Container.Single<IUIFactory>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }
    
        public override void StartBattle()
        {
            _currentCitizens = _battleParticipant.FightUnits;
            _battleUI.Activate();
            MoveObjectsToPositions();
            _battleParticipant.StartCoroutine(DecreaseArmyCount());
            _coins = new List<Collectible>();
        }

        private void MoveObjectsToPositions()
        {
            _characterLogic.CountText.BattleState(_battleUI.transform, 1);
            _characterLogic.MoveToBattlePosition(_battleParticipant.Transform.position);
        }

        protected override IEnumerator DecreaseArmyCount()
        {
            GetArmyParameters(out float armyDamage, out int incomeArmyCount);

            if (_characterLogic.Ride)
            {
                armyDamage = Mathf.Clamp(armyDamage * 2, 2, 5);
            }
        
            _battleFloatingText.SetMaterial((int)armyDamage);
        
            yield return new WaitForSeconds(1f);

            while (incomeArmyCount > 0 & _currentCitizens > 0)
            {
                incomeArmyCount -= 1;
                _currentCitizens -= armyDamage;

                UpdateUI(incomeArmyCount, (int)_currentCitizens);
                _battleFloatingText.Spawn();
                _formationService.KillFollower(false);
                CreateCoin();
                FightFeedbacks();

                float waitTime = Random.Range(0.2f, 0.6f);
                yield return new WaitForSeconds(waitTime);
            }
        
            if (_currentCitizens <= 0)
                HeroWin();
            else
                HeroLose();
        
            _battleUI.Deactivate();
        }

        private void CreateCoin()
        {
            Collectible coin = _gameFactory.CreateCoin(_battleParticipant.Transform.position + Vector3.up * 3.5f);
            coin.Jump(_battleParticipant.Transform.position, _coinsOffset);
            _coins.Add(coin);
        }

        private void FightFeedbacks()
        {
            _battleParticipant.FightUnits = (int) _currentCitizens;
            _battleParticipant.BattleFeedbacks.PlayFeedbacks();
        }

        protected override void HeroWin()
        {
            _battleParticipant.LoseBattle();
            _characterLogic.CountText.MoveLocalX(0);
            Delay.Instance.Invoke(1, Victory);
        }

        private void Victory()
        {
            CollectAllCoins();
            _uiFactory.CreateVictory();
        }

        private void CollectAllCoins()
        {
            foreach (Collectible coin in _coins)
            {
                _characterLogic.ResourceStack.ChangeJumpSettings();
                coin.Collect(_characterLogic.ResourceStack, true);
            }
        }

        protected override void HeroLose()
        {
            _characterLogic.MoveOutOfBattle(new Vector3(0, 0, _battleParticipant.Transform.position.z -7.5f), false);
            _battleParticipant.UpdateFightUnitsUI((int)_currentCitizens);
            _battleParticipant.WinBattle();
        }
    }
}