using System;
using Betting.Data;
using Context.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Betting
{
    public class ChipsStackObject : MonoBehaviour, IDisposableObject, IPointerDownHandler
    {
        public event Action<ChipSO, Transform> OnChipSelected;

        [SerializeField] private ChipSO chipSO;
        [SerializeField] private ChipObject[] chipObjects;
        [SerializeField] private TMP_Text chipValueText;
        [SerializeField] private Transform selectedObjectParent;

        private void Awake()
        {
            foreach (ChipObject chipObject in chipObjects)
            {
                chipObject.SetChip(chipSO);
                chipObject.RotateChip();
            }

            chipValueText.text = "$" + chipSO.ChipValue;
        }

        public void Dispose()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SelectChip();
        }

        public void SelectChip()
        {
            OnChipSelected?.Invoke(chipSO, selectedObjectParent);
        }
    }
}