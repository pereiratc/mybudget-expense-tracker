
namespace MyBudget.Core;

// Creates expense objects and validates their input values.
public static class ExpenseFactory
{
    // Validates the amount and rounds it to two decimal places.
    public static decimal ValidateAmount(decimal amount)
    {
        if (amount <= 0 || amount > 1_000_000)
        {
            throw new InvalidExpenseException("Invalid expense amount.");
        }

        return Math.Round(amount, 2);
    }

    // Creates a one-time expense with a unique identifier.
    public static OneTimeExpense CreateOneTime(
        string description,
        decimal amount,
        ExpenseCategory category,
        DateOnly date)
    {
        // Prevent empty or whitespace-only descriptions.
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException("Description is required.");
        }

        return new OneTimeExpense(
            Guid.NewGuid(),
            description.Trim(),
            ValidateAmount(amount),
            category,
            date);
    }

    // Creates a recurring expense with a monthly frequency.
    public static RecurringExpense CreateRecurring(
        string description,
        decimal amount,
        ExpenseCategory category,
        DateOnly date,
        int timesPerMonth)
    {
        // Validate the description and recurrence frequency.
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException("Description is required.");
        }

        if (timesPerMonth < 1)
        {
            throw new InvalidExpenseException("Frequency must be at least 1.");
        }

        return new RecurringExpense(
            Guid.NewGuid(),
            description.Trim(),
            ValidateAmount(amount),
            category,
            date,
            timesPerMonth);
    }
}

