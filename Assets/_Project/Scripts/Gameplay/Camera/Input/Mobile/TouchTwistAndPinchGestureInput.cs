using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TouchTwistAndPinchGestureInput : ITickable, IRotatorInputProvider, IZoomInputProvider
{
    private const float MINIMAL_VECTOR_LENGTH = 0.0001f;
    private const float FLOATING_EPSILON = 1e-3f;
    private const float DEFAULT_ROTATE_LOCK_DEGREES = 1.2f;
    private const float DEFAULT_PINCH_LOCK_PIXELS = 12.0f;
    private const float DEFAULT_ROTATE_DEADZONE_DEGREES = 0.15f;
    private const float DEFAULT_PINCH_DEADZONE_PIXELS = 1.0f;
    private const float DEFAULT_UNLOCK_AFTER_IDLE_SECONDS = 0.15f;
    private const float DEFAULT_PINCH_ANGLE_GUARD_DEGREES = 1.2f;
    private const float DEFAULT_PINCH_DOMINANCE_RATIO = 2.0f;
    private const float DEFAULT_MINIMUM_SCALE_DELTA_FOR_LOCK = 0.01f;
    
    public event Action<float> RotatorInputed;
    public event Action<float> ZoomInputed;

    private readonly float rotateSensitivity;
    private readonly float pinchSensitivity;
    private readonly float rotateLockDegrees;
    private readonly float pinchLockPixels;
    private readonly float rotateDeadzoneDegrees;
    private readonly float pinchDeadzonePixels;
    private readonly float unlockAfterIdleSeconds;
    private readonly float pinchAngleGuardDegrees;
    private readonly float pinchDominanceRatio;
    private readonly float minimumScaleDeltaForLock;
    private readonly bool ignoreWhenOverUI;
    
    private GestureModeType _gestureMode = GestureModeType.None;

    private Vector2? _previousPositionFirstFinger;
    private Vector2? _previousPositionSecondFinger;

    private int? _firstFingerId;
    private int? _secondFingerId;

    private float _cumulativeAngleAbsolute;
    private float _cumulativePinchAbsolute;
    private float _lastActiveUnscaledTime;

    [Inject]
    public TouchTwistAndPinchGestureInput(float rotateSensitivity, float pinchSensitivity, bool ignoreWhenOverUI = true, float rotateLockDegrees = DEFAULT_ROTATE_LOCK_DEGREES,
        float pinchLockPixels = DEFAULT_PINCH_LOCK_PIXELS, float rotateDeadzoneDegrees = DEFAULT_ROTATE_DEADZONE_DEGREES, float pinchDeadzonePixels = DEFAULT_PINCH_DEADZONE_PIXELS,
        float unlockAfterIdleSeconds = DEFAULT_UNLOCK_AFTER_IDLE_SECONDS, float pinchAngleGuardDegrees = DEFAULT_PINCH_ANGLE_GUARD_DEGREES,
        float pinchDominanceRatio = DEFAULT_PINCH_DOMINANCE_RATIO, float minimumScaleDeltaForLock = DEFAULT_MINIMUM_SCALE_DELTA_FOR_LOCK)
    {
        this.rotateSensitivity = rotateSensitivity;
        this.pinchSensitivity = pinchSensitivity;
        this.ignoreWhenOverUI = ignoreWhenOverUI;

        this.rotateLockDegrees = Mathf.Max(0.01f, rotateLockDegrees);
        this.pinchLockPixels = Mathf.Max(0.01f, pinchLockPixels);
        this.rotateDeadzoneDegrees = Mathf.Max(0f, rotateDeadzoneDegrees);
        this.pinchDeadzonePixels = Mathf.Max(0f, pinchDeadzonePixels);
        this.unlockAfterIdleSeconds = Mathf.Max(0.01f, unlockAfterIdleSeconds);
        this.pinchAngleGuardDegrees = Mathf.Max(0f, pinchAngleGuardDegrees);

        this.pinchDominanceRatio = Mathf.Max(1.0f, pinchDominanceRatio);
        this.minimumScaleDeltaForLock = Mathf.Max(0f, minimumScaleDeltaForLock);
    }
    
    void ITickable.Tick()
    {
        if (Input.touchCount < 2)
        {
            ResetGesture();
            
            return;
        }

        Touch firstTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        if (ignoreWhenOverUI && EventSystem.current != null)
        {
            if (EventSystem.current.IsPointerOverGameObject(firstTouch.fingerId) || EventSystem.current.IsPointerOverGameObject(secondTouch.fingerId))
            {
                ResetGesture();
                
                return;
            }
        }

        bool isNewFingerPair = _firstFingerId != firstTouch.fingerId || _secondFingerId != secondTouch.fingerId;
        
        if (isNewFingerPair)
        {
            _firstFingerId = firstTouch.fingerId;
            _secondFingerId = secondTouch.fingerId;
            _previousPositionFirstFinger = firstTouch.position;
            _previousPositionSecondFinger = secondTouch.position;
            _cumulativeAngleAbsolute = 0f;
            _cumulativePinchAbsolute = 0f;
            _gestureMode = GestureModeType.None;
            _lastActiveUnscaledTime = Time.unscaledTime;
            
            return;
        }

        Vector2 currentPositionFirstFinger = firstTouch.position;
        Vector2 currentPositionSecondFinger = secondTouch.position;

        if (!_previousPositionFirstFinger.HasValue || !_previousPositionSecondFinger.HasValue)
        {
            _previousPositionFirstFinger = currentPositionFirstFinger;
            _previousPositionSecondFinger = currentPositionSecondFinger;
            _lastActiveUnscaledTime = Time.unscaledTime;
            
            return;
        }

        Vector2 previousCenter = 0.5f * (_previousPositionFirstFinger.Value + _previousPositionSecondFinger.Value);
        Vector2 currentCenter = 0.5f * (currentPositionFirstFinger + currentPositionSecondFinger);
        Vector2 previousVectorFirst = _previousPositionFirstFinger.Value - previousCenter;
        Vector2 previousVectorSecond = _previousPositionSecondFinger.Value - previousCenter;
        Vector2 currentVectorFirst = currentPositionFirstFinger - currentCenter;
        Vector2 currentVectorSecond = currentPositionSecondFinger - currentCenter;

        float deltaAngleDegrees = 0f;
        int contributorsCount = 0;

        if (previousVectorFirst.sqrMagnitude > MINIMAL_VECTOR_LENGTH && currentVectorFirst.sqrMagnitude > MINIMAL_VECTOR_LENGTH)
        {
            float previousAngleFirst = Mathf.Atan2(previousVectorFirst.y, previousVectorFirst.x) * Mathf.Rad2Deg;
            float currentAngleFirst = Mathf.Atan2(currentVectorFirst.y, currentVectorFirst.x) * Mathf.Rad2Deg;
            
            deltaAngleDegrees += Mathf.DeltaAngle(previousAngleFirst, currentAngleFirst);
            contributorsCount++;
        }

        if (previousVectorSecond.sqrMagnitude > MINIMAL_VECTOR_LENGTH && currentVectorSecond.sqrMagnitude > MINIMAL_VECTOR_LENGTH)
        {
            float previousAngleSecond = Mathf.Atan2(previousVectorSecond.y, previousVectorSecond.x) * Mathf.Rad2Deg;
            float currentAngleSecond = Mathf.Atan2(currentVectorSecond.y, currentVectorSecond.x) * Mathf.Rad2Deg;
            
            deltaAngleDegrees += Mathf.DeltaAngle(previousAngleSecond, currentAngleSecond);
            contributorsCount++;
        }

        if (contributorsCount > 0)
            deltaAngleDegrees /= contributorsCount;

        float previousDistance = (_previousPositionSecondFinger.Value - _previousPositionFirstFinger.Value).magnitude;
        float currentDistance = (currentPositionSecondFinger - currentPositionFirstFinger).magnitude;
        float deltaDistancePixels = currentDistance - previousDistance;
        bool hasMeaningfulRotation = Mathf.Abs(deltaAngleDegrees) > rotateDeadzoneDegrees;
        bool hasMeaningfulPinch = Mathf.Abs(deltaDistancePixels) > pinchDeadzonePixels;
        
        if (hasMeaningfulRotation || hasMeaningfulPinch)
            _lastActiveUnscaledTime = Time.unscaledTime;

        if (_gestureMode == GestureModeType.None)
        {
            _cumulativeAngleAbsolute += Mathf.Abs(deltaAngleDegrees);

            bool angleSmallEnoughForPinch = Mathf.Abs(deltaAngleDegrees) <= pinchAngleGuardDegrees;

            float scaleDelta = (previousDistance > FLOATING_EPSILON) ? Mathf.Abs(deltaDistancePixels) / previousDistance : 0f;
            bool scaleIsLargeEnough = scaleDelta >= minimumScaleDeltaForLock;

            float expectedBreathingFromTwist = previousDistance * Mathf.Deg2Rad * Mathf.Abs(deltaAngleDegrees);
            bool radialChangeDominates = Mathf.Abs(deltaDistancePixels) >= pinchDominanceRatio * (expectedBreathingFromTwist + FLOATING_EPSILON);

            if (angleSmallEnoughForPinch && scaleIsLargeEnough && radialChangeDominates)
                _cumulativePinchAbsolute += Mathf.Abs(deltaDistancePixels);

            bool shouldLockRotate = _cumulativeAngleAbsolute >= rotateLockDegrees;
            bool shouldLockPinch = _cumulativePinchAbsolute >= pinchLockPixels;

            if (shouldLockRotate || shouldLockPinch)
                _gestureMode = shouldLockRotate ? GestureModeType.Rotate : GestureModeType.Pinch;
        }
        else
        {
            if (_gestureMode == GestureModeType.Rotate)
            {
                if (hasMeaningfulRotation)
                    RotatorInputed?.Invoke(deltaAngleDegrees * rotateSensitivity);
            }
            else
            {
                if (hasMeaningfulPinch)
                    ZoomInputed?.Invoke(deltaDistancePixels * pinchSensitivity);
            }

            bool isIdleTooLong = Time.unscaledTime - _lastActiveUnscaledTime > unlockAfterIdleSeconds;
            
            if (isIdleTooLong)
            {
                _gestureMode = GestureModeType.None;
                _cumulativeAngleAbsolute = 0f;
                _cumulativePinchAbsolute = 0f;
            }
        }

        _previousPositionFirstFinger = currentPositionFirstFinger;
        _previousPositionSecondFinger = currentPositionSecondFinger;
    }

    private void ResetGesture()
    {
        _gestureMode = GestureModeType.None;
        _firstFingerId = null;
        _secondFingerId = null;
        _previousPositionFirstFinger = null;
        _previousPositionSecondFinger = null;
        _cumulativeAngleAbsolute = 0f;
        _cumulativePinchAbsolute = 0f;
        _lastActiveUnscaledTime = 0f;
    }
}