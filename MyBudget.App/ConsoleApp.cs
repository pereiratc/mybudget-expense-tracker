using MyBudget.Core;

namespace MyBudget.App;

/// <summary>
/// The interactive console UI. Depends only on the service abstractions, which
/// are supplied by the DI container (constructor injection via a primary constructor).
/// </summary>
public sealed class ConsoleApp(IExpenseRepository repository, IBudgetService budget)
{

    // Displays a message using the specified console color.
    private static void WriteColored(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void Run()
    {
        WriteColored("============================================================", ConsoleColor.Cyan);
        WriteColored("                  MyBudget - Assignment 2", ConsoleColor.Yellow);
        WriteColored("============================================================", ConsoleColor.Cyan);

        bool running = true;

        while (running)
        {
            Console.WriteLine();
            WriteColored("=============== MAIN MENU ===============", ConsoleColor.Cyan);
            Console.WriteLine("  1) Add expense");
            Console.WriteLine("  2) List expenses");
            Console.WriteLine("  3) Financial report");
            Console.WriteLine("  4) Set monthly budget");
            Console.WriteLine("  5) Save & exit");
            WriteColored("=========================================", ConsoleColor.Cyan);

            Console.Write("Choose an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": AddExpense(); break;
                case "2": ListExpenses(); break;
                case "3": ShowReport(); break;
                case "4": SetBudget(); break;
                case "5": repository.Save();
                    running = false;
                    WriteColored("Saved. Goodbye!", ConsoleColor.Green); break;
                default: WriteColored("Unknown option.", ConsoleColor.Red); break;
            }
        }
    }

    private void AddExpense()
    {
        try
        {
            string description = ReadNonEmpty("Description : ");
            decimal amount = ReadDecimal("Amount      : ");
            ExpenseCategory category = ReadCategory();
            DateOnly date = ReadDate();

            Console.Write("Recurring? (y/N): ");
            bool recurring = (Console.ReadLine()?.Trim().ToLowerInvariant()) is "y" or "yes";

            Expense expense = recurring
                ? ExpenseFactory.CreateRecurring(description, amount, category, date, ReadInt("Times per month: "))
                : ExpenseFactory.CreateOneTime(description, amount, category, date);

            repository.Add(expense);
            Console.WriteLine($"Added: {expense.ToReportLine()}");
        }
        catch (InvalidExpenseException ex)
        {
            Console.WriteLine($"Rejected: {ex.Message}");
        }
    }

    private void ListExpenses()
    {
        IReadOnlyList<Expense> all = repository.GetAll();
        if (all.Count == 0)
        {
            Console.WriteLine("No expenses yet.");
            return;
        }

        foreach (Expense e in all)
            Console.WriteLine("  " + e.ToReportLine());
    }

    private void ShowReport()
    {
        decimal total = repository.Total();
        Console.WriteLine($"\nMonthly impact total: ${total:0.00}");

        Console.WriteLine("By category:");
        foreach ((ExpenseCategory category, decimal amount) in repository.TotalsByCategory())
            Console.WriteLine($"  {category,-13} ${amount:0.00}");

        BudgetStatus status = budget.Evaluate(total);
        if (status == BudgetStatus.NotSet)
        {
            Console.WriteLine("Budget: not set (use option 4).");
            return;
        }

        string label = status switch
        {
            BudgetStatus.OverBudget => "OVER BUDGET",
            BudgetStatus.AlmostOut => "Almost out",
            _ => "On track",
        };
        Console.WriteLine($"Budget: ${budget.Remaining(total):0.00} remaining of ${budget.MonthlyLimit:0.00} -> {label}");
    }

    private void SetBudget()
    {
        try
        {
            budget.SetMonthlyLimit(ReadDecimal("Monthly budget: "));
            Console.WriteLine($"Budget set to ${budget.MonthlyLimit:0.00}.");
        }
        catch (InvalidExpenseException ex)
        {
            Console.WriteLine($"Rejected: {ex.Message}");
        }
    }

    // ----- input helpers -------------------------------------------------

    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();
            Console.WriteLine("  Cannot be empty.");
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal value))
                return value;
            Console.WriteLine("  Enter a number, e.g. 12.50.");
        }
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value))
                return value;
            Console.WriteLine("  Enter a whole number.");
        }
    }

    // Accepts full category names or single-letter shortcuts.
    private static ExpenseCategory ReadCategory()
    {
        while (true)
        {
            WriteColored("Select an expense category:", ConsoleColor.Cyan);

            Console.WriteLine("  [F] Food");
            Console.WriteLine("  [T] Transport");
            Console.WriteLine("  [U] Utilities");
            Console.WriteLine("  [E] Entertainment");
            Console.WriteLine("  [O] Other");

            Console.Write("Category: ");

            // Read and normalize user input.
            string? input = Console.ReadLine()?.Trim();

            // Convert shortcuts into full category names.
            input = input?.ToUpperInvariant() switch
            {
                "F" => "Food",
                "T" => "Transport",
                "U" => "Utilities",
                "E" => "Entertainment",
                "O" => "Other",
                _ => input
            };
            //Console.WriteLine($"DEBUG Input: '{input}'");
            //Console.WriteLine($"DEBUG Categories: {string.Join(", ", Enum.GetNames<ExpenseCategory>())}");

            /* if (Enum.TryParse(Console.ReadLine()?.Trim(), ignoreCase: true, out ExpenseCategory category)
                 && Enum.IsDefined(category))
                 return category;*/

            // Check the result of each validation.
           /* bool parsed = Enum.TryParse(
                input,
                ignoreCase: true,
                out ExpenseCategory category);

            bool defined = Enum.IsDefined(category);*/

            //Console.WriteLine($"DEBUG: Input={input}, Parsed={parsed}, Defined={defined}");

            if (Enum.TryParse(input, ignoreCase: true, out ExpenseCategory category)
                 && Enum.IsDefined(category))
                return category;

            WriteColored("  Unknown category.", ConsoleColor.Red);
        }
    }

    private static DateOnly ReadDate()
    {
        while (true)
        {
            Console.Write("Date (yyyy-mm-dd, blank = today): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return DateOnly.FromDateTime(DateTime.Today);
            if (DateOnly.TryParse(input, out DateOnly date))
                return date;
            Console.WriteLine("  Enter a valid date (yyyy-mm-dd).");
        }
    }
}
