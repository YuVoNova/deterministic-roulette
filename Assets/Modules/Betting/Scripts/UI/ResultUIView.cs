using System;
using Betting.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Betting
{
    public interface IResultUIView
    {
        event Action OnResultFinished;
        
        void ShowResult(BetResultData betResultData);
        void ToggleUI(bool isOn);
    }
    
    public class ResultUIView : MonoBehaviour, IResultUIView
    {
        private const float RESULT_DISPLAY_TIME = 3f;
        
        public event Action OnResultFinished;
        
        [SerializeField] private Image resultNumberImage;
        [SerializeField] private TMP_Text resultNumberText;
        [SerializeField] private TMP_Text payoutText;

        private float _timer = 0f;
        
        private void Update()
        {
            if (_timer <= 0f)
                return;

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
                OnResultFinished?.Invoke();
        }

        public void ShowResult(BetResultData betResultData)
        {
            resultNumberText.text = betResultData.ResultNumber.ToString();
            resultNumberImage.color = betResultData.ResultColor switch
            {
                SlotColors.Green => Const.GREEN_COLOR,
                SlotColors.Red => Const.RED_COLOR,
                SlotColors.Black => Const.BLACK_COLOR,
                _ => resultNumberImage.color
            };
            payoutText.text = "Payout: " + betResultData.WinAmount;
            gameObject.SetActive(true);
            
            _timer = RESULT_DISPLAY_TIME;
        }

        public void ToggleUI(bool isOn)
        {
            gameObject.SetActive(false);
        }
    }
}