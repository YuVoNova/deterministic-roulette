using UnityEngine;
using Context;
using Roulette;
using Betting;
using Player;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        private ContextManager _contextManager;
        private ISceneLoader _sceneLoader;

        private PlayerModule _playerModule;
        private RouletteModule _rouletteModule;
        private BettingModule _bettingModule;
        
        private bool _isGameStarted = false;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _sceneLoader = new SceneLoader();
            _sceneLoader.OnSceneLoaded += SceneLoaded;
        }

        private void Start()
        {
            _sceneLoader.LoadSceneAsync(Scenes.Game);
        }

        private void OnDestroy()
        {
            _sceneLoader?.Dispose();

            if (!_isGameStarted)
                return;
            
            _bettingModule.OnSpinBall -= _rouletteModule.SpinBall;
            
            _rouletteModule.Dispose();
            _bettingModule.Dispose();
            _playerModule.Dispose();
            
            // We dispose context manager last because Statistics and Player modules uses FileService to save their data.
            _contextManager.Dispose();
        }

        private void SceneLoaded()
        {
            _sceneLoader.OnSceneLoaded -= SceneLoaded;
            _sceneLoader.Dispose();

            _contextManager = new ContextManager();

            _playerModule = new PlayerModule(_contextManager.FileService, _contextManager.DataStore);
            _rouletteModule = new RouletteModule();
            _bettingModule = new BettingModule(_contextManager.DataStore);
            
            _bettingModule.OnSpinBall += _rouletteModule.SpinBall;
            
            _isGameStarted = true;
        }
    }
}