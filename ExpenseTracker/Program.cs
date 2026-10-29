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
        1) Add an expense
        2) View summary
        3) Set monthly budget
        4) Exit

        """);


    string choice = Console.ReadLine()?.Trim() ?? "";
    switch (choice)
    {
        case "1":
            AddExpense();
            break;
        case "2":
            ViewSummary();
            break;
        case "3":
            SetMonthlyBudget();
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

void AddExpense()
{
    throw new NotImplementedException();
}

void ViewSummary()
{
    throw new NotImplementedException();
}

void SetMonthlyBudget()
{
    throw new NotImplementedException();
}
