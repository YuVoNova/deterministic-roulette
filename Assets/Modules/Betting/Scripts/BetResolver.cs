using System;
using System.Collections.Generic;
using Betting.Data;

namespace Betting
{
    public static class BetResolver
    {
        public static BetResultData ResolveBets(List<BetSlotData> activeBets, int resultNumber)
        {
            int totalWinAmount = 0;
            foreach (BetSlotData bet in activeBets)
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

            SlotColors resultColor;
            if (resultNumber == 0)
                resultColor = SlotColors.Green;
            else
                resultColor = BetConfig.RedNumbers.Contains(resultNumber) ? SlotColors.Red : SlotColors.Black;
            
            BetResultData betResultData = new BetResultData(activeBets.ToArray(), resultNumber, resultColor, totalWinAmount);
            betResultData.CalculateBalanceAmount();
            return betResultData;
        }
    }
}