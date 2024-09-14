using AdventureWorksQueryPerformance.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorksQueryPerformance.Queries.EFQuery
{
    public static class TopHundredCustomersSpendingEF
    {
        public static IQueryable<CustomerSpendingDto> GetTopCustomersDetailedQuery(AdventureWorksDbContext context)
        {
            var topCustomers = context.SalesOrderHeaders

                .GroupBy(soh => soh.CustomerID)
                .OrderByDescending(g => g.Sum(soh => soh.TotalDue))
                .Take(100)
                .Select(g => new
                {
                    CustomerID = g.Key,
                    TotalSpent = g.Sum(soh => soh.TotalDue)
                }).ToList();

            var query = from soh in context.SalesOrderHeaders
                        join sod in context.SalesOrderDetails on soh.SalesOrderID equals sod.SalesOrderID
                        join p in context.Products on sod.ProductID equals p.ProductID
                        join c in context.Customers on soh.CustomerID equals c.CustomerID
                        join tc in topCustomers on c.CustomerID equals tc.CustomerID
                        where soh.OrderDate >= new DateTime(2012, 1, 1) && soh.OrderDate <= new DateTime(2014, 12, 31)
                        select new CustomerSpendingDto
                        {
                            CustomerID = c.CustomerID,
                            ProductID = p.ProductID,
                            ProductName = p.Name,
                            OrderDate = soh.OrderDate,
                            OrderQty = sod.OrderQty,
                            UnitPrice = sod.UnitPrice,
                            LineTotal = sod.LineTotal,
                            TotalDue = soh.TotalDue,
                            TotalOrders = context.SalesOrderHeaders.Count(s => s.CustomerID == c.CustomerID),
                            TotalSalesForProduct = context.SalesOrderDetails
                                .Where(sod2 => sod2.ProductID == p.ProductID)
                                .Sum(sod2 => sod2.LineTotal),
                            TotalSpent = tc.TotalSpent
                        };

            return query;
        }

        public static async Task<List<CustomerSpendingDto>> GetCustomerSpendingWithForeachAsync(AdventureWorksDbContext context)
        {
            // TODO
            return new List<CustomerSpendingDto>();
        }
    }


    public class CustomerSpendingDto
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
