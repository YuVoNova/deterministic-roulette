using System.Collections.Generic;
using Utils;

namespace Betting
{
    public static class BetSlotMapping
    {
        // Straight Bets: Every single number.
        public static readonly Dictionary<int, int> StraightSlots = new Dictionary<int, int>();

        // Split Bets: Every two adjacent numbers.
        public static readonly Dictionary<int, List<int>> SplitSlots = new Dictionary<int, List<int>>();

        // Street Bets: Every three numbers in a row.
        public static readonly Dictionary<int, List<int>> StreetSlots = new Dictionary<int, List<int>>();

        // Corner Bets: Every corner of four numbers.
        public static readonly Dictionary<int, List<int>> CornerSlots = new Dictionary<int, List<int>>();

        // Six Line Bets: Every two adjacent rows of numbers.
        public static readonly Dictionary<int, List<int>> SixLineSlots = new Dictionary<int, List<int>>();

        // Dozen Bets: Every dozen numbers. (1-12, 13-24, 25-36)
        public static readonly Dictionary<int, List<int>> DozensSlots = new Dictionary<int, List<int>>();

        // Column Bets: Every column of numbers.
        public static readonly Dictionary<int, List<int>> ColumnsSlots = new Dictionary<int, List<int>>();

        // Category Bets: Even/Odd, Red/Black, Low/High.
        public static readonly Dictionary<int, string> EvenOddSlots = new Dictionary<int, string>();
        public static readonly Dictionary<int, string> RedBlackSlots = new Dictionary<int, string>();
        public static readonly Dictionary<int, string> LowHighSlots = new Dictionary<int, string>();

        static BetSlotMapping()
        {
            // Straight Bets
            for (int i = Const.MIN_POCKET_VALUE; i <= Const.MAX_POCKET_VALUE; i++)
            {
                StraightSlots[i] = i;
            }

            // Split Bets
            int splitKey = 0;
            // Horizontal Split Bets
            for (int row = 0; row < 12; row++)
            {
                int baseNum = row * 3 + 1;
                // Horizontal Split 1
                SplitSlots[splitKey++] = new List<int> { baseNum, baseNum + 1 };
                // Horizontal Split 2
                SplitSlots[splitKey++] = new List<int> { baseNum + 1, baseNum + 2 };
            }

            // Vertical Split Bets
            // Vertical Splits 1
            for (int row = 0; row < 11; row++)
            {
                int num1 = row * 3 + 1;
                int num2 = (row + 1) * 3 + 1;
                SplitSlots[splitKey++] = new List<int> { num1, num2 };
            }

            // Vertical Splits 2
            for (int row = 0; row < 11; row++)
            {
                int num1 = row * 3 + 2;
                int num2 = (row + 1) * 3 + 2;
                SplitSlots[splitKey++] = new List<int> { num1, num2 };
            }

            // Vertical Splits 3
            for (int row = 0; row < 11; row++)
            {
                int num1 = row * 3 + 3;
                int num2 = (row + 1) * 3 + 3;
                SplitSlots[splitKey++] = new List<int> { num1, num2 };
            }

            // Street Bets
            for (int row = 0; row < 12; row++)
            {
                int baseNum = row * 3 + 1;
                StreetSlots[row] = new List<int> { baseNum, baseNum + 1, baseNum + 2 };
            }

            // Corner Bets
            int cornerKey = 0;
            for (int row = 0; row < 11; row++)
            {
                int baseNum = row * 3 + 1;
                
                // Corner 1
                CornerSlots[cornerKey++] = new List<int> { baseNum, baseNum + 1, baseNum + 3, baseNum + 4 };
                // Corner 2
                CornerSlots[cornerKey++] = new List<int> { baseNum + 1, baseNum + 2, baseNum + 4, baseNum + 5 };
            }

            // Six Line Bets
            int sixLineKey = 0;
            for (int row = 0; row < 11; row++)
            {
                int baseNum = row * 3 + 1;
                SixLineSlots[sixLineKey] = new List<int>
                {
                    baseNum, baseNum + 1, baseNum + 2,
                    baseNum + 3, baseNum + 4, baseNum + 5
                };
                sixLineKey++;
            }

            // Dozens
            DozensSlots[0] = GenerateRange(1, 12);
            DozensSlots[1] = GenerateRange(13, 24);
            DozensSlots[2] = GenerateRange(25, 36);

            // Columns
            ColumnsSlots[0] = new List<int>();
            ColumnsSlots[1] = new List<int>();
            ColumnsSlots[2] = new List<int>();
            for (int row = 0; row < 12; row++)
            {
                int baseNum = row * 3;
                ColumnsSlots[0].Add(baseNum + 1);
                ColumnsSlots[1].Add(baseNum + 2);
                ColumnsSlots[2].Add(baseNum + 3);
            }

            // Even-Odd Bets
            EvenOddSlots[0] = BetConfig.EVEN_BET;
            EvenOddSlots[1] = BetConfig.ODD_BET;

            // Red-Black Bets
            RedBlackSlots[0] = BetConfig.RED_BET;
            RedBlackSlots[1] = BetConfig.BLACK_BET;

            // Low-High Bets (0: 1-18, 1: 19-36)
            LowHighSlots[0] = BetConfig.LOW_BET;
            LowHighSlots[1] = BetConfig.HIGH_BET;
        }

        private static List<int> GenerateRange(int start, int end)
        {
            List<int> list = new List<int>();
            for (int i = start; i <= end; i++)
            {
                list.Add(i);
            }

            return list;
        }
    }
}