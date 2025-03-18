using System.Collections.Generic;
using Betting.Data;

namespace Betting
{
    public static class BetConfig
    {
        public const string EVEN_BET = "Even";
        public const string ODD_BET = "Odd";
        public const string RED_BET = "Red";
        public const string BLACK_BET = "Black";
        public const string HIGH_BET = "High";
        public const string LOW_BET = "Low";

        public const int MAX_BET_AMOUNT = 250;
        public const int TOTAL_BET_LIMIT = 1000;
        
        public static readonly Dictionary<BetType, int> BetRatios = new Dictionary<BetType, int>
        {
            { BetType.Straight, 36 },
            { BetType.Split, 18 },
            { BetType.Street, 12 },
            { BetType.Corner, 9 },
            { BetType.SixLine, 6 },
            { BetType.Dozens, 3 },
            { BetType.Columns, 3 },
            { BetType.EvenOdd, 2 },
            { BetType.RedBlack, 2 },
            { BetType.LowHigh, 2 }
        };
        
        public static readonly List<int> RedNumbers = new List<int>
        {
            1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36
        };
    }
}