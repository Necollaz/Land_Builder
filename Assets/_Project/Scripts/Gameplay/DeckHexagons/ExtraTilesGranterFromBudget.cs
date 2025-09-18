public class ExtraTilesGranterFromBudget : IExtraTilesGranter
{
    private readonly PlacementBudgetModel budgetModel;

    public ExtraTilesGranterFromBudget(PlacementBudgetModel budgetModel)
    {
        this.budgetModel = budgetModel;
    }

    public void GrantExtraTiles(int count)
    {
        budgetModel.Add(count);
    }
}