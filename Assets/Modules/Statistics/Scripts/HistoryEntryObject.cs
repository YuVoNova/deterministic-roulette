using System;
using Betting.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Statistics
{
    public class HistoryEntryObject : MonoBehaviour
    {
        [SerializeField] private Image resultColorImage;
        [SerializeField] private TMP_Text resultNumberText;
        [SerializeField] private TMP_Text[] betTexts;
        [SerializeField] private TMP_Text balanceText;
        
        [SerializeField] private Color winColor;
        [SerializeField] private Color loseColor;
        
        public void SetEntryData(BetResultData betResultData)
        {
            resultColorImage.color = betResultData.ResultColor switch
            {
                SlotColors.Green => Const.GREEN_COLOR,
                SlotColors.Red => Const.RED_COLOR,
                SlotColors.Black => Const.BLACK_COLOR,
                _ => Const.BLACK_COLOR
            };
            resultNumberText.text = betResultData.ResultNumber.ToString();
            
            int count = Math.Min(betTexts.Length, betResultData.BetSlots.Length);
            for (int i = 0; i < count; i++)
            {
                betTexts[i].text = betResultData.BetSlots[i].ToString();
                betTexts[i].color = betResultData.BetSlots[i].BetAmount > 0 ? winColor : loseColor;
            }
            
            balanceText.text = "Balance: " + (betResultData.BalanceAmount < 0 ? "-" : "+") + "$" + betResultData.BalanceAmount;
            balanceText.color = betResultData.BalanceAmount < 0 ? loseColor : winColor;
        }
    }
}