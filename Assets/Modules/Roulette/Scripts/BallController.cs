using System;
using UnityEngine;
using Utils;

namespace Roulette
{
    public class BallController
    {
        public event Action<int> OnBallResult;
        
        private readonly IBallView _view;
        
        public BallController(Vector3 ballSpinPosition)
        {
            _view = GameObject.FindObjectOfType<BallView>();
            _view.Init(ballSpinPosition);
            _view.Standby();
        }

        public void Dispose()
        {
            _view?.Dispose();
        }

        public void SpinBall(int deterministicResult = -1)
        {
            // No valid deterministic result is given, ball will end in a random pocket.
            if (deterministicResult is < Const.MIN_POCKET_VALUE or > Const.MAX_POCKET_VALUE)
            {
                
            }
        }
    }
}