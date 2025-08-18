using System;
using UnityEngine;

public interface ICameraMoverInputProvider
{
    public event Action<Vector2> PanInputed;
}