using UnityEngine;

[CreateAssetMenu(fileName = "HexTypePalette", menuName = "Game/Configs/Hex Type Palette")]
public class HexTypePaletteConfig : ScriptableObject
{
    [Header("Colors by HexType")]
    public Color Forest = new Color32(0, 70, 0, 255);
    public Color Water = new Color32(0, 0, 180, 255);
    public Color Grass = new Color32(120, 220, 120, 255);
    public Color Rock = new Color32(120, 120, 120, 255);
    public Color Building = new Color32(200, 30, 30, 255);
    public Color Sand = new Color32(230, 200, 60, 255);

    public Color Get(HexType type)
    {
        switch (type)
        {
            case HexType.Forest:
                return Forest;
            case HexType.Water:
                return Water;
            case HexType.Grass:
                return Grass;
            case HexType.Rock:
                return Rock;
            case HexType.Building:
                return Building;
            case HexType.Sand:
                return Sand;
            default:
                return Color.white;
        }
    }
}