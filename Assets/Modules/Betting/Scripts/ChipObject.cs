using Betting.Data;
using UnityEngine;

namespace Betting
{
    public class ChipObject : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;
        
        public void SetChip(ChipSO chipSO)
        {
            renderer.material = chipSO.ChipMaterial;
        }
        
        public void RotateChip()
        {
            transform.Rotate(Vector3.up, Random.Range(0, 360));
        }
    }
}