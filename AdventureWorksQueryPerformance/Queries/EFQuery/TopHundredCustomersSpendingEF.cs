﻿using AdventureWorksQueryPerformance.DBContext;
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
        public static async Task<List<CustomerSpendingDto>> GetTopCustomersDetailedQuery(AdventureWorksDbContext context)
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

            return await query.ToListAsync();
        }

        public static async Task<List<CustomerSpendingDto>> GetCustomerSpendingWithForeachAsync(AdventureWorksDbContext context)
        {
            var customerSpendings = new List<CustomerSpendingDto>();

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
                        customerSpendings.Add(new CustomerSpendingDto
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

    public class SalesPerformanceResult
    {
        public int Rank { get; set; }
        public int Year { get; set; }
        public int ProductID { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
    }
}
