using System.Text.Json;
using UserExpenseForecast.Models;

namespace UserExpenseForecast.Data
{
    public class ExpenseRepository
    {
        private readonly string _filePath;

        public ExpenseRepository(string filePath)
        {
            _filePath = filePath;

            // Якщо файл не існує – створюємо його
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        // Отримати всі витрати
        public List<Expense> GetAll()
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Expense>>(json)
                   ?? new List<Expense>();
        }

        // Зберегти всі витрати
        public void SaveAll(List<Expense> expenses)
        {
            string json = JsonSerializer.Serialize(expenses, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_filePath, json);
        }
    }
}
