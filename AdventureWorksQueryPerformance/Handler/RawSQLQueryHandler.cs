using AdventureWorksQueryPerformance.Enums;
using AdventureWorksQueryPerformance.Queries.RawQuery;
using AdventureWorksQueryPerformance.Request;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Data;

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

            string query = request.QueryType switch
            {
                EfRawQueryEnums.RawGetTopHundred => EfRawQueries.GetTopCustomersDetailedQuery(),
                EfRawQueryEnums.RawSalesPerformance => EfRawQueries.GetSalesPerformance(),
                EfRawQueryEnums.RawLargeData => EfRawQueries.GetSalesLargeDataAllRows(),
                EfRawQueryEnums.RawLargeDataGreaterThan => EfRawQueries.GetSalesLargeDataByValue(),
                EfRawQueryEnums.RawLargeDataGreaterThanWithIndex => EfRawQueries.GetSalesLargeDataByValueWithIndex(),
                _ => throw new ArgumentOutOfRangeException(nameof(request.QueryType), "Unknown query type")
            };

            Log.Information("Executing " + request.QueryType.GetDescription());

            using var command = new SqlCommand(query, connection);

            if (request.QueryType == EfRawQueryEnums.RawSalesPerformance)
            {
                var startDate = new DateTime(2012, 1, 1);
                var endDate = new DateTime(2014, 12, 31);
                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate });
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate });
            }

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
