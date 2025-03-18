using Context;
using UnityEngine;

namespace Statistics
{
    public class StatisticsDataHandler
    {
        private const string FILE_NAME = "StatisticsData.json";
        
        private readonly IFileService _fileService;

        public StatisticsDataHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public StatisticsData LoadData()
        {
            string fileData = _fileService.Load(FILE_NAME);
            if (string.IsNullOrEmpty(fileData) || fileData.Trim() == "{}")
            {
                return new StatisticsData();
            }

            try
            {
                return StatisticsData.FromJson(fileData);
            }
            catch
            {
                return new StatisticsData();
            }
        }

        public void SaveData(StatisticsData statisticsData)
        {
            string json = statisticsData.ToString();
            _fileService.Save(FILE_NAME, json);
        }
    }
}