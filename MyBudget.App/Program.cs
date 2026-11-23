// =====================================================================
//  MyBudget — Assignment 2 entry point (composition root).
//
//  >>> YOU WRITE THE DEPENDENCY-INJECTION WIRING HERE (Module 8). <<<
//
//  The ConsoleApp UI is provided and depends only on the service
//  abstractions. Register your implementations with the container so that
//  ConsoleApp can be resolved. See the "Build specification" in the brief.
// =====================================================================
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBudget.App;
using MyBudget.Core;
using MyBudget.Data;

var builder = Host.CreateApplicationBuilder(args);

// Define the JSON file location.
string dataPath = Path.Combine(AppContext.BaseDirectory, "expenses.json");


// TODO (Module 8): register your services against their interfaces so the
// container can construct ConsoleApp. You will need, for example:
//   - IExpenseStore       -> JsonExpenseStore(dataPath)
//   - IExpenseRepository  -> ExpenseRepository
//   - IBudgetService      -> BudgetService
//   - ConsoleApp          (the UI, so it can be resolved below)
// Choose appropriate service lifetimes (singleton / scoped / transient).

// Register the JSON storage service.
// Singleton: uses the same file path throughout the application.
builder.Services.AddSingleton<IExpenseStore>(
    new JsonExpenseStore(dataPath));

// Register the expense repository.
// Singleton: keeps the same expense collection during execution.
builder.Services.AddSingleton<IExpenseRepository, ExpenseRepository>();

// Register the monthly budget service.
// Singleton: maintains the budget state during execution.
builder.Services.AddSingleton<IBudgetService, BudgetService>();

// Register the console user interface.
builder.Services.AddTransient<ConsoleApp>();

// Build the host and run the application.

using IHost host = builder.Build();

host.Services.GetRequiredService<ConsoleApp>().Run();
