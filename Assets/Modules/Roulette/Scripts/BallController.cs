using System;
using UnityEngine;

namespace Roulette
{
    public class BallController
    {
        public event Action OnBallStopped;

        private readonly IBallView _view;

        public BallController(Transform rotatingWheel, Vector3 ballSpinPosition)
        {
            _view = GameObject.FindObjectOfType<BallView>();
            _view.Init(rotatingWheel, ballSpinPosition);
            _view.OnBallStopped += BallStopped;
        }

        public void Dispose()
        {
            if (_view == null)
                return;

            _view.OnBallStopped -= BallStopped;
            _view.Dispose();
        }

        public void SpinBall(Transform targetPocketTransform)
        {
            if (targetPocketTransform == null)
                return;

            _view.StartBallSpin(targetPocketTransform);
        }

        private void BallStopped()
        {
            OnBallStopped?.Invoke();
        }
    }
}