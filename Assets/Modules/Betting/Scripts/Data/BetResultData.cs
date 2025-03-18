using System;
using System.Linq;

namespace Betting.Data
{
    [Serializable]
    public class BetResultData
    {
        public BetSlotData[] BetSlots;
        public int ResultNumber;
        public SlotColors ResultColor;
        public int WinAmount;
        public int TotalBetAmount;
        public int ProfitAmount;
        
        public BetResultData(BetSlotData[] betSlots, int resultNumber, SlotColors resultColor, int winAmount)
        {
            BetSlots = betSlots;
            ResultNumber = resultNumber;
            ResultColor = resultColor;
            WinAmount = winAmount;
            TotalBetAmount = CalculateTotalBetAmount();
            ProfitAmount = WinAmount - TotalBetAmount;
        }
        
        private int CalculateTotalBetAmount()
        {
            return BetSlots.Sum(betSlot => betSlot.BetAmount);
        }
    }
}