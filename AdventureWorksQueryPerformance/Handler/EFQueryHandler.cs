using AdventureWorksQueryPerformance.DBContext;
using AdventureWorksQueryPerformance.Enums;
using AdventureWorksQueryPerformance.Queries.EFQuery;
using AdventureWorksQueryPerformance.Request;
using MediatR;
using Serilog;

namespace AdventureWorksQueryPerformance.Handler
{
    public class EFQueryHandler : IRequestHandler<EFQueryRequest, Unit>
    {
        private readonly AdventureWorksDbContext _context;

        public EFQueryHandler(AdventureWorksDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EFQueryRequest request, CancellationToken cancellationToken)
        {
            if (request.QueryType == EfQueryEnums.EfForEach)
            {
                Log.Information("Executing EF Foreach query...");
                await EfQueries.GetCustomerSpendingWithForeachAsync(_context);
            } else if (request.QueryType == EfQueryEnums.EfGetTopHundred)
            {
                Log.Information("Executing EF Get top 100 customers details...");
                var topCustomersQuery = EfQueries.GetTopCustomersDetailedQuery(_context);
            } else if (request.QueryType == EfQueryEnums.EfSalesPerformance)
            {
                Log.Information("Executing EF Get sales performance between 01/01/2012 and 31/12/2014...");
                var startDate = new DateTime(2012, 1, 1);
                var endDate = new DateTime(2014, 12, 31);
                var topCustomersQuery = EfQueries.GetSalesPerformanceAsync(_context, startDate, endDate);
            } else if(request.QueryType == EfQueryEnums.EfLargeData)
            {
                Log.Information("Executing EF Get large data...");
                await EfQueries.GetLargeDataAsync(_context);
            }
            else if (request.QueryType == EfQueryEnums.EfLargeDataGreaterThan)
            {
                Log.Information("Executing EF Get large data greater than 4917 value...");
                await EfQueries.GetLargeDataByValueAsync(_context);
            }
            else if (request.QueryType == EfQueryEnums.EfLargeDataGreaterThanWithIndex)
            {
                Log.Information("Executing EF Get large data greater than 4917 value in table with index...");
                await EfQueries.GetLargeDataByValueWithIndexAsync(_context);
            }
            return Unit.Value;
        }
    }
}
