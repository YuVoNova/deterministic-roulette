using System;
using Context.Interfaces;
using UnityEngine;

namespace Betting
{
    public class BettingUIController : IDisposableObject
    {
        public event Action<int> OnSpinButtonClicked;
        public event Action OnClearBetsButtonClicked;
        
        private readonly IBettingUIView _view;
        
        public BettingUIController(int money)
        {
            _view = GameObject.FindObjectOfType<BettingUIView>();
            _view.Init();
            _view.OnSpinButtonClicked += SpinButtonClicked;
            _view.OnClearBetsButtonClicked += ClearBetsButtonClicked;
            
            SetMoneyText(money);
        }
        
        public void Dispose()
        {
            if (_view == null)
                return;
            
            _view.OnSpinButtonClicked -= SpinButtonClicked;
            _view.OnClearBetsButtonClicked -= ClearBetsButtonClicked;
            _view.Dispose();
        }
        
        public void SetMoneyText(int amount)
        {
            _view.SetMoneyText(amount);
        }
        
        public void SetActiveBetsText(int amount)
        {
            _view.SetActiveBetsText(amount);
        }
        
        private void SpinButtonClicked(int result)
        {
            OnSpinButtonClicked?.Invoke(result);
        }
        
        private void ClearBetsButtonClicked()
        {
            OnClearBetsButtonClicked?.Invoke();
        }
    }
}