using System;
using UnityEngine;
using Betting.Data;
using Context.Interfaces;

namespace Betting
{
    public class ChipsController : IDisposableObject
    {
        public event Action<ChipSO> OnChipSelected;
        
        private readonly IChipsView _view;

        private ChipSO _selectedChip;

        public ChipsController()
        {
            _view = GameObject.FindObjectOfType<ChipsView>();
            _view.Init();
            _view.OnChipSelected += ChipSelected;
        }

        public void Dispose()
        {
            if (_view == null)
                return;

            _view.OnChipSelected -= ChipSelected;
            _view.Dispose();
        }
        
        public void InitialSelectedChip(int initialSelectedChipId)
        {
            _view.InitialSelectedChip(initialSelectedChipId);
        }

        private void ChipSelected(ChipSO chipSO)
        {
            _selectedChip = chipSO;
            OnChipSelected?.Invoke(_selectedChip);
        }
    }
}