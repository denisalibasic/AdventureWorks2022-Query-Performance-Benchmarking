using AdventureWorksQueryPerformance.Request;
using MediatR;
using Microsoft.Data.SqlClient;

namespace AdventureWorksQueryPerformance.Handler
{
    public class RawSQLQueryHandler : IRequestHandler<RawSQLQueryRequest, Unit>
    {
        private readonly string _connectionString;

        public RawSQLQueryHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Unit> Handle(RawSQLQueryRequest request, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            // Raw SQL Query
            return Unit.Value;
        }
    }
}
