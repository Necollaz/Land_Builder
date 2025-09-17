using System.Collections;
using UnityEngine;

public class HexTileView : MonoBehaviour
{
    [SerializeField] private Hexagon _hexagon;
    
    private Coroutine _moveRoutine;
    private Transform _cachedTransform;

    private void Awake()
    {
        if (_cachedTransform == null)
            _cachedTransform = transform;
    }
    
    public void ApplyData(HexTileData data)
    {
        if (data == null || data.SideTypes == null || data.SideTypes.Length != 6)
            return;

        if (_hexagon == null)
            _hexagon = GetComponentInChildren<Hexagon>(true);

        if (_hexagon != null)
            _hexagon.SetSideTypes(data.SideTypes);
    }
    
    public void SnapTo(Vector3 localPos, Vector3 localScale, Quaternion localRot)
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }
        
        if (_cachedTransform == null)
            _cachedTransform = transform;

        _cachedTransform.localPosition = localPos;
        _cachedTransform.localScale = localScale;
        _cachedTransform.localRotation = localRot;
    }

    public void AnimateTo(Vector3 localPos, Vector3 localScale, Quaternion localRot, float duration)
    {
        AnimateToDelayed(localPos, localScale, localRot, duration, 0f);
    }
    
    public void AnimateToDelayed(Vector3 localPos, Vector3 localScale, Quaternion localRot, float duration, float delay)
    {
        if (_cachedTransform == null)
            _cachedTransform = transform;

        if (duration <= 0f && delay <= 0f)
        {
            SnapTo(localPos, localScale, localRot);
            return;
        }

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);
        
        _moveRoutine = StartCoroutine(AnimateRoutine(localPos, localScale, localRot, duration, delay));
    }
    
    private void Reset()
    {
        if (_hexagon == null)
            _hexagon = GetComponentInChildren<Hexagon>(true);
    }
    
    private IEnumerator AnimateRoutine(Vector3 targetPosition, Vector3 targetScale, Quaternion targetRotation, float duration, float delay = 0f)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 startPosition = _cachedTransform.localPosition;
        Vector3 startScale = _cachedTransform.localScale;
        Quaternion startRotation = _cachedTransform.localRotation;

        float time = 0f;
        
        while (time < 1f)
        {
            time += Time.deltaTime / Mathf.Max(0.0001f, duration);
            
            float smoothStep = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(time));

            _cachedTransform.localPosition = Vector3.LerpUnclamped(startPosition, targetPosition, smoothStep);
            _cachedTransform.localScale    = Vector3.LerpUnclamped(startScale, targetScale, smoothStep);
            _cachedTransform.localRotation = Quaternion.SlerpUnclamped(startRotation, targetRotation, smoothStep);

            yield return null;
        }

        _cachedTransform.localPosition = targetPosition;
        _cachedTransform.localScale = targetScale;
        _cachedTransform.localRotation = targetRotation;
        _moveRoutine = null;
    }
}