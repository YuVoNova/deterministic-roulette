using Betting.Data;
using Context.Interfaces;
using UnityEngine;

namespace Betting
{
    public class ChipObject : MonoBehaviour, IDisposableObject
    {
        [SerializeField] private Renderer renderer;

        public void Dispose()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
        
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