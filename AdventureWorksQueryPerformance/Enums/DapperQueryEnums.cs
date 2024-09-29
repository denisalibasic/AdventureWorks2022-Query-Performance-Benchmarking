namespace AdventureWorksQueryPerformance.Enums
{
    public enum DapperQueryEnums
    {
        [EnumDescription("Dapper Get top 100 customers details")]
        DapperGetTopHundred,

        [EnumDescription("Dapper Get sales performance between 01/01/2012 and 31/12/2014")]
        DapperSalesPerformance,

        [EnumDescription("Dapper Get large data")]
        DapperLargeData,

        [EnumDescription("Dapper Get large data greater than 4917 value")]
        DapperLargeDataGreaterThan,

        [EnumDescription("Dapper Get large data greater than 4917 value in table with index")]
        DapperLargeDataGreaterThanWithIndex,

        [EnumDescription("Dapper Get large data greater than 1000 value in table with index")]
        DapperLargeDataGreaterThanWithIndexSecond
    }
}
