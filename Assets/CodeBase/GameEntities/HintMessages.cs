using CodeBase.Data;
using CodeBase.GameResources;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.UI.Services.Factory;

namespace CodeBase.GameEntities
{
    public class HintMessages
    {
        private readonly IUIFactory _uiFactory;
        private ResourceStack _resourceStack;
        private Gates _gates;
        private CharacterLogic _character;
        private readonly PlayerProgress _progress;
        private readonly ISaveLoadService _saveLoadService;

        private bool _gatesHintShown;
        private bool _armyUpgradeHintShown;
        private bool _attackHintShown;

        public HintMessages(IUIFactory uiFactory, Gates gates, CharacterLogic character,
            PlayerProgress progress, ISaveLoadService saveLoadService)
        {
            _character = character;
            _progress = progress;
            _saveLoadService = saveLoadService;
            _resourceStack = _character.ResourceStack;
            _gates = gates;
            _uiFactory = uiFactory;
            
            Subscribe();
        }

        private void Subscribe()
        {
            _character.OnAttackWithoutArmy += GatherArmy;
            if (_gates != null & !_progress.GatesHintsShown)
            {
                _gates.OnGatesUpgrade += CanUpgradeArmy;
                _resourceStack.OnResourcesChanged += CanUpgradeGates;
            }
        }

        private void GatherArmy()
        {
            if (!_attackHintShown)
            {
                _attackHintShown = true;
                _uiFactory.CreateMessage("First, gather an army.");
                Delay.Instance.Invoke(1f, () => _attackHintShown = false);
            }
        }

        private void CanUpgradeArmy()
        {
            if (!_armyUpgradeHintShown)
            {
                _armyUpgradeHintShown = true;
                _uiFactory.CreateMessage("Lead your army through the gates.");
                _gates.OnGatesUpgrade -= CanUpgradeArmy;
                _progress.GatesHintsShown = true;
                _saveLoadService.SaveProgress();
            }
        }

        private void CanUpgradeGates(int coinsAmount)
        {
            if (!_gatesHintShown && coinsAmount >=  _gates.ResourceShortage)
            {
                _gatesHintShown = true;
                _uiFactory.CreateMessage("You can repair the gates now.");
                _resourceStack.OnResourcesChanged -= CanUpgradeGates;
            }
        }
    }
}