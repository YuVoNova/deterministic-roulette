using System;
using Betting.Data;
using Context.Interfaces;
using UnityEngine;

namespace Betting
{
    public interface IChipsView : IDisposableObject, IInitializableObject
    {
        event Action<ChipSO> OnChipSelected;
    }

    public class ChipsView : MonoBehaviour, IChipsView
    {
        public event Action<ChipSO> OnChipSelected;

        [SerializeField] private ChipsStackObject[] chipsStacks;
        [SerializeField] private Transform selectedObject;

        private void Awake()
        {
            foreach (ChipsStackObject chipsStack in chipsStacks)
            {
                chipsStack.OnChipSelected += ChipSelected;
            }
        }

        public void Init()
        {
            chipsStacks[0].SelectChip();
        }

        public void Dispose()
        {
            foreach (ChipsStackObject chipsStack in chipsStacks)
            {
                chipsStack.OnChipSelected -= ChipSelected;
            }
        }

        private void ChipSelected(ChipSO chipSO, Transform selectedObjectParent)
        {
            selectedObject.SetParent(selectedObjectParent);
            selectedObject.localPosition = Vector3.zero;
            selectedObject.localRotation = Quaternion.identity;
            OnChipSelected?.Invoke(chipSO);
        }
    }
}