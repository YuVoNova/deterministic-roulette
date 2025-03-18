using System;
using Context.Interfaces;
using Betting.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Statistics
{
    public interface IStatisticsUIView : IDisposableObject, IInitializableObject
    {
        void UpdatePlayHistory(BetResultData[] playHistory);
        void UpdateTotalBalance(int totalBalance);
    }

    public class StatisticsUIView : MonoBehaviour, IStatisticsUIView
    {
        [SerializeField] private Button playHistoryButton;
        [SerializeField] private GameObject playHistoryPanel;
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text totalBalanceText;
        [SerializeField] private GameObject historyEntryPrefab;
        [SerializeField] private Transform historyEntriesParent;

        public void Init()
        {
            playHistoryButton.onClick.AddListener(() => TogglePlayHistoryPanel(true));
            closeButton.onClick.AddListener(() => TogglePlayHistoryPanel(false));
            
            TogglePlayHistoryPanel(false);
        }

        public void TogglePlayHistoryButton(bool isOn)
        {
            playHistoryButton.gameObject.SetActive(isOn);
        }

        public void TogglePlayHistoryPanel(bool isOn)
        {
            playHistoryPanel.SetActive(!playHistoryPanel.activeSelf);
        }

        public void UpdatePlayHistory(BetResultData[] playHistory)
        {
            if (playHistory == null)
                return;
            
            foreach (Transform child in historyEntriesParent)
            {
                Destroy(child.gameObject);
            }

            foreach (BetResultData playHistoryEntry in playHistory)
            {
                GameObject historyEntryObject = Instantiate(historyEntryPrefab, historyEntriesParent);
                historyEntryObject.GetComponent<HistoryEntryObject>().SetEntryData(playHistoryEntry);
            }
        }

        public void UpdateTotalBalance(int totalBalance)
        {
            totalBalanceText.text = "Total Balance: " + (totalBalance < 0 ? "-" : "+") + " $" + totalBalance;
        }

        public void Dispose()
        {
            if (this == null)
                return;

            playHistoryButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
        }
    }
}