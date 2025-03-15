using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Roulette
{
    public class RouletteModule
    {
        public event Action OnBallStopped;

        private readonly RouletteController _rouletteController;
        private readonly BallController _ballController;

        public RouletteModule()
        {
            _rouletteController = new RouletteController();
            _ballController = new BallController(_rouletteController.GetRotatingWheel(), _rouletteController.GetBallSpinPosition());

            _ballController.OnBallStopped += BallStopped;
        }

        public void Dispose()
        {
            if (_ballController != null)
            {
                _ballController.OnBallStopped -= BallStopped;
                _ballController.Dispose();
            }

            _rouletteController?.Dispose();
        }

        public void SpinBall(int deterministicResult = -1)
        {
            int result = deterministicResult;
            if (result is < Const.MIN_POCKET_VALUE or > Const.MAX_POCKET_VALUE)
                result = Random.Range(Const.MIN_POCKET_VALUE, Const.MAX_POCKET_VALUE + 1);

            Debug.Log(result);
            Transform targetPocketTransform = _rouletteController.GetPocket(result);
            _ballController.SpinBall(targetPocketTransform);
        }

        private void BallStopped()
        {
            OnBallStopped?.Invoke();
        }
    }
}