using UnityEngine;

namespace Roulette
{
    public class RouletteController
    {
        private readonly IRouletteView _view;
        
        public RouletteController()
        {
            _view = GameObject.FindObjectOfType<RouletteView>();
            _view.Init();
        }

        public void Dispose()
        {
            _view?.Dispose();
        }

        public Vector3 GetBallSpinPosition()
        {
            return _view.BallSpinPosition;
        }
    }
}