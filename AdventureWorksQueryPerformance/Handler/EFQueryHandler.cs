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
            } else
            {
                var topCustomersQuery = TopHundredCustomersSpendingEF.GetTopCustomersDetailedQuery(_context);
            }
            return Unit.Value;
        }
    }
}
