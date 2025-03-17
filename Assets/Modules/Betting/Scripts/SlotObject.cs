using System;
using Betting.Data;
using Context.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace Betting
{
    public class SlotObject : MonoBehaviour, IDisposableObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public event Action<BetType, int> OnSlotClicked;
        public event Action<SlotObject> OnHoverEnter;
        public event Action<SlotObject> OnHoverExit;

        [SerializeField] private Renderer highlightRenderer;
        [SerializeField] private TMP_Text betAmountText;

        private GameObject _betAmountTextObject;

        [HideInInspector] public BetType betType;
        [HideInInspector] public int slotId;

        private void Awake()
        {
            _betAmountTextObject = betAmountText.gameObject;

            string[] slotName = transform.name.Split('_');
            betType = (BetType)Enum.Parse(typeof(BetType), slotName[0]);
            slotId = int.Parse(slotName[1]);

            ResetSlot();
        }

        public void Dispose()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        public void AddBet(int amount)
        {
            betAmountText.text = amount.ToString();

            if (!_betAmountTextObject.activeSelf)
                _betAmountTextObject.SetActive(true);
        }

        public void ResetSlot()
        {
            betAmountText.text = "0";
            _betAmountTextObject.SetActive(false);
        }

        public void HighlightSlot(Material material)
        {
            highlightRenderer.material = material;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverExit?.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSlotClicked?.Invoke(betType, slotId);
        }
    }
}