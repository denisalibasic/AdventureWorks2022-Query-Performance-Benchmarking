using AdventureWorksQueryPerformance.DBContext;
using AdventureWorksQueryPerformance.Queries.EFQuery.Results;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksQueryPerformance.Queries.EFQuery
{
    public static class EfQueries
    {
        public static async Task<List<CustomerSpendingResult>> GetTopCustomersDetailedQuery(AdventureWorksDbContext context)
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
                        select new CustomerSpendingResult
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

            return await query.ToListAsync();
        }

        public static async Task<List<CustomerSpendingResult>> GetCustomerSpendingWithForeachAsync(AdventureWorksDbContext context)
        {
            var customerSpendings = new List<CustomerSpendingResult>();

            var startDate = new DateTime(2012, 1, 1);
            var endDate = new DateTime(2014, 12, 31);

            var topCustomers = await context.SalesOrderHeaders
                .GroupBy(soh => soh.CustomerID)
                .OrderByDescending(g => g.Sum(soh => soh.TotalDue))
                .Take(100)
                .Select(g => new
                {
                    CustomerID = g.Key,
                    TotalSpent = g.Sum(soh => soh.TotalDue)
                })
                .ToListAsync();

            foreach (var customer in topCustomers)
            {
                var salesOrders = await context.SalesOrderHeaders
                    .Where(soh => soh.CustomerID == customer.CustomerID)
                    .Where(soh => soh.OrderDate >= startDate && soh.OrderDate <= endDate)
                    .ToListAsync();

                foreach (var soh in salesOrders)
                {
                    var salesOrderDetails = await context.SalesOrderDetails
                        .Where(sod => sod.SalesOrderID == soh.SalesOrderID)
                        .Join(context.Products, sod => sod.ProductID, p => p.ProductID, (sod, p) => new
                        {
                            sod.ProductID,
                            p.Name,
                            sod.OrderQty,
                            sod.UnitPrice,
                            sod.LineTotal
                        })
                        .ToListAsync();

                    foreach (var sod in salesOrderDetails)
                    {
                        customerSpendings.Add(new CustomerSpendingResult
                        {
                            CustomerID = customer.CustomerID,
                            ProductID = sod.ProductID,
                            ProductName = sod.Name,
                            OrderDate = soh.OrderDate,
                            OrderQty = sod.OrderQty,
                            UnitPrice = sod.UnitPrice,
                            LineTotal = sod.LineTotal,
                            TotalDue = soh.TotalDue,
                            TotalOrders = await context.SalesOrderHeaders.CountAsync(s => s.CustomerID == customer.CustomerID), // Inefficient count
                            TotalSalesForProduct = await context.SalesOrderDetails
                                .Where(sod2 => sod2.ProductID == sod.ProductID)
                                .SumAsync(sod2 => sod2.LineTotal),
                            TotalSpent = customer.TotalSpent
                        });
                    }
                }
            }

            return customerSpendings;
        }

        public static async Task<List<SalesPerformanceResult>> GetSalesPerformanceAsync(AdventureWorksDbContext context, DateTime startDate, DateTime endDate)
        {
            var result = await context.SalesOrderHeaders
                .Where(soh => soh.OrderDate >= startDate && soh.OrderDate <= endDate)
                .Join(context.SalesOrderDetails,
                      soh => soh.SalesOrderID,
                      sod => sod.SalesOrderID,
                      (soh, sod) => new { soh, sod })
                .GroupBy(g => new { Year = g.soh.OrderDate.Year, g.sod.ProductID })
                .Select(grp => new
                {
                    Year = grp.Key.Year,
                    ProductID = grp.Key.ProductID,
                    TotalSales = grp.Sum(x => x.sod.LineTotal),
                    TotalOrders = grp.Count()
                })
                .OrderByDescending(x => x.TotalSales)
                .ToListAsync();

            var rankedResult = result
                .Select((item, index) => new SalesPerformanceResult
                {
                    Rank = index + 1,
                    Year = item.Year,
                    ProductID = item.ProductID,
                    TotalSales = item.TotalSales,
                    TotalOrders = item.TotalOrders
                })
                .ToList();

            return rankedResult;
        }

        public static async Task<List<LargeDataTestResult>> GetLargeDataAsync(AdventureWorksDbContext context)
        {
            return await context.LargeDataTests
                .Select(item => new LargeDataTestResult
                {
                    Id = item.Id,
                    Date = item.Date,
                    Name = item.Name,
                    Value = item.Value
                }).ToListAsync();
        }

        public static async Task<List<LargeDataTestResult>> GetLargeDataByValueAsync(AdventureWorksDbContext context)
        {
            return await context.LargeDataTests
                .Where(item => item.Value > 4917)
                .Select(item => new LargeDataTestResult
                {
                    Id = item.Id,
                    Date = item.Date,
                    Name = item.Name,
                    Value = item.Value
                }).ToListAsync();
        }

        public static async Task<List<LargeDataTestResult>> GetLargeDataByValueWithIndexAsync(AdventureWorksDbContext context, Decimal minValue)
        {
            return await context.LargeDataTestWithIndices
                .Where(item => item.Value > minValue)
                .Select(item => new LargeDataTestResult
                {
                    Id = item.Id,
                    Date = item.Date,
                    Name = item.Name,
                    Value = item.Value
                }).ToListAsync();
        }
    }
}
