using UnityEngine;
using Context.Interfaces;

namespace Statistics
{
    public class StatisticsUIController : IDisposableObject
    {
        private readonly IStatisticsUIView _view;

        private StatisticsData _data;

        public StatisticsUIController(StatisticsData data)
        {
            _data = data;
            _view = GameObject.FindObjectOfType<StatisticsUIView>();
            _view.Init();
            UpdateUI();
        }

        public void UpdateData(StatisticsData data)
        {
            _data = data;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _view.UpdatePlayHistory(_data.PlayHistory);
            _view.UpdateTotalBalance(_data.TotalBalance);
        }

        public void Dispose()
        {
            _view?.Dispose();
        }
    }
}