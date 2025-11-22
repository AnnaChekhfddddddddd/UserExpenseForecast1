using System;
using System.IO;
using UserExpenseForecast.Data;
using UserExpenseForecast.Services;
using Xunit;

namespace UserExpenseForecast.Tests
{
    public class ForecastServiceTests
    {
        private ExpenseRepository CreateCleanRepository(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            return new ExpenseRepository(fileName);
        }

        [Fact]
        public void ForecastNextDay_ReturnsAverageForLast7Days()
        {
            // Arrange
            var repo = CreateCleanRepository("test_forecast_avg.json");
            var expenseService = new ExpenseService(repo);

            DateTime today = DateTime.Now.Date;

            expenseService.AddExpense("Їжа", 100m, today.AddDays(-1));
            expenseService.AddExpense("Транспорт", 200m, today.AddDays(-2));
            expenseService.AddExpense("Кава", 300m, today.AddDays(-3));

            var forecastService = new ForecastService(expenseService);

            // Act
            decimal forecast = forecastService.ForecastNextDay(days: 7);

            // Assert
            Assert.Equal(200m, forecast); // (100 + 200 + 300) / 3
        }

        [Fact]
        public void ForecastNextDay_ReturnsZero_WhenNoExpenses()
        {
            // Arrange
            var repo = CreateCleanRepository("test_forecast_empty.json");
            var expenseService = new ExpenseService(repo);
            var forecastService = new ForecastService(expenseService);

            // Act
            decimal forecast = forecastService.ForecastNextDay();

            // Assert
            Assert.Equal(0m, forecast);
        }

        [Fact]
        public void ForecastNextDay_ReturnsZero_WhenNoExpensesInWindow()
        {
            // Arrange
            var repo = CreateCleanRepository("test_forecast_window.json");
            var expenseService = new ExpenseService(repo);
            var forecastService = new ForecastService(expenseService);

            DateTime today = DateTime.Now.Date;

            // Витрата була 30 днів тому – за останні 7 днів її не буде
            expenseService.AddExpense("Їжа", 500m, today.AddDays(-30));

            // Act
            decimal forecast = forecastService.ForecastNextDay(days: 7);

            // Assert
            Assert.Equal(0m, forecast);
        }

        [Fact]
        public void ForecastNextDay_UsesCustomWindowSize()
        {
            // Arrange
            var repo = CreateCleanRepository("test_forecast_custom.json");
            var expenseService = new ExpenseService(repo);
            var forecastService = new ForecastService(expenseService);

            DateTime today = DateTime.Now.Date;

            // за останні 3 дні: суми 10, 20, 30
            expenseService.AddExpense("Їжа", 10m, today.AddDays(-1));
            expenseService.AddExpense("Їжа", 20m, today.AddDays(-2));
            expenseService.AddExpense("Їжа", 30m, today.AddDays(-3));

            // старі витрати — за межами вікна
            expenseService.AddExpense("Їжа", 1000m, today.AddDays(-10));

            // Act
            decimal forecast = forecastService.ForecastNextDay(days: 3);

            // Assert
            Assert.Equal(20m, forecast); // (10 + 20 + 30) / 3
        }
    }
}
