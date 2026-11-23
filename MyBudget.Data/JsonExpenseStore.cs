
using System.Text.Json;
using MyBudget.Core;

namespace MyBudget.Data;

// Saves and loads expenses using a JSON file.
public class JsonExpenseStore : IExpenseStore
{
    private readonly string _path;

    // Receives the file path where expenses will be stored.
    public JsonExpenseStore(string path)
    {
        _path = path;
    }


    // Loads existing expenses from the JSON file.
    public IReadOnlyList<Expense> Load()
    {
        // Return an empty list if the file does not exist.
        if (!File.Exists(_path))
        {
            return new List<Expense>();
        }

        string json = File.ReadAllText(_path);

        // Return an empty list if the file contains no data.
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Expense>();
        }

        // Deserialize JSON into a list of expense objects.
        return JsonSerializer.Deserialize<List<Expense>>(json)
               ?? new List<Expense>();
    }


    // Saves the expenses to a formatted JSON file.
    public void Save(IEnumerable<Expense> expenses)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // Convert expense objects into JSON.
        string json = JsonSerializer.Serialize(expenses, options);

        // Write the JSON content to the specified file.
        File.WriteAllText(_path, json);
    }
}
