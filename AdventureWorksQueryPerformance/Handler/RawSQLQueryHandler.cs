using AdventureWorksQueryPerformance.Queries.RawQuery;
using AdventureWorksQueryPerformance.Request;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AdventureWorksQueryPerformance.Handler
{
    public class RawSQLQueryHandler : IRequestHandler<RawSQLQueryRequest, Unit>
    {
        private readonly string _connectionString;

        public RawSQLQueryHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksDb");
        }

        public async Task<Unit> Handle(RawSQLQueryRequest request, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var query = TopHundredCustomersSpendingRawSQL.GetTopCustomersDetailedQuery();
            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {

            }

            return Unit.Value;
        }
    }
}
