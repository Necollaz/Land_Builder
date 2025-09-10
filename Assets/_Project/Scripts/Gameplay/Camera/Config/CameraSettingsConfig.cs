using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettingsConfig", menuName = "Game/Configs/Camera Settings")]
public class CameraSettingsConfig : ScriptableObject
{
    [Header("General")]
    [SerializeField, Tooltip("Игнорировать ввод, если курсор/палец над UI-элементами (EventSystem).")] private bool _ignoreWhenOverUI = true;
    [SerializeField, Tooltip("Мировой множитель сдвига цели камеры при панорамировании (чем больше — тем быстрее ездит камера по миру).")] private float _panMultiplierWorld = 1.0f;

    [Header("Pan Smoothing")]
    [SerializeField, Tooltip("Время сглаживания при перемещении камеры (чем больше — тем плавнее и медленнее отклик).")] private float _panSmoothSeconds = 0.08f;
    [SerializeField, Tooltip("Максимальная скорость перемещения камеры. Ограничивает слишком резкие скачки при перетаскивании.")] private float _maxPanUnitsPerSecond = 100f;

    [Header("Rotate Smoothing")]
    [SerializeField, Tooltip("Время сглаживания поворота камеры (сек). Чем больше — тем плавнее и медленнее отклик.")] private float _rotateSmoothSeconds = 0.10f;
    [SerializeField, Tooltip("Максимальная скорость поворота (град/сек). 0 = без лимита.")] private float _maxRotateDegreesPerSecond = 540f;

    [Header("World Bounds (X/Z)")]
    [SerializeField, Tooltip("Минимальная координата X для цели камеры.")] private float _minX = -50f;
    [SerializeField, Tooltip("Максимальная координата X для цели камеры.")] private float _maxX = 50f;
    [SerializeField, Tooltip("Минимальная координата Z для цели камеры.")] private float _minZ = -50f;
    [SerializeField, Tooltip("Максимальная координата Z для цели камеры.")] private float _maxZ = 50f;

    [Header("Zoom (Cinemachine Ortho)")]
    [SerializeField, Tooltip("Скорость изменения размера ортографической проекции камеры при зуме (общий множитель).")] private float _zoomSpeed = 1.0f;
    [SerializeField, Tooltip("Минимальное значение OrthographicSize.")] private float _minZoom = 3f;
    [SerializeField, Tooltip("Максимальное значение OrthographicSize.")] private float _maxZoom = 20f;

    [Header("Zoom-Based Sensitivity")]
    [SerializeField, Tooltip("Масштабировать чувствительность панорамирования в зависимости от текущего зума.")] private bool _scalePanByZoom = true;
    [SerializeField, Tooltip("Масштабировать чувствительность поворота в зависимости от текущего зума.")] private bool _scaleRotateByZoom = false;
    [SerializeField, Tooltip("Опорное значение OrthographicSize.")] private float _referenceOrthoSize = 10f;

    [Header("Desktop Input")]
    [SerializeField, Tooltip("Чувствительность панорамирования мышью.")] private float _desktopPanSensitivity = 1.0f;
    [SerializeField, Tooltip("Инвертировать направление панорамирования мышью.")] private bool _desktopPanInvert = false;
    [SerializeField, Tooltip("Чувствительность поворота (ЛКМ+ПКМ).")] private float _desktopRotateSensitivity = 1.0f;
    [SerializeField, Tooltip("Чувствительность колесика/драга для зума.")] private float _desktopWheelZoomSensitivity = 1.0f;

    [Header("Mobile Input")]
    [SerializeField, Tooltip("Чувствительность панорамирования одним пальцем.")] private float _mobilePanSensitivity = 0.6f;
    [SerializeField, Tooltip("Инвертировать направление панорамирования касанием.")] private bool _mobilePanInvert = false;
    [SerializeField, Tooltip("Чувствительность поворота по горизонтальному свайпу.")] private float _mobileRotateSensitivity = 0.6f;
    [SerializeField, Tooltip("Чувствительность pinch-жеста для зума.")] private float _mobilePinchZoomSensitivity = 0.6f;
    
    public float PanSmoothSeconds => _panSmoothSeconds;
    public float PanMultiplierWorld => _panMultiplierWorld;
    public float MaxPanUnitsPerSecond => _maxPanUnitsPerSecond;
    public float RotateSmoothSeconds => _rotateSmoothSeconds;
    public float MaxRotateDegreesPerSecond => _maxRotateDegreesPerSecond;
    public float MinX => _minX;
    public float MaxX => _maxX;
    public float MinZ => _minZ;
    public float MaxZ => _maxZ;
    public float ZoomSpeed => _zoomSpeed;
    public float MinZoom => _minZoom;
    public float MaxZoom => _maxZoom;
    public float ReferenceOrthoSize => _referenceOrthoSize;
    public float DesktopPanSensitivity => _desktopPanSensitivity;
    public float DesktopRotateSensitivity => _desktopRotateSensitivity;
    public float DesktopWheelZoomSensitivity => _desktopWheelZoomSensitivity;
    public float MobilePanSensitivity => _mobilePanSensitivity;
    public float MobileRotateSensitivity => _mobileRotateSensitivity;
    public float MobilePinchZoomSensitivity => _mobilePinchZoomSensitivity;
    public bool IgnoreWhenOverUI => _ignoreWhenOverUI;
    public bool ScalePanByZoom => _scalePanByZoom;
    public bool ScaleRotateByZoom => _scaleRotateByZoom;
    public bool DesktopPanInvert => _desktopPanInvert;
    public bool MobilePanInvert => _mobilePanInvert;
}