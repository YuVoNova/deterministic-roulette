using System;
using System.Collections.Generic;
using Betting.Data;
using Context;
using Context.Interfaces;
using Player.Data;
using UnityEngine;

namespace Betting
{
    public class BettingController : IDisposableObject
    {
        public event Action<int> OnSpinBallClicked;
        
        private readonly DataStore _dataStore;
        private readonly IBettingView _view;
        private readonly BettingUIController _bettingUIController;
        private readonly List<BetSlotData> _activeBets = new List<BetSlotData>();
        
        private int _totalBetAmount = 0;
        private ChipSO _selectedChip;
        
        public BettingController(DataStore dataStore)
        {
            _dataStore = dataStore;
            _view = GameObject.FindObjectOfType<BettingView>();
            _view.Init();
            _bettingUIController = new BettingUIController(_dataStore.playerData.Get().Money);
            
            _view.OnSlotClicked += SlotClicked;
            _bettingUIController.OnSpinButtonClicked += SpinButtonClicked;
            _bettingUIController.OnClearBetsButtonClicked += ClearBetsButtonClicked;
            _bettingUIController.OnResultFinished += ResultFinished;
        }

        public void Dispose()
        {
            if (_view != null)
            {
                _view.OnSlotClicked -= SlotClicked;
                _view.Dispose();
            }
            
            if (_bettingUIController != null)
            {
                _bettingUIController.OnSpinButtonClicked -= SpinButtonClicked;
                _bettingUIController.OnClearBetsButtonClicked -= ClearBetsButtonClicked;
                _bettingUIController.OnResultFinished -= ResultFinished;
                _bettingUIController.Dispose();
            }
        }

        public void SetSelectedChip(ChipSO selectedChip)
        {
            _selectedChip = selectedChip;
        }
        
        public void ShowResult(BetResultData betResultData)
        {
            _bettingUIController.ShowResult(betResultData);
        }
        
        public List<BetSlotData> GetActiveBets()
        {
            return _activeBets;
        }
        
        private void SlotClicked(SlotObject slotObject)
        {
            if (_selectedChip == null)
                return;
            
            if (_totalBetAmount + _selectedChip.ChipValue > BetConfig.TOTAL_BET_LIMIT)
                return;

            int newBetAmount = 0;
            BetSlotData existingBet = _activeBets.Find(b => b.BetType == slotObject.betType && b.SlotId == slotObject.slotId);
            if (existingBet != null)
            {
                if (existingBet.BetAmount + _selectedChip.ChipValue > BetConfig.MAX_BET_AMOUNT)
                    return;
                
                existingBet.BetAmount += _selectedChip.ChipValue;
                newBetAmount = existingBet.BetAmount;
            }
            else
            {
                BetSlotData newBet = new BetSlotData(_selectedChip.ChipValue, slotObject.betType, slotObject.slotId);
                _activeBets.Add(newBet);
                newBetAmount = newBet.BetAmount;
            }
            
            _totalBetAmount += _selectedChip.ChipValue;
            _bettingUIController.SetActiveBetsText(_totalBetAmount);
            slotObject.AddBet(_selectedChip, newBetAmount);
        }
        
        private void SpinButtonClicked(int result)
        {
            if (_activeBets.Count == 0)
                return;

            PlayerData playerData = _dataStore.playerData.Get();
            if (!playerData.WithdrawMoney(_totalBetAmount))
                return;
            
            _view.ToggleSlots(false);
            _bettingUIController.SetMoneyText(playerData.Money);
            _bettingUIController.SpinStarted();
            
            OnSpinBallClicked?.Invoke(result);
        }

        private void ClearBetsButtonClicked()
        {
            _totalBetAmount = 0;
            _activeBets.Clear();
            _bettingUIController.SetActiveBetsText(_totalBetAmount);
            _view.ClearBetDisplay();
        }
        
        private void ResultFinished()
        {
            _view.ToggleSlots(true);
            // TODO -> Unlock the betting and spinning, update the player money amount
        }
    }
}
