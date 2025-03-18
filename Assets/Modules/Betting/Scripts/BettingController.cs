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
        public event Action<BetResultData> OnBetResult;
        
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
            _view.OnSlotClicked += SlotClicked;
            
            _bettingUIController = new BettingUIController(_dataStore.playerData.Get().Money);
            _bettingUIController.OnSpinButtonClicked += SpinButtonClicked;
            _bettingUIController.OnClearBetsButtonClicked += ClearBetsButtonClicked;
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
                _bettingUIController.Dispose();
            }
        }

        public void ResolveBets(int resultNumber)
        {
            int totalWinAmount = 0;
            foreach (BetSlotData bet in _activeBets)
            {
                bool win = false;
                switch (bet.BetType)
                {
                    case BetType.Straight:
                        win = bet.SlotId == resultNumber;
                        break;
                    case BetType.Split:
                        if (BetSlotMapping.SplitSlots.TryGetValue(bet.SlotId, out List<int> splitSlot))
                            win = splitSlot.Contains(resultNumber);
                        break;
                    case BetType.Street:
                        if (BetSlotMapping.StreetSlots.TryGetValue(bet.SlotId, out List<int> streetSlot))
                            win = streetSlot.Contains(resultNumber);
                        break;
                    case BetType.Corner:
                        if (BetSlotMapping.CornerSlots.TryGetValue(bet.SlotId, out List<int> cornerSlot))
                            win = cornerSlot.Contains(resultNumber);
                        break;
                    case BetType.SixLine:
                        if (BetSlotMapping.SixLineSlots.TryGetValue(bet.SlotId, out List<int> lineSlot))
                            win = lineSlot.Contains(resultNumber);
                        break;
                    case BetType.Dozens:
                        if (BetSlotMapping.DozensSlots.TryGetValue(bet.SlotId, out List<int> dozensSlot))
                            win = dozensSlot.Contains(resultNumber);
                        break;
                    case BetType.Columns:
                        if (BetSlotMapping.ColumnsSlots.TryGetValue(bet.SlotId, out List<int> slot))
                            win = slot.Contains(resultNumber);
                        break;
                    case BetType.EvenOdd:
                        if (BetSlotMapping.EvenOddSlots.TryGetValue(bet.SlotId, out string betSide))
                        {
                            win = (resultNumber != 0) &&
                                  ((betSide == BetConfig.EVEN_BET && resultNumber % 2 == 0) ||
                                   (betSide == BetConfig.ODD_BET && resultNumber % 2 != 0));
                        }
                        break;
                    case BetType.RedBlack:
                        if (BetSlotMapping.RedBlackSlots.TryGetValue(bet.SlotId, out string betColor))
                        {
                            win = (resultNumber != 0) &&
                                  ((betColor == BetConfig.RED_BET && BetConfig.RedNumbers.Contains(resultNumber)) ||
                                   (betColor == BetConfig.BLACK_BET && !BetConfig.RedNumbers.Contains(resultNumber)));
                        }
                        break;
                    case BetType.LowHigh:
                        if (BetSlotMapping.LowHighSlots.TryGetValue(bet.SlotId, out string betRange))
                        {
                            win = (resultNumber != 0) &&
                                  ((betRange == BetConfig.HIGH_BET && resultNumber >= 19) ||
                                   (betRange == BetConfig.LOW_BET && resultNumber <= 18));
                        }
                        break;
                    default:
                        break;
                }

                if (!win)
                    continue;
                
                BetConfig.BetRatios.TryGetValue(bet.BetType, out int ratio);
                totalWinAmount += bet.BetAmount * ratio;
            }

            BetResultData betResultData = new BetResultData(_activeBets.ToArray(), totalWinAmount);
            OnBetResult?.Invoke(betResultData);
        }

        public void SetSelectedChip(ChipSO selectedChip)
        {
            _selectedChip = selectedChip;
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
            
            _bettingUIController.SetMoneyText(playerData.Money);
            // TODO -> Lock the betting and spinning until the result is resolved, lower the money amount, etc.
            
            OnSpinBallClicked?.Invoke(result);
        }

        private void ClearBetsButtonClicked()
        {
            _totalBetAmount = 0;
            _activeBets.Clear();
            _bettingUIController.SetActiveBetsText(_totalBetAmount);
            _view.ClearBetDisplay();
        }
    }
}
