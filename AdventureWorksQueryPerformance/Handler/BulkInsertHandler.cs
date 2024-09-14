using AdventureWorksQueryPerformance.Request;
using MediatR;
using Microsoft.Data.SqlClient;

namespace AdventureWorksQueryPerformance.Handler
{
    public class BulkInsertHandler : IRequestHandler<BulkInsertRequest, Unit>
    {
        private readonly string _connectionString;

        public BulkInsertHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Unit> Handle(BulkInsertRequest request, CancellationToken cancellationToken)
        {
            // TOADD Sample data to insert

            using var connection = new SqlConnection(_connectionString);
            // Bulk Insert
            return Unit.Value;
        }
    }
}
