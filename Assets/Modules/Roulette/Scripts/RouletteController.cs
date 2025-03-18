using UnityEngine;

namespace Roulette
{
    public class RouletteController
    {
        private readonly IRouletteView _view;

        public RouletteController()
        {
            _view = GameObject.FindObjectOfType<RouletteView>();
        }

        public void Dispose()
        {
            _view?.Dispose();
        }

        public Transform GetPocket(int pocketValue)
        {
            return _view.GetPocket(pocketValue);
        }

        public Vector3 GetBallSpinPosition()
        {
            return _view.BallSpinPosition;
        }

        public Transform GetRotatingWheel()
        {
            return _view.GetRotatingWheel();
        }
    }
}