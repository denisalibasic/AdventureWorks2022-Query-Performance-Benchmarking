using AdventureWorksQueryPerformance.DBContext;
using AdventureWorksQueryPerformance.Queries.EFQuery;
using AdventureWorksQueryPerformance.Request;
using MediatR;

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
            if (request.QueryType == "Foreach")
            {
                await TopHundredCustomersSpendingEF.GetCustomerSpendingWithForeachAsync(_context);
            } else if (request.QueryType == "Optimized ex 1")
            {
                var topCustomersQuery = TopHundredCustomersSpendingEF.GetTopCustomersDetailedQuery(_context);
            }
            else if (request.QueryType == "Optimized ex 2")
            {
                var startDate = new DateTime(2012, 1, 1);
                var endDate = new DateTime(2014, 12, 31);
                var topCustomersQuery = TopHundredCustomersSpendingEF.GetSalesPerformanceAsync(_context, startDate, endDate);
            }
            return Unit.Value;
        }
    }
}
