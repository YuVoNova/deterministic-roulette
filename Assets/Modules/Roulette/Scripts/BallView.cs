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
        private const float SPIN_SPEED = 5f;
        private const float SPIN_DURATION = 3f;
        private const float DROP_DURATION = 1.5f;
        private const float TRACK_RADIUS = 5.6f;
        private const float DROP_RADIUS = 3.075f;
        private const float MIN_SPIN_SPEED = 0.8f;
        private const float MIN_POCKET_DISTANCE_THRESHOLD = 8.65f;
        private const float MAX_POCKET_DISTANCE_THRESHOLD = 8.75f;
        private const float DROP_HEIGHT_OFFSET = 0.75f;
        private const float FLICKER_DURATION = 1f;
        private const float INITIAL_FLICKER_Y_AMPLITUDE = 0.05f;
        private const float INITIAL_FLICKER_XZ_AMPLITUDE = 0.1f;
        private const float INITIAL_FLICKER_Y_FREQUENCY = 20f;
        private const float INITIAL_FLICKER_XZ_FREQUENCY = 40f;
        private const float TARGET_FLICKER_Y_FREQUENCY = 5f;
        private const float TARGET_FLICKER_XZ_FREQUENCY = 10f;

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
            // Phase A: Deceleration Phase
            _currentSpinSpeed = SPIN_SPEED;
            while (_currentSpinSpeed > MIN_SPIN_SPEED)
            {
                _currentSpinSpeed = Mathf.Max(MIN_SPIN_SPEED, _currentSpinSpeed - ((SPIN_SPEED - MIN_SPIN_SPEED) * Time.deltaTime / 2f));
                _angle -= _currentSpinSpeed * Time.deltaTime * 100f;
                float radianAngle = _angle * Mathf.Deg2Rad;
                transform.position = new Vector3(
                    _wheel.position.x + TRACK_RADIUS * Mathf.Cos(radianAngle),
                    transform.position.y,
                    _wheel.position.z + TRACK_RADIUS * Mathf.Sin(radianAngle)
                );
                yield return null;
            }

            // Phase B: Pocket Tracking Phase
            // Wait until the ball is within the distance threshold, and the target pocket is still ahead.
            float previousDistance = Vector3.Distance(transform.position, _targetPocket.position);
            while (true)
            {
                _angle -= _currentSpinSpeed * Time.deltaTime * 100f;
                float radianAngle = _angle * Mathf.Deg2Rad;
                Vector3 ballPos = new Vector3(
                    _wheel.position.x + TRACK_RADIUS * Mathf.Cos(radianAngle),
                    transform.position.y,
                    _wheel.position.z + TRACK_RADIUS * Mathf.Sin(radianAngle)
                );
                transform.position = ballPos;

                float currentDistance = Vector3.Distance(ballPos, _targetPocket.position);
                float distanceDelta = currentDistance - previousDistance;
                bool isApproaching = (distanceDelta < 0);

                if (currentDistance is <= MAX_POCKET_DISTANCE_THRESHOLD and >= MIN_POCKET_DISTANCE_THRESHOLD && isApproaching)
                    break;

                previousDistance = currentDistance;
                yield return null;
            }
        }

        private IEnumerator DropBallRoutine()
        {
            // Phase A: Drop Phase
            float elapsedTime = 0f;
            float initialHeight = transform.position.y;
            float targetHeight = initialHeight - DROP_HEIGHT_OFFSET;

            Vector3 moveDirection = Vector3.zero;
            while (elapsedTime < DROP_DURATION)
            {
                float progress = elapsedTime / DROP_DURATION;
                float easedProgress = Mathf.SmoothStep(0f, 1f, progress);
                float currentRadius = Mathf.Lerp(TRACK_RADIUS, DROP_RADIUS, easedProgress);

                _angle -= _currentSpinSpeed * Time.deltaTime * 100f;
                float radianAngle = _angle * Mathf.Deg2Rad;

                float newX = _wheel.position.x + currentRadius * Mathf.Cos(radianAngle);
                float newZ = _wheel.position.z + currentRadius * Mathf.Sin(radianAngle);
                float newY = Mathf.Lerp(initialHeight, targetHeight, easedProgress);

                moveDirection = (transform.position - _targetPocket.position).normalized;

                transform.position = new Vector3(newX, newY, newZ);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = new Vector3(
                _wheel.position.x + DROP_RADIUS * Mathf.Cos(_angle * Mathf.Deg2Rad),
                targetHeight,
                _wheel.position.z + DROP_RADIUS * Mathf.Sin(_angle * Mathf.Deg2Rad)
            );
            transform.parent = _targetPocket;
            transform.localRotation = Quaternion.identity;

            // Phase B: Flicker (Oscillation) Phase
            elapsedTime = 0f;
            Vector3 originalLocalPos = transform.localPosition;
            Vector3 localDropDirection = _targetPocket.InverseTransformDirection(moveDirection).normalized;
            Vector3 lateralDirection = Vector3.Cross(Vector3.up, localDropDirection).normalized;

            while (elapsedTime < FLICKER_DURATION)
            {
                float progress = elapsedTime / FLICKER_DURATION;
                float dampingFactor = 1f - progress;
                float currentYFrequency = Mathf.Lerp(INITIAL_FLICKER_Y_FREQUENCY, TARGET_FLICKER_Y_FREQUENCY, progress);
                float currentXZFrequency = Mathf.Lerp(INITIAL_FLICKER_XZ_FREQUENCY, TARGET_FLICKER_XZ_FREQUENCY, progress);

                float offsetY = INITIAL_FLICKER_Y_AMPLITUDE * dampingFactor * Mathf.Sin(elapsedTime * currentYFrequency);
                float offsetXZ = INITIAL_FLICKER_XZ_AMPLITUDE * dampingFactor * Mathf.Sin(elapsedTime * currentXZFrequency);

                Vector3 offset = lateralDirection * offsetXZ + new Vector3(0, offsetY, 0);
                transform.localPosition = originalLocalPos + offset;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalLocalPos;
            OnBallStopped?.Invoke();
        }
    }
}