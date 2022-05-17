using System;
using System.Collections;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class VictoryWindow : BaseWindow
    {
        public Button nextLevelButton; 
        
        private IGameStateMachine _stateMachine;
        private IStaticDataService _staticDataService;
        private ISaveLoadService _saveLoadService;
        protected override void OnAwake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _staticDataService = AllServices.Container.Single<IStaticDataService>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            
            nextLevelButton.onClick.AddListener(TransferToNextLevel);
        }
        
        private void TransferToNextLevel()
        {
            _saveLoadService.SaveProgress();
            string transferToScene = _staticDataService.
                ForLevel(SceneManager.GetActiveScene().name).NextLevelKey;

            if (transferToScene == "Level 1")
            {
                _saveLoadService.Clear();
            }
            
            StartCoroutine(InvokeWithDelay(1, () =>
            {
                _stateMachine.Enter<LoadLevelState, string>(transferToScene);
            }));
        }

        private IEnumerator InvokeWithDelay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}
