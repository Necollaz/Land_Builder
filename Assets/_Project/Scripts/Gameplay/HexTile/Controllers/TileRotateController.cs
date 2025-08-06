using UnityEngine;

public class TileRotateController
{
    private const float ROTATION_STEP_ANGLE = 60f;
    private const float FULL_CIRCLE_ANGLE = 360f;
        
    public void HandleRotation(GameObject tile)
    {
        if (tile == null)
            return;
        
        float scroll = Input.mouseScrollDelta.y;
            
        if (scroll > 0f)
            Rotate(tile, ROTATION_STEP_ANGLE);
        else if (scroll < 0f)
            Rotate(tile, -ROTATION_STEP_ANGLE);
        
        if (Input.GetKeyDown(KeyCode.Q))
            Rotate(tile, -ROTATION_STEP_ANGLE);
            
        if (Input.GetKeyDown(KeyCode.E))
            Rotate(tile, ROTATION_STEP_ANGLE);
    }
        
    private void Rotate(GameObject tile, float angleDelta)
    {
        Vector3 angles = tile.transform.eulerAngles;
            
        angles.y = (angles.y + angleDelta) % FULL_CIRCLE_ANGLE;
        tile.transform.eulerAngles = angles;
    }
}