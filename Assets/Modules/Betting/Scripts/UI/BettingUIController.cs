using System;
using Context.Interfaces;

namespace Betting
{
    public class BettingUIController : IDisposableObject
    {
        public event Action OnSpinButtonClicked;
        public event Action OnClearBetsButtonClicked;
        
        private readonly IBettingUIView _view;
        
        public BettingUIController()
        {
            _view = new BettingUIView();
            _view.Init();
            _view.OnSpinButtonClicked += SpinButtonClicked;
            _view.OnClearBetsButtonClicked += ClearBetsButtonClicked;
        }
        
        public void Dispose()
        {
            if (_view == null)
                return;
            
            _view.OnSpinButtonClicked -= SpinButtonClicked;
            _view.OnClearBetsButtonClicked -= ClearBetsButtonClicked;
            _view.Dispose();
        }
        
        private void SpinButtonClicked()
        {
            OnSpinButtonClicked?.Invoke();
        }
        
        private void ClearBetsButtonClicked()
        {
            OnClearBetsButtonClicked?.Invoke();
        }
    }
}