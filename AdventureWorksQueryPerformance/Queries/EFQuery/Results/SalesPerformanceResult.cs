namespace AdventureWorksQueryPerformance.Queries.EFQuery.Results
{
    public class SalesPerformanceResult
    {
        public int Rank { get; set; }
        public int Year { get; set; }
        public int ProductID { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
    }
}
