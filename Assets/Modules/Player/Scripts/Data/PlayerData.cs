using System;
using UnityEngine;

namespace Player.Data
{
    [Serializable]
    public class PlayerData
    {
        public const int DEFAULT_MONEY = 1000;

        public int Money => _money;

        [SerializeField] private int _money;

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