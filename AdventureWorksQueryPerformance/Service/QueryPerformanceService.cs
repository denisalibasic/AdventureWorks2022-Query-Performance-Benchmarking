using AdventureWorksQueryPerformance.Request;
using MediatR;
using System.Diagnostics;

namespace AdventureWorksQueryPerformance.Service
{
    public class QueryPerformanceService
    {
        private readonly IMediator _mediator;
        private readonly ClearCacheService _clearCacheService;

        public QueryPerformanceService(IMediator mediator , ClearCacheService clearCacheService)
        {
            _mediator = mediator;
            _clearCacheService = clearCacheService;
        }

        public async Task RunQueriesSequentiallyAsync()
        {
            _clearCacheService.ClearCache();
            await ExecuteAndMeasureTimeAsync(new EFQueryRequest(), "EF Query");
            _clearCacheService.ClearCache();
            await ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest(), "Raw SQL Query");
            _clearCacheService.ClearCache();
            await ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest(), "Stored Procedure");
            _clearCacheService.ClearCache();
            await ExecuteAndMeasureTimeAsync(new BulkInsertRequest(), "Bulk Insert");
            _clearCacheService.ClearCache();
        }

        private async Task ExecuteAndMeasureTimeAsync(IRequest<Unit> request, string queryType)
        {
            var stopwatch = Stopwatch.StartNew();
            await _mediator.Send(request);
            stopwatch.Stop();
            Console.WriteLine($"{queryType} took {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
