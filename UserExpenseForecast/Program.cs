using UserExpenseForecast.Data;
using UserExpenseForecast.Services;
using UserExpenseForecast.Models;
using System.Linq;
using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Фіксований список категорій витрат
string[] categories = new[]
{
    "Їжа",
    "Транспорт",
    "Кава",
    "Підписки",
    "Розваги",
    "Одяг",
    "Інше"
};

// Створення основних сервісів
var repository = new ExpenseRepository("expenses.json");
var expenseService = new ExpenseService(repository);
var forecastService = new ForecastService(expenseService);

// Головний цикл меню
while (true)
{
    Console.WriteLine("\n=== Система прогнозування витрат (FinTech) ===");
    Console.WriteLine("1. Додати витрату");
    Console.WriteLine("2. Показати всі витрати");
    Console.WriteLine("3. Показати витрати за поточний місяць");
    Console.WriteLine("4. Показати витрати за категорією");
    Console.WriteLine("5. Прогноз витрат на завтра");
    Console.WriteLine("6. Вийти");
    Console.Write("Ваш вибір: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddExpense(expenseService);
            break;

        case "2":
            ShowAllExpenses(expenseService);
            break;

        case "3":
            ShowMonthlyTotal(expenseService);
            break;

        case "4":
            ShowExpensesByCategory(expenseService);
            break;

        case "5":
            ShowForecast(forecastService);
            break;

        case "6":
            Console.WriteLine("До побачення!");
            return;

        default:
            Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
            break;
    }
}

// ===== ЛОКАЛЬНІ МЕТОДИ =====

void AddExpense(ExpenseService expenseService)
{
    Console.WriteLine("\nОберіть категорію витрати:");

    // Виводимо список категорій з номерами
    for (int i = 0; i < categories.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {categories[i]}");
    }

    Console.Write("Ваш вибір (номер): ");
    string? categoryChoice = Console.ReadLine();

    // Перевіряємо, що користувач ввів коректний номер
    if (!int.TryParse(categoryChoice, out int categoryIndex) ||
        categoryIndex < 1 || categoryIndex > categories.Length)
    {
        Console.WriteLine("Помилка: невірний номер категорії.");
        return;
    }

    // Обираємо категорію зі списку
    string category = categories[categoryIndex - 1];

    // Якщо обрано "Інше" – дозволяємо ввести свою назву
    if (category == "Інше")
    {
        Console.Write("Введіть свою категорію: ");
        string? customCategory = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(customCategory))
        {
            category = customCategory;
        }
    }

    Console.Write("Введіть суму витрати (наприклад, 150.50): ");
    string? amountInput = Console.ReadLine();

    if (!decimal.TryParse(amountInput, out decimal amount) || amount <= 0)
    {
        Console.WriteLine("Помилка: сума має бути додатнім числом.");
        return;
    }

    // ✅ Нове: дата витрати
    Console.Write("Введіть дату витрати у форматі dd.MM.yyyy або залиште порожнім для сьогодні: ");
    string? dateInput = Console.ReadLine();

    DateTime date;

    if (string.IsNullOrWhiteSpace(dateInput))
    {
        // Користувач нічого не ввів — беремо сьогодні
        date = DateTime.Now;
    }
    else
    {
        if (!DateTime.TryParseExact(
                dateInput,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            Console.WriteLine("Помилка: неправильний формат дати.");
            return;
        }
    }

    expenseService.AddExpense(category, amount, date);

    Console.WriteLine("✅ Витрату додано успішно.");
}

void ShowAllExpenses(ExpenseService expenseService)
{
    var expenses = expenseService.GetAllExpenses()
        .OrderBy(e => e.Date)
        .ToList();

    if (!expenses.Any())
    {
        Console.WriteLine("\nПоки що немає жодної витрати.");
        return;
    }

    Console.WriteLine("\n=== Усі витрати ===");

    foreach (var e in expenses)
    {
        Console.WriteLine($"{e.Date:dd.MM.yyyy HH:mm} | {e.Category,-15} | {e.Amount} грн");
    }
}

void ShowMonthlyTotal(ExpenseService expenseService)
{
    DateTime now = DateTime.Now;
    decimal total = expenseService.GetMonthlyTotal(now);

    Console.WriteLine($"\nСумарні витрати за {now:MMMM yyyy}: {total} грн");
}

void ShowExpensesByCategory(ExpenseService expenseService)
{
    Console.WriteLine("\nОберіть категорію для фільтрації:");

    for (int i = 0; i < categories.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {categories[i]}");
    }

    Console.Write("Ваш вибір (номер): ");
    string? categoryChoice = Console.ReadLine();

    if (!int.TryParse(categoryChoice, out int categoryIndex) ||
        categoryIndex < 1 || categoryIndex > categories.Length)
    {
        Console.WriteLine("Помилка: невірний номер категорії.");
        return;
    }

    string category = categories[categoryIndex - 1];

    if (category == "Інше")
    {
        Console.Write("Введіть свою категорію: ");
        string? customCategory = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(customCategory))
        {
            category = customCategory;
        }
    }

    var expenses = expenseService
        .GetAllExpenses()
        .Where(e => string.Equals(e.Category, category, StringComparison.OrdinalIgnoreCase))
        .OrderBy(e => e.Date)
        .ToList();

    if (!expenses.Any())
    {
        Console.WriteLine($"\nНемає витрат у категорії \"{category}\".");
        return;
    }

    Console.WriteLine($"\n=== Витрати у категорії \"{category}\" ===");

    foreach (var e in expenses)
    {
        Console.WriteLine($"{e.Date:dd.MM.yyyy HH:mm} | {e.Category,-15} | {e.Amount} грн");
    }

    decimal total = expenses.Sum(e => e.Amount);
    Console.WriteLine($"\nРазом у цій категорії: {total} грн");
}

void ShowForecast(ForecastService forecastService)
{
    decimal forecast = forecastService.ForecastNextDay();

    Console.WriteLine($"\nПрогноз витрат на завтра: {forecast} грн");
}
