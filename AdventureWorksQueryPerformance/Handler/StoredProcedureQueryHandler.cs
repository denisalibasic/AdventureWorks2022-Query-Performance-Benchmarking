using AdventureWorksQueryPerformance.Request;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AdventureWorksQueryPerformance.Handler
{
    public class StoredProcedureQueryHandler : IRequestHandler<StoredProcedureQueryRequest, Unit>
    {
        private readonly string _connectionString;

        public StoredProcedureQueryHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksDb");
        }

        public async Task<Unit> Handle(StoredProcedureQueryRequest request, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("GetTopCustomersDetailed", connection);
            command.CommandType = CommandType.StoredProcedure;
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {

            }

            return Unit.Value;
        }
    }
}
