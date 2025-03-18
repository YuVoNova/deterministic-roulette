using System;
using Betting.Data;
using UnityEngine;

namespace Player.Data
{
    [Serializable]
    public class PlayerData
    {
        public const int DEFAULT_MONEY = 10000;
        public const int DEFAULT_SELECTED_CHIP_ID = 0;

        public int Money => _money;
        public int SelectedChipId => _selectedChipId;
        public BetSlotData[] ActiveBets => _activeBets;

        [SerializeField] private int _money;
        [SerializeField] private int _selectedChipId;
        [SerializeField] private BetSlotData[] _activeBets;

        public PlayerData()
        {
            _money = DEFAULT_MONEY;
            _selectedChipId = DEFAULT_SELECTED_CHIP_ID;
            _activeBets = null;
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }

        public static PlayerData FromJson(string json)
        {
            return JsonUtility.FromJson<PlayerData>(json);
        }

        public void AddMoney(int amount)
        {
            _money += amount;
        }

        public bool WithdrawMoney(int amount)
        {
            if (_money < amount)
                return false;

            _money -= amount;
            return true;
        }
        
        public void SetSelectedChipId(int selectedChipId)
        {
            _selectedChipId = selectedChipId;
        }
        
        public void SetActiveBets(BetSlotData[] activeBets)
        {
            _activeBets = activeBets;
        }
    }
}