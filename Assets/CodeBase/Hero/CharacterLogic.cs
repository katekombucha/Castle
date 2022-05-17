using System;
using CodeBase.Equipment;
using CodeBase.GameEntities;
using CodeBase.GameEntities.Dragon;
using CodeBase.GameEntities.Villager;
using CodeBase.GameResources;
using CodeBase.Services.Formation;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.Hero
{
    public class CharacterLogic : MonoBehaviour, ISwimmer
    {
        public CharacterAnimator CharacterAnimator;
        public ResourceStack ResourceStack;
        public UnitCountUI CountText;
        public Transform HeroModelTransform;
        public ArmyUpgradeUI ArmyUpgradeUI;
        public UnitEquipment UnitEquipment;
        
        [Header("Feedbacks")]
        public MMFeedbacks ScreamFeedback;
        public MMFeedbacks CollectiblesFeedbacks;
        public ParticleSystem BubbleParticles;

        public Action OnCastleBattle;
        public Action OnLose;
        public Action OnAttackWithoutArmy;
        
        private IFormationService _formationService;
        private CharacterMove _characterMove;
        private bool _ride;
        public bool Ride => _ride;

        private void Start()
        {
            _characterMove = GetComponent<CharacterMove>();
        }

        public void Construct(IFormationService formationService, IStaticDataService staticDataService)
        {
            _formationService = formationService;
            _formationService.OnArmyNumberChange += UpdateText;
            _formationService.SetNewTarget(transform);
            UnitEquipment.Initialize(staticDataService);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Unit>())
            {
                ScreamFeedback.PlayFeedbacks();
                return;
            }

            if (other.GetComponent<Collectible>())
            {
                CollectiblesFeedbacks.PlayFeedbacks();
                return;
            }

            if (other.GetComponent<Water>() && !_ride)
            {
                CharacterAnimator.PlaySwim();
                BubbleParticles.Play();
                HeroModelTransform.DOLocalMoveY(-2, 0.1f);
            }
            
            if (other.GetComponent<DragonController>())
            {
                CharacterAnimator.PlayFight();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<DragonController>())
            {
                CharacterAnimator.StopFight();
            }
        }

        private void UpdateText(int newNum) => 
            CountText.UpdateCount(newNum);


        public void MoveToBattlePosition(Vector3 castlePosition)
        {
            if (_ride)
            {
                OnCastleBattle?.Invoke();
                HeroModelTransform.DOLocalMove(new Vector3(0, 4.5f, -8f), 0.5f);
                _characterMove.MoveTo(castlePosition, .5f, () => _characterMove.BattleRotate());
            }

            else
            {
                _characterMove.MoveTo(castlePosition, .5f);
            }
        }

        public void MoveOutOfBattle(Vector3 outPosition, bool withoutArmy)
        {
            if (_ride)
                StopUseDragon();
            
            CharacterAnimator.PlayKicked();
            _characterMove.MoveTo(outPosition, .8f, () => _characterMove.ResetControl());
            
            if (withoutArmy)
            {
                OnAttackWithoutArmy?.Invoke();
            }

            CountText.NormalState(transform, Vector3.up * 4, 0.7f);
        }

        public void RideDragon()
        {
            _ride = true;
            CharacterAnimator.StopFight();
            CharacterAnimator.PlayRide();
            HeroModelTransform.DOLocalMoveY(1, 0.5f);
            _characterMove.MovementSpeed *= 1.3f;
            ArmyUpgradeUI.ShowFire();
        }

        private void StopUseDragon()
        {
            OnLose?.Invoke();
            _ride = false;
            CharacterAnimator.StopRide();
            HeroModelTransform.DOLocalMove(Vector3.zero, 0.2f);
            _characterMove.MovementSpeed /= 1.3f;
            _formationService.SlowDownArmy();
            ArmyUpgradeUI.HideFire();
        }

        private void OnDisable() => 
            _formationService.OnArmyNumberChange -= UpdateText;

        public bool SwimCondition => !_ride;
        public void StopSwim()
        {
            CharacterAnimator.StopSwim();
            BubbleParticles.Stop();
            HeroModelTransform.DOLocalMoveY(0, 0.1f).SetEase(Ease.OutFlash);
        }
    }
}