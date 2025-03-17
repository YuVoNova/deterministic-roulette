using System;
using Betting.Data;
using Context.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Betting
{
    public interface IChipsView : IDisposableObject
    {
        event Action<ChipSO> OnChipSelected;
    }
    
    public class ChipsView : MonoBehaviour, IChipsView
    {
        public event Action<ChipSO> OnChipSelected;
        
        [SerializeField] private ChipsStackObject[] chipsStacks;

        private void Awake()
        {
            foreach (ChipsStackObject chipsStack in chipsStacks)
            {
                chipsStack.OnChipSelected += ChipSelected;
            }
        }
        
        public void Dispose()
        {
            foreach (ChipsStackObject chipsStack in chipsStacks)
            {
                chipsStack.OnChipSelected -= ChipSelected;
            }
        }
        
        private void ChipSelected(ChipSO chipSO)
        {
            OnChipSelected?.Invoke(chipSO);
        }
    }
}