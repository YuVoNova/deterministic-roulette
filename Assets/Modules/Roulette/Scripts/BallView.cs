using System;
using System.Collections;
using Context.Interfaces;
using UnityEngine;

namespace Roulette
{
    public interface IBallView : IDisposableObject
    {
        event Action OnBallStopped;

        void Init(Transform rotatingWheel, Vector3 ballSpinPosition);
        void Standby();
        void StartBallSpin(Transform targetPocketTransform);
    }

    public class BallView : MonoBehaviour, IBallView
    {
        private const float BOUNCE_FORCE = 0.5f;
        private const float SPIN_SPEED = 5f;
        private const float SPIN_DURATION = 3f;
        private const float DROP_DURATION = 1.5f;
        private const float TRACK_RADIUS = 5.6f;
        private const float DROP_RADIUS = 3.075f;
        private const float MIN_SPIN_SPEED = 1f;
        private const float POCKET_DISTANCE_THRESHOLD = 3.75f;
        private const float DROP_HEIGHT_OFFSET = 0.25f;

        public event Action OnBallStopped;

        private Transform _initialParent;
        private Transform _wheel;
        private Transform _targetPocket;

        private Vector3 _ballSpinPosition;
        private float _angle;
        private float _currentSpinSpeed = SPIN_SPEED;

        private void Awake()
        {
            _initialParent = transform.parent;
        }

        public void Init(Transform rotatingWheel, Vector3 ballSpinPosition)
        {
            _wheel = rotatingWheel;
            _ballSpinPosition = ballSpinPosition;
        }

        public void Dispose()
        {
            if (this == null)
                return;
            
            StopAllCoroutines();
            Destroy(gameObject);
        }

        public void Standby()
        {
        }

        public void StartBallSpin(Transform targetPocketTransform)
        {
            _targetPocket = targetPocketTransform;
            transform.parent = _initialParent;
            transform.position = _ballSpinPosition;
            _angle = 0f;

            StartCoroutine(SpinBallRoutine());
        }

        private IEnumerator SpinBallRoutine()
        {
            _currentSpinSpeed = SPIN_SPEED;
            float elapsedTime = 0f;

            while (elapsedTime < SPIN_DURATION)
            {
                _angle -= _currentSpinSpeed * Time.deltaTime * 100f;
                float radianAngle = _angle * Mathf.Deg2Rad;
                transform.position = new Vector3(
                    _wheel.position.x + TRACK_RADIUS * Mathf.Cos(radianAngle),
                    transform.position.y,
                    _wheel.position.z + TRACK_RADIUS * Mathf.Sin(radianAngle)
                );
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return StartCoroutine(TransitionRoutine());
            yield return StartCoroutine(DropBallRoutine());
        }

        private IEnumerator TransitionRoutine()
        {
            _currentSpinSpeed = SPIN_SPEED;
            while (_currentSpinSpeed > MIN_SPIN_SPEED)
            {
                _currentSpinSpeed = Mathf.Max(MIN_SPIN_SPEED, _currentSpinSpeed - ((SPIN_SPEED - MIN_SPIN_SPEED) * Time.deltaTime / 1f));
                _angle -= _currentSpinSpeed * Time.deltaTime * 100f;
                float radianAngle = _angle * Mathf.Deg2Rad;
                transform.position = new Vector3(
                    _wheel.position.x + TRACK_RADIUS * Mathf.Cos(radianAngle),
                    transform.position.y,
                    _wheel.position.z + TRACK_RADIUS * Mathf.Sin(radianAngle)
                );
                yield return null;
            }

            while (Vector3.Distance(transform.position, _targetPocket.position) > POCKET_DISTANCE_THRESHOLD)
            {
                _angle -= _currentSpinSpeed * Time.deltaTime * 100f;
                float radianAngle = _angle * Mathf.Deg2Rad;
                transform.position = new Vector3(
                    _wheel.position.x + TRACK_RADIUS * Mathf.Cos(radianAngle),
                    transform.position.y,
                    _wheel.position.z + TRACK_RADIUS * Mathf.Sin(radianAngle)
                );
                yield return null;
            }
        }

        private IEnumerator DropBallRoutine()
        {
            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;
            Vector3 dropDirection = (startPosition - _wheel.position).normalized;
            Vector3 finalPosition = _wheel.position + dropDirection * DROP_RADIUS;
            finalPosition.y -= DROP_HEIGHT_OFFSET;

            float initialHeight = startPosition.y;
            float targetHeight = finalPosition.y;

            while (elapsedTime < DROP_DURATION)
            {
                float progress = elapsedTime / DROP_DURATION;
                float bounceOffset = BOUNCE_FORCE * Mathf.Sin(4 * Mathf.PI * progress) * Mathf.Exp(-3 * progress);
                Vector3 newPosition = Vector3.Lerp(startPosition, finalPosition, progress);
                newPosition.y = Mathf.Lerp(initialHeight, targetHeight, progress) + bounceOffset;
                transform.position = newPosition;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = finalPosition;
            transform.parent = _targetPocket;
            OnBallStopped?.Invoke();
        }
    }
}