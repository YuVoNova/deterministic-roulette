using System;
using Betting.Data;
using Context.Interfaces;
using Context;
using UnityEngine;

namespace Betting
{
    public class BettingModule : IDisposableObject
    {
        public event Action<int> OnSpinBall;
        public event Action<BetResultData> OnBetResult;

        private readonly DataStore _dataStore;
        private readonly BettingController _bettingController;
        private readonly ChipsController _chipsController;

        public BettingModule(DataStore dataStore)
        {
            _dataStore = dataStore;
            _bettingController = new BettingController(_dataStore);
            _chipsController = new ChipsController();

            _bettingController.OnSpinBallClicked += SpinBall;
            _chipsController.OnChipSelected += ChipSelected;
            
            int initialSelectedChipId = _dataStore.playerData.Get().SelectedChipId;
            _chipsController.InitialSelectedChip(initialSelectedChipId);
            
            BetSlotData[] activeBets = _dataStore.playerData.Get().ActiveBets;
            _bettingController.InitializeActiveBets(activeBets);
        }

        public void Dispose()
        {
            _bettingController.OnSpinBallClicked -= SpinBall;
            _chipsController.OnChipSelected -= ChipSelected;

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
        
        private void ChipSelected(ChipSO chipSO)
        {
            _dataStore.playerData.Get().SetSelectedChipId(chipSO.ChipId);
            _bettingController.SetSelectedChip(chipSO);
        }
    }
}