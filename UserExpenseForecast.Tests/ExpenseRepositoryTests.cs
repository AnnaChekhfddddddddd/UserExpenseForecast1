using System;
using System.Collections.Generic;
using System.IO;
using UserExpenseForecast.Data;
using UserExpenseForecast.Models;
using Xunit;

namespace UserExpenseForecast.Tests
{
    public class ExpenseRepositoryTests
    {
        private string CreateTempFileName(string name) => $"repo_{name}.json";

        [Fact]
        public void GetAll_ReturnsEmptyList_WhenFileIsEmpty()
        {
            // Arrange
            string fileName = CreateTempFileName("empty");
            if (File.Exists(fileName))
                File.Delete(fileName);

            var repo = new ExpenseRepository(fileName);

            // Act
            var all = repo.GetAll();

            // Assert
            Assert.NotNull(all);
            Assert.Empty(all);
        }

        [Fact]
        public void SaveAll_ThenGetAll_ReturnsSameData()
        {
            // Arrange
            string fileName = CreateTempFileName("save_get");
            if (File.Exists(fileName))
                File.Delete(fileName);

            var repo = new ExpenseRepository(fileName);

            var list = new List<Expense>
            {
                new Expense("Їжа", 100m, new DateTime(2025, 1, 1)),
                new Expense("Кава", 50m, new DateTime(2025, 1, 2))
            };

            // Act
            repo.SaveAll(list);
            var loaded = repo.GetAll();

            // Assert
            Assert.Equal(2, loaded.Count);
            Assert.Equal("Їжа", loaded[0].Category);
            Assert.Equal(100m, loaded[0].Amount);
            Assert.Equal("Кава", loaded[1].Category);
            Assert.Equal(50m, loaded[1].Amount);
        }
    }
}
