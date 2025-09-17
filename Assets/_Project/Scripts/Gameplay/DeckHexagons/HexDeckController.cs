using UnityEngine;

public class HexDeckController
{
    private readonly HexDeckModel model;
    private readonly HexDeckView view;
    
    private readonly int preloadVisualsCount;

    public HexDeckController(HexDeckModel model, HexDeckView view, int preloadVisualsCount)
    {
        this.model = model;
        this.view = view;
        this.preloadVisualsCount = Mathf.Max(1, preloadVisualsCount);
        
        view.SetSnapshot(model.SnapshotAll());
        view.RebuildVisuals(this.preloadVisualsCount);
        view.RefreshLayout();
    }
    
    public void TilePlaced()
    {
        model.Draw();
        view.SetSnapshot(model.SnapshotAll());
        view.ShiftAfterDraw();
    }
}