using UnityEngine;

public class WorldAutoRotator : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _axis = Vector3.up;
    [SerializeField] private float _speedDegreesPerSecond = 10f;

    public void SetSpeed(float speedDegPerSec) => _speedDegreesPerSecond = speedDegPerSec;

    private void LateUpdate()
    {
        if (_target == null || _speedDegreesPerSecond == 0f)
            return;
        
        float delta = Time.unscaledDeltaTime * _speedDegreesPerSecond;
        _target.Rotate(_axis, delta, Space.World);
    }
}