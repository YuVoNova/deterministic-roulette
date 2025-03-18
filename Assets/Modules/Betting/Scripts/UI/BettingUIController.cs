using System;
using Betting.Data;
using Context.Interfaces;
using UnityEngine;

namespace Betting
{
    public class BettingUIController : IDisposableObject
    {
        public event Action<int> OnSpinButtonClicked;
        public event Action OnClearBetsButtonClicked;
        public event Action OnResultFinished;
        
        private readonly IBettingUIView _bettingUIView;
        private readonly IResultUIView _resultUIView;
        
        public BettingUIController(int money)
        {
            _bettingUIView = GameObject.FindObjectOfType<BettingUIView>();
            _bettingUIView.Init();
            _bettingUIView.OnSpinButtonClicked += SpinButtonClicked;
            _bettingUIView.OnClearBetsButtonClicked += ClearBetsButtonClicked;
            
            _resultUIView = GameObject.FindObjectOfType<ResultUIView>();
            _resultUIView.ToggleUI(false);
            _resultUIView.OnResultFinished += ResultFinished;
            
            SetMoneyText(money);
        }
        
        public void Dispose()
        {
            if (_bettingUIView == null)
                return;
            
            _bettingUIView.OnSpinButtonClicked -= SpinButtonClicked;
            _bettingUIView.OnClearBetsButtonClicked -= ClearBetsButtonClicked;
            _bettingUIView.Dispose();
            
            if (_resultUIView == null)
                return;
            
            _resultUIView.OnResultFinished -= ResultFinished;
        }
        
        public void SetMoneyText(int amount)
        {
            _bettingUIView.SetMoneyText(amount);
        }
        
        public void SetActiveBetsText(int amount)
        {
            _bettingUIView.SetActiveBetsText(amount);
        }
        
        public void ShowResult(BetResultData betResultData)
        {
            _resultUIView.ShowResult(betResultData);
        }
        
        public void SpinStarted()
        {
            _bettingUIView.ToggleButtons(false);
        }
        
        private void SpinButtonClicked(int result)
        {
            OnSpinButtonClicked?.Invoke(result);
        }
        
        private void ClearBetsButtonClicked()
        {
            OnClearBetsButtonClicked?.Invoke();
        }

        private void ResultFinished()
        {
            _resultUIView.ToggleUI(false);
            _bettingUIView.ToggleUI(true);
            _bettingUIView.ToggleButtons(true);
            OnResultFinished?.Invoke();
        }
    }
}