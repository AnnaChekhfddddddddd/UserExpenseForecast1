namespace UserExpenseForecast.Models
{
    public class Expense
    {
        public DateTime Date { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }

        public Expense() {}

        public Expense(string category, decimal amount, DateTime date)
        {
            Category = category;
            Amount = amount;
            Date = date;
            
        }
    }
}
