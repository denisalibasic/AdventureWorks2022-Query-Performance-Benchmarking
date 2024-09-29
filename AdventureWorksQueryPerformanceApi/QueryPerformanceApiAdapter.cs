using AdventureWorksQueryPerformance.Service;
using AdventureWorksQueryPerformanceApi.Response;
using Serilog;
using System.Text.Json;

namespace AdventureWorksQueryPerformanceApi
{
    public class QueryPerformanceApiAdapter : IQueryPerformanceApiAdapter
    {
        private readonly IQueryPerformanceService _queryPerformanceService;

        public QueryPerformanceApiAdapter(IQueryPerformanceService queryPerformanceService)
        {
            _queryPerformanceService = queryPerformanceService;
        }

        public async Task<List<QueryResult>> ExecuteQueries()
        {
            try
            {
                var taskResult = await _queryPerformanceService.RunQueriesSequentially();
                string jsonResult = JsonSerializer.Serialize(taskResult);
                var queryResult = JsonSerializer.Deserialize<List<QueryResult>>(jsonResult);
                return queryResult ?? new List<QueryResult>();
            }
            catch (JsonException jsonEx)
            {
                Log.Error($"JSON Error: {jsonEx.Message}");
                return new List<QueryResult>();
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while executing queries: {ex.Message}");
                return new List<QueryResult>();
            }
        }
    }
}
