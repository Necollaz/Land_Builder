using UnityEngine;

public class CameraController
{
    public static Camera MainCamera { get; private set; }
    
    public CameraController(Camera camera)
    {
        MainCamera = camera;
    }
}