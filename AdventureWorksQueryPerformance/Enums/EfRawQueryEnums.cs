namespace AdventureWorksQueryPerformance.Enums
{
    public enum EfRawQueryEnums
    {
        [EnumDescription("Raw query Get top 100 customers details")]
        RawGetTopHundred,

        [EnumDescription("Raw query Get sales performance between 01/01/2012 and 31/12/2014")]
        RawSalesPerformance,

        [EnumDescription("Raw query Get large data")]
        RawLargeData,

        [EnumDescription("Raw query Get large data greater than 4917 value")]
        RawLargeDataGreaterThan,

        [EnumDescription("Raw query Get large data greater than 4917 value in table with index")]
        RawLargeDataGreaterThanWithIndex
    }
}
