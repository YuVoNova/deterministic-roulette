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
    }
}