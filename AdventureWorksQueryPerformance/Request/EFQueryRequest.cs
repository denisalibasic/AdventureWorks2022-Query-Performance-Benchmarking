using AdventureWorksQueryPerformance.Enums;
using MediatR;

namespace AdventureWorksQueryPerformance.Request
{
    public class EFQueryRequest : IRequest<Unit>
    {
        public EfQueryEnums QueryType { get; set; }
    }
}
