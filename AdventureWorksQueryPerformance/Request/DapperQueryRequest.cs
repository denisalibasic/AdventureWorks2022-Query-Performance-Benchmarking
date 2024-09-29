using AdventureWorksQueryPerformance.Enums;
using MediatR;

namespace AdventureWorksQueryPerformance.Request
{
    public class DapperQueryRequest : IRequest<Unit>
    {
        public DapperQueryEnums QueryType { get; set; }
    }
}
