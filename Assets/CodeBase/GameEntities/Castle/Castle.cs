using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.Formation;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Pointers;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.GameEntities.Castle
{
    public class Castle : MonoBehaviour, IBattleParticipant, ISavedProgress
    {
        public MMFeedbacks HeroWinFeedbacks;
        public MMFeedbacks BattleFeedbackEffects;
        public BattleUI BattleUI;
        public UnitCountUI CitizenCountUI;
        public FireInCastle Fire;
        public Transform Door;
        public GameSound GameSound;
        public BattleFloatingText BattleFloatingText;

        private IFormationService _formationService;
        private ISaveLoadService _saveLoadService;
        private int _maxCitizens;
        private int _currentCitizens;
        private CastleBattle _castleBattle;
        private float _coinsOffset;
        private IPointersService _pointersService;
        private Pointer _canAttackPointer;
        public MMFeedbacks BattleFeedbacks => BattleFeedbackEffects;
        public Transform Transform => transform;
        public int FightUnits
        {
            get => _currentCitizens;
            set => _currentCitizens = value;
        }

        public void Construct(int numberOfCitizens, IFormationService formationService,
            ISaveLoadService saveLoadService, IPointersService pointersService, float coinsOffset)
        {
            _pointersService = pointersService;
            _coinsOffset = coinsOffset;
            _saveLoadService = saveLoadService;
            _formationService = formationService;
            _maxCitizens = numberOfCitizens;
        }

        private void Start()
        {
            _formationService.OnArmyNumberChange += CanAttackPointerEnable;
            CitizenCountUI.UpdateCount(_currentCitizens);
            Fire.Construct(_maxCitizens, this);
            _canAttackPointer = AddPointer();
        }

        private void CanAttackPointerEnable(int armyNumber)
        {
            _canAttackPointer.gameObject.SetActive(armyNumber > 0);
        }

        private Pointer AddPointer()
        {
            Pointer pointer = _pointersService.CreatePointer(transform.position);
            pointer.gameObject.SetActive(false);
            return pointer;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterLogic character))
            {
                if (_formationService.NumberOfFollowers <= 0 && _castleBattle == null)
                {
                    character.MoveOutOfBattle(new Vector3(0, 0, transform.position.z -7.5f), true);
                    return;
                }

                if (_castleBattle == null)
                    StartBattle(other);
            }
        }

        private void StartBattle(Collider other)
        {
            Door.DORotate(new Vector3(90, 0, 0), 0.2f);
            CitizenCountUI.MoveLocalX(xPosition: 1.75f);
            GameSound.PlayBattle();
            _castleBattle = new CastleBattle(other.GetComponent<CharacterLogic>(),
                this,
                _formationService,
                BattleFloatingText,
                BattleUI,
                _coinsOffset);
            _castleBattle.StartBattle();
        }

        public void UpdateFightUnitsUI(int currentValue) => 
            CitizenCountUI.UpdateCount(currentValue);

        public void LoseBattle()
        {
            Unsubscribe();
            HeroWinFeedbacks.PlayFeedbacks();
            GameSound.StopBattle();

            SaveProgress();
        }

        private void Unsubscribe() => 
            _formationService.OnArmyNumberChange -= CanAttackPointerEnable;

        public void WinBattle()
        {
            GameSound.StopBattle();
            CitizenCountUI.MoveLocalX(xPosition: 0);
            Delay.Instance.Invoke(2, BlockEnter);
            
            SaveProgress();
        }

        private void SaveProgress() => 
            _saveLoadService.SaveProgress();

        private void BlockEnter()
        {
            Door.DORotate(Vector3.zero, 0.5f);
            _castleBattle = null;
        }

        public void LoadProgress(PlayerProgress progress, string sceneName) => 
            _currentCitizens = progress.DataOnLevel[sceneName].CastleCitizens;

        public void UpdateProgress(PlayerProgress progress, string sceneName) => 
            progress.DataOnLevel[sceneName].CastleCitizens = _currentCitizens;
    }
}