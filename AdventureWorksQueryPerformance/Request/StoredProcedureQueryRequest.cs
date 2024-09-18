using MediatR;

namespace AdventureWorksQueryPerformance.Request
{
    public class StoredProcedureQueryRequest : IRequest<Unit>
    {
        public string? QueryType { get; set; }
    }
}
