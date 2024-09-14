using AdventureWorksQueryPerformance.Request;
using MediatR;
using Microsoft.Data.SqlClient;

namespace AdventureWorksQueryPerformance.Handler
{
    public class StoredProcedureQueryHandler : IRequestHandler<StoredProcedureQueryRequest, Unit>
    {
        private readonly string _connectionString;

        public StoredProcedureQueryHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Unit> Handle(StoredProcedureQueryRequest request, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            // Stored Procedure Call
            return Unit.Value;
        }
    }
}
