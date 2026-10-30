// =====================================================================
//  Program.cs  —  the interactive console UI for MyBudget (Assignment 1).
//  Target framework: .NET 10 (LTS), language C# 14.
//
//  >>> BUILD THE MENU-DRIVEN UI HERE (Modules 1-3). <<<
//
//  Once you have implemented BudgetRules.cs (so the unit tests pass), wire it
//  up to a console interface that meets the assignment brief:
//
//    * Print a banner (try a raw string literal).
//    * Loop a menu until the user exits, using a switch on the choice:
//        1) Add an expense   2) View summary   3) Set monthly budget   4) Exit
//    * Read and VALIDATE input, re-prompting on bad data (decimal.TryParse,
//      BudgetRules.NormalizeCategory, a date parse, non-empty text).
//    * Keep running totals in simple variables (no collections / no classes).
//    * Use BudgetRules.ValidateAmount / ClassifyAmount / BudgetStatus /
//      FormatCurrency for all logic and formatting.
//    * Handle bad input with try / catch / finally and InvalidExpenseException.
//
//  See section 6 of the assignment brief for a sample run to aim for.
// =====================================================================
using ExpenseTracker;

// Delete the line above and implement the application here.

decimal totalExpenses = 0m;
decimal highestExpense = 0m;
decimal? monthlyBudget = null;

int expenseCount = 0;

decimal foodTotal = 0m;
decimal transportationTotal = 0m;
decimal utilitiesTotal = 0m;
decimal entertainmentTotal = 0m;
decimal otherTotal = 0m;

bool isRunning = true;

Console.WriteLine("""
    ==========================================
                MyBudget Expense Tracker
    ==========================================
    """);

