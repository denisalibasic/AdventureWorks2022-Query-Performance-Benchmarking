using AdventureWorksQueryPerformanceApi.Response;

namespace AdventureWorksQueryPerformanceApi
{
    public interface IQueryPerformanceApiAdapter
    {
        Task<List<QueryResult>> ExecuteQueries();
    }
}
