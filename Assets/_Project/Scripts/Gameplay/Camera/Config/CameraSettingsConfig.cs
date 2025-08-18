using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettingsConfig", menuName = "Game/Configs/Camera Settings")]
public class CameraSettingsConfig : ScriptableObject
{
    [field: Header("General")]
    [field: SerializeField, Tooltip("Игнорировать ввод, если курсор/палец над UI-элементами (EventSystem).")] public bool IgnoreWhenOverUI { get; private set; } = true;
    [field: SerializeField, Tooltip("Мировой множитель сдвига цели камеры при панорамировании (чем больше — тем быстрее ездит камера по миру).")] public float PanMultiplierWorld { get; private set; } = 1.0f;
    
    [field: Header("Pan Smoothing")]
    [field: SerializeField, Tooltip("Время сглаживания при перемещении камеры (чем больше — тем плавнее и медленнее отклик).")] public float PanSmoothSeconds { get; private set; } = 0.08f;
    [field: SerializeField, Tooltip("Максимальная скорость перемещения камеры. Ограничивает слишком резкие скачки при перетаскивании.")] public float MaxPanUnitsPerSecond { get; private set; } = 100f;
    
    [field: Header("World Bounds (X/Z)")]
    [field: SerializeField, Tooltip("Минимальная координата X для цели камеры.")] public float MinX { get; private set; } = -50f;
    [field: SerializeField, Tooltip("Максимальная координата X для цели камеры.")] public float MaxX { get; private set; } = 50f;
    [field: SerializeField, Tooltip("Минимальная координата Z для цели камеры.")] public float MinZ { get; private set; } = -50f;
    [field: SerializeField, Tooltip("Максимальная координата Z для цели камеры.")] public float MaxZ { get; private set; } = 50f;
    
    [field: Header("Zoom (Cinemachine Ortho)")]
    [field: SerializeField, Tooltip("Скорость изменения размера ортографической проекции камеры при зуме (общий множитель).")] public float ZoomSpeed { get; private set; } = 1.0f;
    [field: SerializeField, Tooltip("Минимальное значение OrthographicSize.")] public float MinZoom { get; private set; } = 3f;
    [field: SerializeField, Tooltip("Максимальное значение OrthographicSize.")] public float MaxZoom { get; private set; } = 20f;

    [field: Header("Zoom-Based Sensitivity")]
    [field: SerializeField, Tooltip("Масштабировать чувствительность панорамирования в зависимости от текущего зума.")] public bool ScalePanByZoom { get; private set; } = true;
    [field: SerializeField, Tooltip("Масштабировать чувствительность поворота в зависимости от текущего зума.")] public bool ScaleRotateByZoom { get; private set; } = false;
    [field: SerializeField, Tooltip("Опорное значение OrthographicSize.")] public float ReferenceOrthoSize { get; private set; } = 10f;
    
    [field: Header("Desktop Input")]
    [field: SerializeField, Tooltip("Чувствительность панорамирования мышью.")] public float DesktopPanSensitivity { get; private set; } = 1.0f;
    [field: SerializeField, Tooltip("Инвертировать направление панорамирования мышью.")] public bool DesktopPanInvert { get; private set; } = false;
    [field: SerializeField, Tooltip("Чувствительность поворота при зажатой средней кнопке мыши.")] public float DesktopRotateSensitivity { get; private set; } = 1.0f;
    [field: SerializeField, Tooltip("Чувствительность колесика мыши для зума.")] public float DesktopWheelZoomSensitivity { get; private set; } = 1.0f;

    [field: Header("Mobile Input")]
    [field: SerializeField, Tooltip("Чувствительность панорамирования одним пальцем.")] public float MobilePanSensitivity { get; private set; } = 0.6f;
    [field: SerializeField, Tooltip("Инвертировать направление панорамирования касанием.")] public bool MobilePanInvert { get; private set; } = false;
    [field: SerializeField, Tooltip("Чувствительность поворота по горизонтальному свайпу.")] public float MobileRotateSensitivity { get; private set; } = 0.6f;
    [field: SerializeField, Tooltip("Чувствительность pinch-жеста для зума.")] public float MobilePinchZoomSensitivity { get; private set; } = 0.6f;
}