using UnityEngine;
using Context;
using Roulette;
using Betting;
using Betting.Data;
using Player;
using Utils;

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

        private BetResultData _betResultData;

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

            _bettingModule.OnSpinBall -= GenerateResultAndSpinBall;
            _bettingModule.OnBetResult -= BetResolved;
            _rouletteModule.OnBallStopped -= BallStopped;

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

            _bettingModule.OnSpinBall += GenerateResultAndSpinBall;
            _bettingModule.OnBetResult += BetResolved;
            _rouletteModule.OnBallStopped += BallStopped;

            _isGameStarted = true;
        }

        private void GenerateResultAndSpinBall(int deterministicResult = -1)
        {
            int result = deterministicResult;
            if (result is < Const.MIN_POCKET_VALUE or > Const.MAX_POCKET_VALUE)
                result = Random.Range(Const.MIN_POCKET_VALUE, Const.MAX_POCKET_VALUE + 1);

            _bettingModule.ResolveBets(result);
            _rouletteModule.SpinBall(result);
        }

        private void BetResolved(BetResultData betResultData)
        {
            _betResultData = betResultData;
        }

        private void BallStopped()
        {
            _bettingModule.ShowResult(_betResultData);
        }
    }
}