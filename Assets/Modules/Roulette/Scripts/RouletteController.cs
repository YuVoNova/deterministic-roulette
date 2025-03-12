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
        
        
    }
}