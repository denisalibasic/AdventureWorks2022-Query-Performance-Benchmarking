namespace AdventureWorksQueryPerformance.Model
{
    public class SalesOrderHeader
    {
        public int SalesOrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalDue { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<SalesOrderDetail>? SalesOrderDetails { get; set; }
    }
}
