using UnityEngine;
using UnityEngine.Serialization;

namespace Betting.Data
{
    [CreateAssetMenu(fileName = "ChipSO", menuName = "Betting/ChipSO")]
    public class ChipSO : ScriptableObject
    {
        public int ChipId;
        public int ChipValue;
        public Material ChipMaterial;
    }
}