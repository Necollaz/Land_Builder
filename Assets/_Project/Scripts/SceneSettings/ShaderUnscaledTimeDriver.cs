using UnityEngine;

public class ShaderUnscaledTimeDriver : MonoBehaviour
{
    private readonly int unscaledTimeID = Shader.PropertyToID("_UnscaledTime");

    private void Update()
    {
        Shader.SetGlobalFloat(unscaledTimeID, Time.unscaledTime);
    }
}