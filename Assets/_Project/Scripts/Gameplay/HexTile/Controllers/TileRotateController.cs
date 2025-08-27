using UnityEngine;

public class TileRotateController
{
    private const float ROTATION_STEP_ANGLE = 60f;
    private const float FULL_CIRCLE_ANGLE = 360f;
        
    public void HandleRotation(Hexagon tile)
    {
        if (tile == null) return;

        int delta = 0;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0.1f) delta += 1;
        else if (scroll < -0.1f) delta -= 1;

        if (Input.GetKeyDown(KeyCode.Q)) delta -= 1;
        if (Input.GetKeyDown(KeyCode.E)) delta += 1;

        if (delta != 0)
            tile.RotateSteps(delta);
    }
        
    private void Rotate(Hexagon tile, float angleDelta)
    {
        Vector3 angles = tile.transform.eulerAngles;
            
        angles.y = (angles.y + angleDelta) % FULL_CIRCLE_ANGLE;
        tile.transform.eulerAngles = angles;
    }
}