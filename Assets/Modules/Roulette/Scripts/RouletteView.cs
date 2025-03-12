using System;
using UnityEngine;

namespace Roulette
{
    public interface IRouletteView
    {
        Vector3 BallSpinPosition { get; }
        
        void Init();
        void Dispose();
    }

    public class RouletteView : MonoBehaviour, IRouletteView
    {
        [SerializeField] private GameObject[] pockets;
        [SerializeField] private Transform ballSpinPoint;

        public Vector3 BallSpinPosition => ballSpinPoint.position;

        public void Init()
        {
            
        }

        public void Dispose()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }
}