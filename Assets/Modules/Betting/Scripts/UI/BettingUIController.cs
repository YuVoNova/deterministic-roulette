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
        
        private readonly IBettingUIView _bettingView;
        private readonly IResultUIView _resultView;
        
        public BettingUIController(int money)
        {
            _bettingView = GameObject.FindObjectOfType<BettingUIView>();
            _bettingView.Init();
            _bettingView.OnSpinButtonClicked += SpinButtonClicked;
            _bettingView.OnClearBetsButtonClicked += ClearBetsButtonClicked;
            
            _resultView = GameObject.FindObjectOfType<ResultUIView>();
            _resultView.HideResult();
            _resultView.OnResultFinished += ResultFinished;
            
            SetMoneyText(money);
        }
        
        public void Dispose()
        {
            if (_bettingView == null)
                return;
            
            _bettingView.OnSpinButtonClicked -= SpinButtonClicked;
            _bettingView.OnClearBetsButtonClicked -= ClearBetsButtonClicked;
            _bettingView.Dispose();
            
            if (_resultView == null)
                return;
            
            _resultView.OnResultFinished -= ResultFinished;
        }
        
        public void SetMoneyText(int amount)
        {
            _bettingView.SetMoneyText(amount);
        }
        
        public void SetActiveBetsText(int amount)
        {
            _bettingView.SetActiveBetsText(amount);
        }
        
        public void ShowResult(BetResultData betResultData)
        {
            _resultView.ShowResult(betResultData);
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
            _resultView.HideResult();
            OnResultFinished?.Invoke();
        }
    }
}