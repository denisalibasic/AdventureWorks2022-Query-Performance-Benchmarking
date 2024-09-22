namespace AdventureWorksQueryPerformance.Queries.EFQuery.Results
{
    public class CustomerSpendingResult
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public DateTime OrderDate { get; set; }
        public short OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal TotalDue { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSalesForProduct { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
