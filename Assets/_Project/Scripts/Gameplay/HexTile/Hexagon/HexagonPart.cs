using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonPart : MonoBehaviour
{
    [field: SerializeField] public HexagonSides Side { get; private set; }
    
    [field: SerializeField] public HexType Type { get; private set; }
}
