using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorksQueryPerformance.Queries.RawQuery
{
    public static class EfRawQueries
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

        public static string GetSalesPerformance()
        {
            return @"
                CREATE TABLE #SalesPerformance (
                Rank INT,
                Year INT,
                ProductID INT,
                TotalSales DECIMAL(18, 2),
                TotalOrders INT
            );

            INSERT INTO #SalesPerformance (Rank, Year, ProductID, TotalSales, TotalOrders)
            SELECT
                ROW_NUMBER() OVER (PARTITION BY YEAR(soh.OrderDate) ORDER BY SUM(sod.LineTotal) DESC) AS Rank,
                YEAR(soh.OrderDate) AS Year,
                sod.ProductID AS ProductID,
                SUM(sod.LineTotal) AS TotalSales,
                COUNT(soh.SalesOrderID) AS TotalOrders
            FROM Sales.SalesOrderHeader soh
            JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
            WHERE soh.OrderDate BETWEEN @StartDate AND @EndDate
            GROUP BY YEAR(soh.OrderDate), sod.ProductID;

            SELECT
                Rank,
                Year,
                ProductID,
                TotalSales,
                TotalOrders
            FROM #SalesPerformance
            ORDER BY TotalSales DESC;

            DROP TABLE #SalesPerformance;";
        }

        public static string GetSalesLargeDataAllRows()
        {
            return @"select * from LargeDataTest";
        }

        public static string GetSalesLargeDataByValue()
        {
            return @"
                select * from [dbo].[LargeDataTest]
	            where Value > 4917";
        }

        public static string GetSalesLargeDataByValueWithIndex()
        {
            return @"
                select * from [dbo].[LargeDataTestWithIndex]
	            where Value > 4917";
        }

        public static string GetSalesLargeDataByValueWithIndexSecond()
        {
            return @"
                select * from [dbo].[LargeDataTestWithIndex]
	            where Value > 1000";
        }
    }
}
