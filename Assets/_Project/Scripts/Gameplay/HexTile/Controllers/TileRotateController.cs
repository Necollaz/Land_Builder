using UnityEngine;

public class TileRotateController
{
    public void TryRotation(Hexagon tile)
    {
        if (tile == null) return;

        int delta = 0;

        if (Input.GetKeyDown(KeyCode.Q))
            delta -= 1;
        if (Input.GetKeyDown(KeyCode.E))
            delta += 1;
        
        if (delta != 0)
            tile.RotateSteps(delta);

        if (delta != 0)
            tile.RotateSteps(delta);
    }
}