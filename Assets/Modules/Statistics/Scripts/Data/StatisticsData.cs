using System;
using System.Collections.Generic;
using Betting.Data;
using UnityEngine;

namespace Statistics
{
    [Serializable]
    public class StatisticsData
    {
        public BetResultData[] PlayHistory => _playHistory;
        public int TotalBalance => _totalBalance;
        
        [SerializeField] private BetResultData[] _playHistory;
        [SerializeField] private int _totalBalance;
        
        private List<BetResultData> _playHistoryList;

        public StatisticsData(BetResultData[] playHistory, int totalBalance)
        {
            _playHistory = playHistory;
            _totalBalance = totalBalance;
            
            _playHistoryList = new List<BetResultData>(_playHistory);
        }

        public StatisticsData()
        {
            _playHistory = null;
            _totalBalance = 0;
            
            _playHistoryList = new List<BetResultData>();
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }

        public static StatisticsData FromJson(string json)
        {
            return JsonUtility.FromJson<StatisticsData>(json);
        }

        public void AddBetResult(BetResultData betResultData)
        {
            _playHistoryList.Add(betResultData);
            _playHistory = _playHistoryList.ToArray();
            
            _totalBalance += betResultData.BalanceAmount;
        }
        
        public void CalculateBetResults()
        {
            _playHistoryList.ForEach(betResultData => betResultData.CalculateBalanceAmount());
        }
    }
}