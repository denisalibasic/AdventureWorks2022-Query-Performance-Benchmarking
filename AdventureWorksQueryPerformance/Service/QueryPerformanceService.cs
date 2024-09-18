using AdventureWorksQueryPerformance.Request;
using MediatR;
using System.Diagnostics;
using System.Text.Json;

namespace AdventureWorksQueryPerformance.Service
{
    public class QueryPerformanceService
    {
        private readonly IMediator _mediator;
        private readonly ClearCacheService _clearCacheService;
        private readonly List<TaskResult> _results = new();

        public QueryPerformanceService(IMediator mediator , ClearCacheService clearCacheService)
        {
            _mediator = mediator;
            _clearCacheService = clearCacheService;
        }

        public async Task RunQueriesSequentiallyAsync()
        {
            _clearCacheService.ClearCache();
            _results.Clear();

            var tasks = new List<Task>
            {
                ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = "SP ex 1" } , "Stored Procedure ex 1"),
                //ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = "SP Cursor" } , "Stored Procedure cursor"),
                ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest(), "EF Raw SQL Query"),
                //ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = "Foreach" }, "EF Foreach Query"),
                ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = "Optimized ex 1" }, "EF Optimized Query ex 1"),

            };
            await Task.WhenAll(tasks);

            var secondBatchTasks = new List<Task>
            {
                ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = "SP ex 2" }, "Stored Procedure ex 2"),
                ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = "Optimized ex 2" }, "EF Optimized Query ex 2")
            };

            await Task.WhenAll(secondBatchTasks);

            DisplayResultsAsJson();
        }

        private async Task ExecuteAndMeasureTimeAsync(IRequest<Unit> request, string queryType)
        {
            Console.WriteLine($"{queryType} started...");
            var stopwatch = Stopwatch.StartNew();
            await _mediator.Send(request);
            stopwatch.Stop();

            var elapsedTime = stopwatch.ElapsedMilliseconds;
            _results.Add(new TaskResult
            {
                TaskName = queryType,
                ElapsedMilliseconds = elapsedTime
            });

            Console.WriteLine($"{queryType} took {elapsedTime} ms");
        }

        private void DisplayResultsAsJson()
        {
            var json = JsonSerializer.Serialize(_results, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Query performance results:");
            Console.WriteLine(json);
        }
    }

    public class TaskResult
    {
        public string TaskName { get; set; } = string.Empty;
        public long ElapsedMilliseconds { get; set; }
    }
}
