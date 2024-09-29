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
                Log.Information("Executing " + EfQueryEnums.EfForEach.GetDescription());
                await EfQueries.GetCustomerSpendingWithForeachAsync(_context);
            } else if (request.QueryType == EfQueryEnums.EfGetTopHundred)
            {
                Log.Information("Executing " + EfQueryEnums.EfGetTopHundred.GetDescription());
                var topCustomersQuery = EfQueries.GetTopCustomersDetailedQuery(_context);
            } else if (request.QueryType == EfQueryEnums.EfSalesPerformance)
            {
                Log.Information("Executing " + EfQueryEnums.EfSalesPerformance.GetDescription());
                var startDate = new DateTime(2012, 1, 1);
                var endDate = new DateTime(2014, 12, 31);
                var topCustomersQuery = EfQueries.GetSalesPerformanceAsync(_context, startDate, endDate);
            } else if(request.QueryType == EfQueryEnums.EfLargeData)
            {
                Log.Information("Executing " + EfQueryEnums.EfLargeData.GetDescription());
                await EfQueries.GetLargeDataAsync(_context);
            }
            else if (request.QueryType == EfQueryEnums.EfLargeDataGreaterThan)
            {
                Log.Information("Executing " + EfQueryEnums.EfLargeDataGreaterThan.GetDescription());
                await EfQueries.GetLargeDataByValueAsync(_context);
            }
            else if (request.QueryType == EfQueryEnums.EfLargeDataGreaterThanWithIndex)
            {
                Log.Information("Executing " + EfQueryEnums.EfLargeDataGreaterThanWithIndex.GetDescription());
                var minValue = 4917;
                await EfQueries.GetLargeDataByValueWithIndexAsync(_context, minValue);
            }
            else if (request.QueryType == EfQueryEnums.EfLargeDataGreaterThanWithIndexSecond)
            {
                Log.Information("Executing " + EfQueryEnums.EfLargeDataGreaterThanWithIndexSecond.GetDescription());
                var minValue = 1000;
                await EfQueries.GetLargeDataByValueWithIndexAsync(_context, minValue);
            }

            return Unit.Value;
        }
    }
}
