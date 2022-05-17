using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase
{
    public class GameManager : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;

        public void Construct(ISaveLoadService saveLoadService) => 
            _saveLoadService = saveLoadService;

        private void OnApplicationPause(bool pauseStatus) => 
            _saveLoadService.SaveProgress();
    }
}
