namespace AdventureWorksQueryPerformance.Enums
{
    public enum SpQueryEnums
    {
        [EnumDescription("Stored Procedure Get top 100 customers details")]
        SpGetTopHundred,

        [EnumDescription("Stored Procedure Cursor Get top 100 customers details")]
        SpCursor,

        [EnumDescription("Stored Procedure Get sales performance between 01/01/2012 and 31/12/2014")]
        SpSalesPerformance,

        [EnumDescription("Stored Procedure Get large data")]
        SpLargeData,

        [EnumDescription("Stored Procedure Get large data greater than 4917 value")]
        SpLargeDataGreaterThan,

        [EnumDescription("Stored Procedure Get large data greater than 4917 value in table with index")]
        SpLargeDataGreaterThanWithIndex,

        [EnumDescription("Stored Procedure Get large data greater than 1000 value in table with index")]
        SpLargeDataGreaterThanWithIndexSecond
    }
}
