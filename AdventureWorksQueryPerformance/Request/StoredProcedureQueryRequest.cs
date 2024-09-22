using AdventureWorksQueryPerformance.Enums;
using MediatR;

namespace AdventureWorksQueryPerformance.Request
{
    public class StoredProcedureQueryRequest : IRequest<Unit>
    {
        public SpQueryEnums QueryType { get; set; }
    }
}
