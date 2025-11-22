using UserExpenseForecast.Models;
using UserExpenseForecast.Data;

namespace UserExpenseForecast.Services
{
    public class ExpenseService
    {
        private readonly ExpenseRepository _repository;

        public ExpenseService(ExpenseRepository repository)
        {
            _repository = repository;
        }

        // Додати витрату
        public void AddExpense(string category, decimal amount, DateTime date)
        {
            var expenses = _repository.GetAll();

            expenses.Add(new Expense(category, amount, date));

            _repository.SaveAll(expenses);
        }

        // Отримати витрати за певний місяць
        public decimal GetMonthlyTotal(DateTime month)
        {
            var expenses = _repository.GetAll();

            return expenses
                .Where(e => e.Date.Month == month.Month && e.Date.Year == month.Year)
                .Sum(e => e.Amount);
        }

        // Отримати всі витрати
        public List<Expense> GetAllExpenses()
        {
            return _repository.GetAll();
        }
    }
}
