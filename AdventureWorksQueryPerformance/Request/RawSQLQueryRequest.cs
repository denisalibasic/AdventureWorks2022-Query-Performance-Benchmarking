using AdventureWorksQueryPerformance.Enums;
using MediatR;

namespace AdventureWorksQueryPerformance.Request
{
    public class RawSQLQueryRequest : IRequest<Unit>
    {
        public EfRawQueryEnums QueryType { get; set; }
    }
}
