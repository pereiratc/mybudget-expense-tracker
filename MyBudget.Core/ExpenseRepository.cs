
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBudget.Core;

// Manages expenses using a collection and a storage interface.
public class ExpenseRepository : IExpenseRepository
{
    private readonly IExpenseStore _store;
    private readonly List<Expense> _expenses;

    // Receives the storage dependency and loads existing expenses.
    public ExpenseRepository(IExpenseStore store)
    {
        _store = store;
        _expenses = store.Load().ToList();
    }

    // Returns all expenses ordered by date.
    public IReadOnlyList<Expense> GetAll()
    {
        return _expenses.OrderBy(e => e.Date).ToList();
    }

    // Adds a valid expense to the collection.
    public void Add(Expense expense)
    {
        if (expense is null)
        {
            throw new ArgumentNullException(nameof(expense));
        }

        _expenses.Add(expense);
    }

    // Calculates the total monthly impact of all expenses.
    public decimal Total()
    {
        return _expenses.Sum(e => e.MonthlyImpact);
    }

    // Calculates total spending for each expense category.
    public IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory()
    {
        return _expenses
            .GroupBy(e => e.Category)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(e => e.MonthlyImpact));
    }

    // Returns expenses matching a category, ordered by date.
    public IReadOnlyList<Expense> InCategory(ExpenseCategory category)
    {
        return _expenses
            .Where(e => e.Category == category)
            .OrderBy(e => e.Date)
            .ToList();
    }

    // Saves the current expenses through the storage interface.
    public void Save()
    {
        _store.Save(_expenses);
    }
}

