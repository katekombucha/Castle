using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string FirstLevel = "Level 1";
        private const string ProgressKey = "Progress";
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _staticDataService;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory,
            IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void SaveProgress()
        {
            string levelName = SceneManager.GetActiveScene().name;
            
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress, levelName);

            _progressService.Progress.CurrentLevel = levelName;
            ES3.Save(ProgressKey, _progressService.Progress);
        }

        public PlayerProgress LoadProgress()
        {
            if (ES3.KeyExists(ProgressKey))
            {
                PlayerProgress progress = ES3.Load<PlayerProgress>(ProgressKey);
                return progress;
            }
            else
                return null;
        }

        public void Clear()
        {
            _progressService.Progress = new PlayerProgress(FirstLevel, _staticDataService);
            ES3.Save(ProgressKey, _progressService.Progress);
        }

        public void SaveNewLevel(string levelName)
        {
            _progressService.Progress.CurrentLevel = levelName;
            ES3.Save(ProgressKey, _progressService.Progress);
        }
    }
}