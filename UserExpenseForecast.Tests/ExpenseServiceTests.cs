using System;
using System.Collections.Generic;
using System.IO;
using UserExpenseForecast.Data;
using UserExpenseForecast.Models;
using UserExpenseForecast.Services;
using Xunit;

namespace UserExpenseForecast.Tests
{
    public class ExpenseServiceTests
    {
        private ExpenseRepository CreateCleanRepository(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            return new ExpenseRepository(fileName);
        }

        [Fact]
        public void AddExpense_SavesExpenseToRepository()
        {
            // Arrange
            var repo = CreateCleanRepository("test_expenses_add.json");
            var service = new ExpenseService(repo);

            // Act
            service.AddExpense("Їжа", 150m, new DateTime(2025, 1, 10));
            var all = service.GetAllExpenses();

            // Assert
            Assert.Single(all);
            Assert.Equal("Їжа", all[0].Category);
            Assert.Equal(150m, all[0].Amount);
            Assert.Equal(new DateTime(2025, 1, 10), all[0].Date);
        }

        [Fact]
        public void GetMonthlyTotal_ReturnsSumForGivenMonth()
        {
            // Arrange
            var repo = CreateCleanRepository("test_expenses_month1.json");
            var service = new ExpenseService(repo);

            var jan10 = new DateTime(2025, 1, 10);
            var jan15 = new DateTime(2025, 1, 15);
            var feb01 = new DateTime(2025, 2, 1);

            service.AddExpense("Їжа", 100m, jan10);
            service.AddExpense("Транспорт", 200m, jan15);
            service.AddExpense("Кава", 300m, feb01);

            // Act
            decimal totalJanuary = service.GetMonthlyTotal(new DateTime(2025, 1, 1));

            // Assert
            Assert.Equal(300m, totalJanuary); // 100 + 200
        }

        [Fact]
        public void GetMonthlyTotal_IgnoresOtherMonthsAndYears()
        {
            // Arrange
            var repo = CreateCleanRepository("test_expenses_month2.json");
            var service = new ExpenseService(repo);

            service.AddExpense("Їжа", 100m, new DateTime(2025, 3, 10));
            service.AddExpense("Їжа", 200m, new DateTime(2025, 4, 5));
            service.AddExpense("Їжа", 300m, new DateTime(2024, 3, 10)); // інший рік

            // Act
            decimal totalMarch2025 = service.GetMonthlyTotal(new DateTime(2025, 3, 1));

            // Assert
            Assert.Equal(100m, totalMarch2025);
        }

        [Fact]
        public void GetAllExpenses_ReturnsAllSavedExpenses()
        {
            // Arrange
            var repo = CreateCleanRepository("test_expenses_all.json");
            var service = new ExpenseService(repo);

            service.AddExpense("Їжа", 50m, new DateTime(2025, 1, 1));
            service.AddExpense("Кава", 30m, new DateTime(2025, 1, 2));
            service.AddExpense("Транспорт", 20m, new DateTime(2025, 1, 3));

            // Act
            var all = service.GetAllExpenses();

            // Assert
            Assert.Equal(3, all.Count);
        }
    }
}
