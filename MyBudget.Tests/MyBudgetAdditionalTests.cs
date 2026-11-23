
using MyBudget.Core;
using Xunit;

namespace MyBudget.Tests;

// Additional unit tests for Assignment 2.
public class MyBudgetAdditionalTests
{
    [Fact]
    public void CreateOneTime_GeneratesDifferentIds()
    {
        // Arrange
        DateOnly date = new DateOnly(2026, 11, 22);

        // Act
        var first = ExpenseFactory.CreateOneTime(
            "Lunch", 15m, ExpenseCategory.Food, date);

        var second = ExpenseFactory.CreateOneTime(
            "Lunch", 15m, ExpenseCategory.Food, date);

        // Assert
        Assert.NotEqual(first.Id, second.Id);
    }

    [Theory]
    [InlineData(2, 50)]
    [InlineData(4, 100)]
    [InlineData(10, 250)]
    public void CreateRecurring_CalculatesMonthlyImpact(
        int frequency,
        int expectedTotal)
    {
        // Arrange
        decimal amount = 25m;

        // Act
        var expense = ExpenseFactory.CreateRecurring(
            "Transport",
            amount,
            ExpenseCategory.Transport,
            new DateOnly(2026, 11, 22),
            frequency);

        // Assert
        Assert.Equal((decimal)expectedTotal, expense.MonthlyImpact);
    }

    [Fact]
    public void BudgetService_Remaining_CalculatesBalance()
    {
        // Arrange
        var budget = new BudgetService();
        budget.SetMonthlyLimit(1000m);

        // Act
        decimal remaining = budget.Remaining(350m);

        // Assert
        Assert.Equal(650m, remaining);
    }
}