while (isRunning)
{
    Console.WriteLine("""
        Please select an option:
        1) Add an expense        2) View summary        3) Set monthly budget        4) Exit
        
        """);


    string choice = Console.ReadLine()?.Trim() ?? "";
    switch (choice)
    {
        case "1":
            AddExpense(); //The method to add an expense is called here
            break;
        case "2":
            ViewSummary(); //The method to view the summary is called here
            break;
        case "3":
            SetMonthlyBudget();//The method to set the monthly budget is called here
            break;
        case "4":
            isRunning = false;
            Console.WriteLine("Exiting the application. Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}


// Method to add an expense: Add validation for description, amount, category, date, and optional note. Updates running totals and displays confirmation.
void AddExpense()
{
    string description;

    // Validate description
    while (true)
    {
        Console.Write("Description : ");
        description = Console.ReadLine()?.Trim() ?? "";

        if (!string.IsNullOrWhiteSpace(description))
            break;

        Console.WriteLine("  Description cannot be empty.");
    }

    // Validate amount
    decimal amount;

    while (true)
    {
        Console.Write("Amount      : ");
        string? input = Console.ReadLine();

        if (!decimal.TryParse(input, out decimal parsedAmount))
        {
            Console.WriteLine("  Please enter a valid number.");
            continue;
        }

        try
        {
            amount = BudgetRules.ValidateAmount(parsedAmount);
            break;
        }
        catch (InvalidExpenseException ex)
        {
            Console.WriteLine($"  {ex.Message}");
        }
        finally
        {
            // Validation attempt completed.
            // No external resources require cleanup.
        }
    }

    // Validate category
    string category;

    while (true)
    {
        Console.Write(
            "Category    : [Food/Transport/Utilities/Entertainment/Other] ");

        string? input = Console.ReadLine();
        string? normalized = BudgetRules.NormalizeCategory(input);

        if (normalized is not null)
        {
            category = normalized;
            break;
        }

        Console.WriteLine("  Invalid category. Please try again.");
    }

    // Validate date
    DateTime expenseDate;

    while (true)
    {
        Console.Write("Date (blank = today): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            expenseDate = DateTime.Today;
            break;
        }

        if (DateTime.TryParse(input, out expenseDate))
            break;

        Console.WriteLine("  Invalid date. Please try again.");
    }

    // Optional note
    Console.Write("Note (optional): ");
    string note = Console.ReadLine()?.Trim() ?? "";

    // Update running totals
    totalExpenses += amount;
    expenseCount++;

    if (amount > highestExpense)
        highestExpense = amount;

    switch (category)
    {
        case "Food":
            foodTotal += amount;
            break;

        case "Transport":
            transportationTotal += amount;
            break;

        case "Utilities":
            utilitiesTotal += amount;
            break;

        case "Entertainment":
            entertainmentTotal += amount;
            break;

        case "Other":
            otherTotal += amount;
            break;
    }

    // Confirmation
    Console.WriteLine(
        $"  Recorded: {BudgetRules.FormatCurrency(amount)} | " +
        $"{category} | {expenseDate:yyyy-MM-dd}");

    Console.WriteLine(
        $"    Size band : {BudgetRules.ClassifyAmount(amount)}");

    // Show budget status only when configured
    if (monthlyBudget.HasValue)
    {
        decimal remaining = monthlyBudget.Value - totalExpenses;

        Console.WriteLine(
            $"  Budget: {BudgetRules.FormatCurrency(remaining)} remaining of " +
            $"{BudgetRules.FormatCurrency(monthlyBudget.Value)} -> " +
            BudgetRules.BudgetStatus(remaining, monthlyBudget.Value));
    }

}


// Method to view the summary: Displays total expenses, highest expense, average expense, and spending by category. Also shows budget status if set.
void ViewSummary()
{
    Console.WriteLine();
    Console.WriteLine("========== Expense Summary ==========");

    Console.WriteLine($"Expenses recorded : {expenseCount}");

    Console.WriteLine(
        $"Total spent       : {BudgetRules.FormatCurrency(totalExpenses)}");

    Console.WriteLine(
        $"Highest expense   : {BudgetRules.FormatCurrency(highestExpense)}");

    decimal average = expenseCount > 0
        ? totalExpenses / expenseCount
        : 0m;

    Console.WriteLine(
        $"Average expense   : {BudgetRules.FormatCurrency(average)}");

    Console.WriteLine();
    Console.WriteLine("Spending by category:");

    Console.WriteLine($"  Food          : {BudgetRules.FormatCurrency(foodTotal)}");
    Console.WriteLine($"  Transport     : {BudgetRules.FormatCurrency(transportationTotal)}");
    Console.WriteLine($"  Utilities     : {BudgetRules.FormatCurrency(utilitiesTotal)}");
    Console.WriteLine($"  Entertainment : {BudgetRules.FormatCurrency(entertainmentTotal)}");
    Console.WriteLine($"  Other         : {BudgetRules.FormatCurrency(otherTotal)}");

    Console.WriteLine();

    if (monthlyBudget.HasValue)
    {
        decimal remaining = monthlyBudget.Value - totalExpenses;

        Console.WriteLine(
            $"Budget remaining : {BudgetRules.FormatCurrency(remaining)}");

        Console.WriteLine(
            $"Budget status    : " +
            BudgetRules.BudgetStatus(remaining, monthlyBudget.Value));
    }
    else
    {
        Console.WriteLine("Monthly budget has not been set.");
    }

    Console.WriteLine("=====================================");
}


// Method to set the monthly budget: Validates input and updates the budget. Displays confirmation and remaining budget.
void SetMonthlyBudget()
{
    int validationAttempts = 0;

    while (true)
    {
        Console.Write("Monthly budget: ");
        string? input = Console.ReadLine();

        if (!decimal.TryParse(input, out decimal amount))
        {
            Console.WriteLine("  Please enter a valid number.");
            continue;
        }

        try
        {
            decimal validatedBudget = BudgetRules.ValidateAmount(amount);
            monthlyBudget = validatedBudget;

            Console.WriteLine(
                $"Budget set to {BudgetRules.FormatCurrency(validatedBudget)}.");

            decimal remaining = validatedBudget - totalExpenses;

            Console.WriteLine(
                $"  Budget: {BudgetRules.FormatCurrency(remaining)} remaining of " +
                $"{BudgetRules.FormatCurrency(validatedBudget)} -> " +
                BudgetRules.BudgetStatus(remaining, validatedBudget));

            break;
        }
        catch (InvalidExpenseException ex)
        {
            Console.WriteLine($"  {ex.Message}");
        }
        finally
        {
            validationAttempts++;
        }
    }
}
