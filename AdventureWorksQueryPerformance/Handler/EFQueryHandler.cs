using AdventureWorksQueryPerformance.DBContext;
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
            // Example EF Query
            return Unit.Value;
        }
    }
}
