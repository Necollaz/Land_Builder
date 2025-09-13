using System;
using UnityEngine;

public class PlacementBudgetListener : IDisposable
{
    private readonly HexTileGridBuilder gridBuilder;
    private readonly PlacementBudgetModel budgetModel;
    private readonly GameResultModel gameResultModel;

    private bool _initialEventConsumed;
    private bool _defeatFired;

    public PlacementBudgetListener(HexTileGridBuilder gridBuilder, PlacementBudgetModel budgetModel, GameResultModel gameResultModel)
    {
        this.gridBuilder = gridBuilder;
        this.budgetModel = budgetModel;
        this.gameResultModel = gameResultModel;

        this.gridBuilder.OnBuildCompleted += HandleBuildCompleted;
    }

    void IDisposable.Dispose()
    {
        gridBuilder.OnBuildCompleted -= HandleBuildCompleted;
    }

    private void HandleBuildCompleted(System.Collections.Generic.Dictionary<Vector2Int, HexCellView> map, Vector2Int position)
    {
        if (!_initialEventConsumed)
        {
            _initialEventConsumed = true;
            
            return;
        }

        if (_defeatFired)
            return;
        
        if (!budgetModel.TrySpendOne())
        {
            _defeatFired = true;
            
            gameResultModel.SetDefeat();
            
            return;
        }

        if (budgetModel.Remaining <= 0)
        {
            _defeatFired = true;
            
            gameResultModel.SetDefeat();
        }
    }
}