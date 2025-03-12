using UnityEngine;

namespace Roulette
{
    public class RouletteModule
    {
        private RouletteController _rouletteController;
        private BallController _ballController;

        public RouletteModule()
        {
            _rouletteController = new RouletteController();
            _ballController = new BallController(_rouletteController.GetBallSpinPosition());
        }

        public void Dispose()
        {
            _rouletteController.Dispose();
            _ballController.Dispose();
        }
    }
}
