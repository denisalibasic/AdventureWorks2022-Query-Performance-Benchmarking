namespace AdventureWorksQueryPerformance.Model
{
    public class SalesOrderDetail
    {
        public int SalesOrderID { get; set; }
        public int ProductID { get; set; }
        public short OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public SalesOrderHeader? SalesOrderHeader { get; set; }
        public Product? Product { get; set; }
    }
}
