using AdventureWorksQueryPerformance.Request;
using MediatR;
using System.Diagnostics;

namespace AdventureWorksQueryPerformance.Service
{
    public class QueryPerformanceService
    {
        private readonly IMediator _mediator;

        public QueryPerformanceService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task RunQueriesSequentiallyAsync()
        {
            await ExecuteAndMeasureTimeAsync(new EFQueryRequest(), "EF Query");
            await ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest(), "Raw SQL Query");
            await ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest(), "Stored Procedure");
            await ExecuteAndMeasureTimeAsync(new BulkInsertRequest(), "Bulk Insert");
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
