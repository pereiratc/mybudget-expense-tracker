
namespace MyBudget.Core;

// Manages the monthly budget and evaluates spending.
public class BudgetService : IBudgetService
{
    // Stores the monthly limit. Only this class can change it.
    public decimal MonthlyLimit { get; private set; }

    // Sets a valid monthly budget limit.
    public void SetMonthlyLimit(decimal limit)
    {
        if (limit <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(limit),
                "Monthly budget must be greater than zero.");
        }

        MonthlyLimit = Math.Round(limit, 2);
    }

    // Calculates how much money remains in the budget.
    public decimal Remaining(decimal totalSpent)
    {
        return MonthlyLimit - totalSpent;
    }

    // Determines the budget status based on spending.
    public BudgetStatus Evaluate(decimal totalSpent)
    {
        if (MonthlyLimit == 0)
        {
            return BudgetStatus.NotSet;
        }

        decimal remaining = Remaining(totalSpent);

        if (remaining < 0)
        {
            return BudgetStatus.OverBudget;
        }
        else if (remaining < MonthlyLimit * 0.10m)
        {
            return BudgetStatus.AlmostOut;
        }
        else
        {
            return BudgetStatus.OnTrack;
        }
    }
}

