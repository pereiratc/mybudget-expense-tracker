// =====================================================================
//  BudgetRules.cs  —  the PROVIDED, TESTABLE CONTRACT for Assignment 1.
//
//  >>> THIS IS WHERE YOU WORK. <<<
//
//  The instructor-provided xUnit tests (ExpenseTracker.Tests) target this
//  class by name. Do NOT rename the class, the methods, or change their
//  signatures, and do NOT modify the tests. Replace each "throw new
//  NotImplementedException()" with a correct implementation so that every
//  test passes (Test Explorer, or:  dotnet test).
//
//  Work one method at a time. Run the tests, watch them turn green.
//  All methods are pure: no Console I/O, no shared state.
// =====================================================================
namespace ExpenseTracker;

public static class BudgetRules
{
    public const decimal MaxAmount = 1_000_000m;     // upper sanity limit
    public const decimal NearLimitFraction = 0.10m;  // "almost out" threshold

    /// <summary>
    /// Validates a monetary amount and returns it rounded to two decimal places.
    /// Throw <see cref="InvalidExpenseException"/> when the amount is not greater
    /// than zero, or is greater than <see cref="MaxAmount"/>.
    /// </summary>
    public static decimal ValidateAmount(decimal amount)
    {
        // TODO: guard clauses + decimal.Round(amount, 2)
        if (amount <= 0 || amount > MaxAmount)
        {
            throw new NotImplementedException("Invalid amount. Please enter a positive amount within the allowed limit.");
        }

        return decimal.Round(amount, 2);
    }

    /// <summary>
    /// Classifies a positive amount into a size band: "Micro" (&lt; 10),
    /// "Small" (&lt; 50), "Medium" (&lt; 200), otherwise "Large".
    /// Throw <see cref="InvalidExpenseException"/> for a non-positive amount.
    /// Hint: use a switch expression with relational patterns.
    /// </summary>
    public static string ClassifyAmount(decimal amount)
    {
        // TODO
        if (amount <= 0)
        {
            throw new NotImplementedException("Amount must be positive.");
        }

        return amount switch
        {
            < 10 => "Micro",
            < 50 => "Small",
            < 200 => "Medium",
            _ => "Large"
        };

        //old switch expression
        /*
        switch (amount)
        {
            case < 10:
                return "Micro";
            case < 50:
                return "Small";
            case < 200:
                return "Medium";
            default:
                return "Large";
        }       
          
         */
    }

    /// <summary>
    /// Maps free-text input to one of the five canonical category names
    /// (Food, Transport, Utilities, Entertainment, Other), case-insensitively
    /// and allowing the first-letter shortcut. Return <c>null</c> when the
    /// input is not recognised. Hint: a switch expression over the trimmed,
    /// lower-cased input, with "food" or "f" => "Food", etc.
    /// </summary>
    public static string? NormalizeCategory(string? input)
    {
        // TODO
        
        //throw new NotImplementedException();
        if (input == null)
        {
            return null;
        }

        string category = input.Trim().ToLowerInvariant();

        return category switch
        {
            "food" or "f" => "Food",
            "transport" or "t" => "Transport",
            "utilities" or "u" => "Utilities",
            "entertainment" or "e" => "Entertainment",
            "other" or "o" => "Other",
            _ => null
        };
    }

    //old switch expression
    /*
        switch (category)
        {
            case "food":
            case "f":
                return "Food";
            case "transport":
            case "t":
                return "Transport";
            case "utilities":
            case "u":
                return "Utilities";
            case "entertainment":
            case "e":
                return "Entertainment";
            case "other":
            case "o":
                return "Other";
            default:
                return null;
        }
     */

    /// <summary>
    /// Returns a budget-status message from the remaining funds and the limit:
    /// "OVER BUDGET" (remaining &lt; 0), "Almost out" (less than 10% of the
    /// limit remains), or "On track". Throw when the limit is not positive.
    /// </summary>
    public static string BudgetStatus(decimal remaining, decimal monthlyLimit)
{
        // TODO
        if (monthlyLimit <= 0)
        {
            throw new NotImplementedException(
                "Monthly budget must be positive.");
        }

        return remaining switch
        {
            < 0 => "OVER BUDGET",
            >= 0 when remaining < monthlyLimit * NearLimitFraction => "Almost out",
            _ => "On track"
        };
    }

/// <summary>
/// Formats an amount as currency using the default "$" symbol.
/// Implement this as an expression-bodied member that calls the overload.
/// </summary>
public static string FormatCurrency(decimal amount)
{
    // TODO: return FormatCurrency(amount, "$");
    throw new NotImplementedException();
}

/// <summary>
/// Formats an amount as currency using the given symbol, e.g. "$62.40".
/// </summary>
public static string FormatCurrency(decimal amount, string currencySymbol)
{
    // TODO: use a "0.00" format string
    throw new NotImplementedException();
}
}

/// <summary>
/// Custom exception for invalid expense data (Module 4: custom exceptions).
/// This one is provided complete — use it from the methods above.
/// </summary>
public sealed class InvalidExpenseException(string message) : Exception(message);
