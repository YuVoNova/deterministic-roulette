using UnityEngine;

namespace Betting.Data
{
    [CreateAssetMenu(fileName = "ChipSO", menuName = "Betting/ChipSO")]
    public class ChipSO : ScriptableObject
    {
        public int ChipValue;
        public Material ChipMaterial;
    }
}