using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorksQueryPerformance.Queries.RawQuery
{
    public static class TopHundredCustomersSpendingRawSQL
    {
        public static string GetTopCustomersDetailedQuery()
        {
            return @"
                WITH TopCustomers AS (
                    SELECT TOP 100 CustomerID, SUM(TotalDue) AS TotalSpent
                    FROM Sales.SalesOrderHeader
                    GROUP BY CustomerID
                    ORDER BY TotalSpent DESC
                )
                SELECT 
                    c.CustomerID,
                    p.ProductID,
                    p.Name AS ProductName,
                    soh.OrderDate,
                    sod.OrderQty,
                    sod.UnitPrice,
                    sod.LineTotal,
                    soh.TotalDue,
                    COUNT(soh.SalesOrderID) OVER(PARTITION BY c.CustomerID) AS TotalOrders,
                    (SELECT SUM(sod2.LineTotal)
                     FROM Sales.SalesOrderDetail sod2
                     WHERE sod2.ProductID = p.ProductID) AS TotalSalesForProduct,
                    tc.TotalSpent
                FROM Sales.SalesOrderHeader soh
                JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
                JOIN Production.Product p ON sod.ProductID = p.ProductID
                JOIN Sales.Customer c ON soh.CustomerID = c.CustomerID
                JOIN TopCustomers tc ON c.CustomerID = tc.CustomerID
                WHERE soh.OrderDate BETWEEN '2012-01-01' AND '2014-12-31'
                ORDER BY tc.TotalSpent DESC";
        }
    }
}
