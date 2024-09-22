namespace AdventureWorksQueryPerformance.Enums
{
    public enum EfQueryEnums
    {
        [EnumDescription("Ef Foreach Get top 100 customers details")]
        EfForEach,

        [EnumDescription("EF Get top 100 customers details")]
        EfGetTopHundred,

        [EnumDescription("EF Get sales performance between 01/01/2012 and 31/12/2014")]
        EfSalesPerformance,

        [EnumDescription("EF Get large data")]
        EfLargeData,

        [EnumDescription("EF Get large data greater than 4917 value")]
        EfLargeDataGreaterThan,

        [EnumDescription("EF Get large data greater than 4917 value in table with index")]
        EfLargeDataGreaterThanWithIndex,

        [EnumDescription("EF Get large data greater than 1000 value in table with index")]
        EfLargeDataGreaterThanWithIndexSecond
    }
}
