
namespace MyBudget.Core;

public sealed record RecurringExpense(
    Guid Id,
    string Description,
    decimal Amount,
    ExpenseCategory Category,
    DateOnly Date,
    int TimesPerMonth
) : Expense(Id, Description, Amount, Category, Date)
{
    public override decimal MonthlyImpact => Amount * TimesPerMonth;

    public override string ToReportLine()
    {
        return $"{base.ToReportLine()} (x{TimesPerMonth}/month)";
    }
}
