using System;
using Betting.Data;
using Context.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Betting
{
    public interface IBettingView : IDisposableObject, IInitializableObject
    {
        event Action<SlotObject> OnSlotClicked;

        void ClearBetDisplay();
    }

    public class BettingView : MonoBehaviour, IBettingView
    {
        public event Action<SlotObject> OnSlotClicked;

        [SerializeField] private SlotObject[] slotObjects;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Button spinBallButton;

        public void Init()
        {
            foreach (SlotObject slotObject in slotObjects)
            {
                slotObject.OnSlotClicked += SlotClicked;
                slotObject.OnHoverEnter += SlotHoverEnter;
                slotObject.OnHoverExit += SlotHoverExit;
            }
        }

        public void Dispose()
        {
            foreach (SlotObject slotObject in slotObjects)
            {
                if (slotObject == null)
                    continue;

                slotObject.OnSlotClicked -= SlotClicked;
                slotObject.OnHoverEnter -= SlotHoverEnter;
                slotObject.OnHoverExit -= SlotHoverExit;
            }
        }

        public void ClearBetDisplay()
        {
            foreach (SlotObject slotObject in slotObjects)
            {
                slotObject.ResetSlot();
            }
        }

        private void SlotClicked(SlotObject slotObject)
        {
            OnSlotClicked?.Invoke(slotObject);
        }

        private void SlotHoverEnter(SlotObject slotObject)
        {
            slotObject.HighlightSlot(highlightMaterial);
        }

        private void SlotHoverExit(SlotObject slotObject)
        {
            slotObject.HighlightSlot(defaultMaterial);
        }
    }
}