using UserExpenseForecast.Models;

namespace UserExpenseForecast.Services
{
    public class ForecastService
    {
        private readonly ExpenseService _expenseService;

        public ForecastService(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // Прогноз витрат на наступний день
        public decimal ForecastNextDay(int days = 7)
        {
            var allExpenses = _expenseService.GetAllExpenses();

            if (!allExpenses.Any())
            {
                // Немає витрат – немає з чого прогнозувати
                return 0;
            }

            // Беремо витрати за останні N днів
            DateTime today = DateTime.Now.Date;
            DateTime fromDate = today.AddDays(-days);

            var recentExpenses = allExpenses
                .Where(e => e.Date.Date >= fromDate && e.Date.Date <= today)
                .ToList();

            if (!recentExpenses.Any())
            {
                // За останні N днів немає витрат
                return 0;
            }

            // Рахуємо середнє значення витрат
            decimal total = recentExpenses.Sum(e => e.Amount);
            int count = recentExpenses.Count;

            decimal average = total / count;

            // Округлюємо до 2 знаків після коми
            return Math.Round(average, 2);
        }
    }
}
