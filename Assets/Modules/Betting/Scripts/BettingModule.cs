using System;
using Betting.Data;
using Context.Interfaces;
using Context;

namespace Betting
{
    public class BettingModule : IDisposableObject
    {
        public event Action<int> OnSpinBall;
        public event Action<BetResultData> OnBetResult;

        private readonly BettingController _bettingController;
        private readonly ChipsController _chipsController;

        public BettingModule(DataStore dataStore)
        {
            _bettingController = new BettingController(dataStore);
            _chipsController = new ChipsController();

            _bettingController.OnSpinBallClicked += SpinBall;
            _chipsController.OnChipSelected += _bettingController.SetSelectedChip;
        }

        public void Dispose()
        {
            _bettingController.OnSpinBallClicked -= SpinBall;
            _chipsController.OnChipSelected -= _bettingController.SetSelectedChip;

            _bettingController.Dispose();
            _chipsController.Dispose();
        }

        public void ResolveBets(int resultNumber)
        {
            BetResultData betResultData = BetResolver.ResolveBets(_bettingController.GetActiveBets(), resultNumber);
            OnBetResult?.Invoke(betResultData);
        }
        
        public void ShowResult(BetResultData betResultData)
        {
            _bettingController.ShowResult(betResultData);
        }

        private void SpinBall(int result)
        {
            OnSpinBall?.Invoke(result);
        }
    }
}