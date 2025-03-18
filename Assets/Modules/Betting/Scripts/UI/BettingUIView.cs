using System;
using Context.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Betting
{
    public interface IBettingUIView : IDisposableObject, IInitializableObject
    {
        event Action<int> OnSpinButtonClicked;
        event Action OnClearBetsButtonClicked;

        void SetMoneyText(int amount);
        void SetActiveBetsText(int amount);
        void ToggleButtons(bool isOn);
        void ToggleUI(bool isOn);
    }

    public class BettingUIView : MonoBehaviour, IBettingUIView
    {
        private const float MONEY_DURATION = 1f;

        public event Action<int> OnSpinButtonClicked;
        public event Action OnClearBetsButtonClicked;

        [SerializeField] private Button spinButton;
        [SerializeField] private Button clearBetsButton;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text activeBetsText;
        [SerializeField] private GameObject deterministicResultPanel;
        [SerializeField] private Toggle deterministicResultToggle;
        [SerializeField] private TMP_InputField resultInputField;

        private bool _isDeterministicResult = false;
        private float _moneyStep = 0;
        private int _targetMoney = 0;
        private float _currentMoney = 0f;

        private void Update()
        {
            if ((int)_currentMoney == _targetMoney)
                return;

            _currentMoney = Mathf.MoveTowards(_currentMoney, _targetMoney, _moneyStep * Time.deltaTime);
            ShowMoneyAmount((int)_currentMoney);
        }

        public void Init()
        {
            spinButton.onClick.AddListener(SpinButtonClicked);
            clearBetsButton.onClick.AddListener(ClearBetsButtonClicked);
            deterministicResultToggle.onValueChanged.AddListener(ToggleDeterministicResult);
        }

        public void Dispose()
        {
            spinButton.onClick.RemoveListener(SpinButtonClicked);
            clearBetsButton.onClick.RemoveListener(ClearBetsButtonClicked);
            deterministicResultToggle.onValueChanged.RemoveListener(ToggleDeterministicResult);

            if (this != null)
                Destroy(gameObject);
        }

        public void SetMoneyText(int amount)
        {
            _targetMoney = amount;
            _moneyStep = Mathf.Abs(_targetMoney - _currentMoney) / MONEY_DURATION;
        }

        public void SetActiveBetsText(int amount)
        {
            string text = amount.ToString("N0");
            activeBetsText.text = "Active Bets: $" + text;
        }

        public void ToggleButtons(bool isOn)
        {
            spinButton.gameObject.SetActive(isOn);
            clearBetsButton.gameObject.SetActive(isOn);
            deterministicResultPanel.SetActive(isOn);
        }
        
        public void ToggleUI(bool isOn)
        {
            gameObject.SetActive(isOn);
        }

        private void ShowMoneyAmount(int amount)
        {
            string text = amount.ToString("N0");
            moneyText.text = "$" + text;
        }

        private void SpinButtonClicked()
        {
            if (_isDeterministicResult && int.TryParse(resultInputField.text, out int result))
                OnSpinButtonClicked?.Invoke(result);
            else
                OnSpinButtonClicked?.Invoke(Const.DEFAULT_RESULT);
        }

        private void ClearBetsButtonClicked()
        {
            OnClearBetsButtonClicked?.Invoke();
        }

        private void ToggleDeterministicResult(bool isOn)
        {
            _isDeterministicResult = isOn;
            resultInputField.gameObject.SetActive(isOn);
        }
    }
}