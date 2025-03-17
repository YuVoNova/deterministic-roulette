using System;
using Context.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Betting
{
    public interface IBettingUIView : IDisposableObject, IInitializableObject
    {
        event Action OnSpinButtonClicked;
        event Action OnClearBetsButtonClicked;
        
        void SetMoneyText(int amount);
        void SetActiveBetsText(int amount);
    }
    
    public class BettingUIView : MonoBehaviour, IBettingUIView
    {
        public event Action OnSpinButtonClicked;
        public event Action OnClearBetsButtonClicked;
        
        [SerializeField] private Button spinButton;
        [SerializeField] private Button clearBetsButton;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text activeBetsText;
        
        public void Init()
        {
            spinButton.onClick.AddListener(SpinButtonClicked);
            clearBetsButton.onClick.AddListener(ClearBetsButtonClicked);
        }
        
        public void Dispose()
        {
            spinButton.onClick.RemoveListener(SpinButtonClicked);
            clearBetsButton.onClick.RemoveListener(ClearBetsButtonClicked);
            
            if (gameObject != null)
                Destroy(gameObject);
        }

        public void SetMoneyText(int amount)
        {
            string text = amount.ToString("N0");
            moneyText.text = "$" + text;
        }

        public void SetActiveBetsText(int amount)
        {
            string text = amount.ToString("N0");
            activeBetsText.text = "Active Bets: $" + text;
        }

        private void SpinButtonClicked()
        {
            OnSpinButtonClicked?.Invoke();
        }

        private void ClearBetsButtonClicked()
        {
            OnClearBetsButtonClicked?.Invoke();
        }
    }
}