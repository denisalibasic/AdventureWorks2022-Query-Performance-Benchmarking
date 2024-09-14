namespace AdventureWorksQueryPerformance.Model
{
    public class Product
    {
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public ICollection<SalesOrderDetail>? SalesOrderDetails { get; set; }
    }
}
