using MediatR;

namespace AdventureWorksQueryPerformance.Request
{
    public class EFQueryRequest : IRequest<Unit>
    {
        public string? QueryType { get; set; }
    }
}
