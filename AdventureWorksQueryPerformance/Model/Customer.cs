namespace AdventureWorksQueryPerformance.Model
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<SalesOrderHeader>? SalesOrderHeaders { get; set; }
    }
}
