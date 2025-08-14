using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckPlacementListener : IDisposable
{
    private readonly HexTileGridBuilder _gridBuilder;
    private readonly HexDeckController _deckController;

    private bool _initialEventConsumed;

    public DeckPlacementListener(HexTileGridBuilder gridBuilder, HexDeckController deckController)
    {
        _gridBuilder = gridBuilder;
        _deckController = deckController;
        _gridBuilder.OnBuildCompleted += HandleBuildCompleted;
    }

    void IDisposable.Dispose()
    {
        _gridBuilder.OnBuildCompleted -= HandleBuildCompleted;
    }

    private void HandleBuildCompleted(Dictionary<Vector2Int, HexCellView> map, Vector2Int position)
    {
        if (!_initialEventConsumed)
        {
            _initialEventConsumed = true;
            
            return;
        }

        _deckController.TilePlaced();
    }
}