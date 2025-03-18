using System;
using Betting.Data;
using UnityEngine;

namespace Player.Data
{
    [Serializable]
    public class PlayerData
    {
        public const int DEFAULT_MONEY = 10000;

        public int Money => _money;

        [SerializeField] private int _money;
        [SerializeField] private BetSlotData[] _activeBets;

        public PlayerData()
        {
            _money = DEFAULT_MONEY;
        }
        
        public PlayerData(int money)
        {
            _money = money;
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
    }
}