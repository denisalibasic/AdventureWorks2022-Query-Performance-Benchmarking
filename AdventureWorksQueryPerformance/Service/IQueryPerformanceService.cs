using AdventureWorksQueryPerformance.Result;

namespace AdventureWorksQueryPerformance.Service
{
    public interface IQueryPerformanceService
    {
        Task<List<TaskResult>> RunQueriesSequentially();
    }
}
