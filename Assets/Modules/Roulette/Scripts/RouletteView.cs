using UnityEngine;

namespace Roulette
{
    public interface IRouletteView
    {
        Vector3 BallSpinPosition { get; }

        void Init();
        void Dispose();
        Transform GetPocket(int pocketValue);
        Transform GetRotatingWheel();
    }

    public class RouletteView : MonoBehaviour, IRouletteView
    {
        [SerializeField] private Transform[] pockets;
        [SerializeField] private Transform ballSpinPoint;
        [SerializeField] private Transform rotatingWheel;

        public Vector3 BallSpinPosition => ballSpinPoint.position;

        public void Init()
        {
        }

        public void Dispose()
        {
            if (this != null)
                return;

            if (gameObject != null)
                Destroy(gameObject);
        }

        public Transform GetPocket(int pocketValue)
        {
            return pockets[pocketValue];
        }

        public Transform GetRotatingWheel()
        {
            return rotatingWheel;
        }
    }
}