using System;
using UnityEngine;
using Utils;

namespace Roulette
{
    public interface IBallView
    {
        event Action<int> OnBallStopped;
        
        Rigidbody Rigidbody { get; }
        Collider Collider { get; }

        void Init(Vector3 ballSpinPosition);
        void Dispose();
        void Standby();
        void Spin();
    }

    public class BallView : MonoBehaviour, IBallView
    {
        public event Action<int> OnBallStopped;
        
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Collider collider;

        private Vector3 _initialPosition;
        private Vector3 _ballSpinPosition;
        private int _latestInteractedPocket = -1;
        private bool _isSpinning = false;

        public Rigidbody Rigidbody => rigidbody;
        public Collider Collider => collider;

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        private void Update()
        {
            if (_isSpinning)
            {
                if (rigidbody.velocity.magnitude < 0.1f && rigidbody.angularVelocity.magnitude < 0.1f)
                {
                    _isSpinning = false;
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;

                    if (_latestInteractedPocket != -1)
                    {
                        OnBallStopped?.Invoke(_latestInteractedPocket);
                    }
                    else
                    {
                        // ERROR -> Ball stopped at a non-pocket position.
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            int pocketNumber = GetPocketNumber(other.transform.tag, other.transform.name);
            if (pocketNumber != -1)
                _latestInteractedPocket = pocketNumber;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_latestInteractedPocket == -1)
                return;
            
            int pocketNumber = GetPocketNumber(other.transform.tag, other.transform.name);
            if (pocketNumber == _latestInteractedPocket)
                _latestInteractedPocket = -1;
        }

        public void Init(Vector3 ballSpinPosition)
        {
            _ballSpinPosition = ballSpinPosition;
        }

        public void Dispose()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        public void Standby()
        {
            
        }

        public void Spin()
        {
            
        }

        private static int GetPocketNumber(string tag, string name)
        {
            if (!string.Equals(tag, Const.POCKET_TAG))
                return -1;

            string[] pocketName = name.Split('_');
            if (pocketName.Length < 2)
                return -1;

            if (int.TryParse(pocketName[1], out int pocketNumber))
                return pocketNumber;

            return -1;
        }
    }
}