using System;

namespace Betting.Data
{
    [Serializable]
    public class BetSlotData
    {
        public int BetAmount;
        public BetType BetType;
        public int SlotId;
        
        public BetSlotData(int betAmount, BetType betType, int slotId)
        {
            BetAmount = betAmount;
            BetType = betType;
            SlotId = slotId;
        }

        public override string ToString()
        {
            string slotId = SlotId.ToString();
            if (BetType is BetType.Dozens or BetType.Columns)
            {
                if (SlotId == 0)
                    slotId = "1st";
                else if (SlotId == 1)
                    slotId = "2nd";
                else if (SlotId == 2)
                    slotId = "3rd";
            }
            else if (BetType is BetType.EvenOdd)
            {
                if (SlotId == 0)
                    slotId = "Even";
                else if (SlotId == 1)
                    slotId = "Odd";
            }
            else if (BetType is BetType.RedBlack)
            {
                if (SlotId == 0)
                    slotId = "Red";
                else if (SlotId == 1)
                    slotId = "Black";
            }
            else if (BetType is BetType.LowHigh)
            {
                if (SlotId == 0)
                    slotId = "Low";
                else if (SlotId == 1)
                    slotId = "High";
            }
            
            return $"{BetType}: {slotId} - ${BetAmount}";
        }
    }
}