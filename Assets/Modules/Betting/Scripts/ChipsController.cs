using UnityEngine;
using Betting.Data;
using Context.Interfaces;

namespace Betting
{
    public class ChipsController : IDisposableObject
    {
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

        private void ChipSelected(ChipSO chipSO)
        {
            _selectedChip = chipSO;
        }
    }
}