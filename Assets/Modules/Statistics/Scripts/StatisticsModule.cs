using System.Collections.Generic;
using Betting.Data;
using Context;
using Context.Interfaces;

namespace Statistics
{
    public class StatisticsModule : IDisposableObject
    {
        private readonly StatisticsDataHandler _dataHandler;
        private readonly StatisticsUIController _statisticsUIController;
        private readonly StatisticsData _statisticsData;

        public StatisticsModule(IFileService fileService)
        {
            _dataHandler = new StatisticsDataHandler(fileService);
            _statisticsData = _dataHandler.LoadData();
            _statisticsData.CalculateBetResults();
            
            _statisticsUIController = new StatisticsUIController(_statisticsData);
        }

        public void Dispose()
        {
            Save();
            
            _statisticsUIController.Dispose();
        }

        public void AddBetResult(BetResultData betResultData)
        {
            _statisticsData.AddBetResult(betResultData);
            _statisticsUIController.UpdateData(_statisticsData);
        }
        
        private void Save()
        {
            _dataHandler.SaveData(_statisticsData);
        }
    }
}