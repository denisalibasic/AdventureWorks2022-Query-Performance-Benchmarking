using AdventureWorksQueryPerformance.Enums;
using AdventureWorksQueryPerformance.Request;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
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

            string storedProcedureName = request.QueryType switch
            {
                SpQueryEnums.SpCursor => "GetTopCustomersDetailedWithCursor",
                SpQueryEnums.SpGetTopHundred => "GetTopCustomersDetailed",
                SpQueryEnums.SpSalesPerformance => "GetSalesPerformance",
                SpQueryEnums.SpLargeData => "GetSalesLargeDataAllRows",
                SpQueryEnums.SpLargeDataGreaterThan => "GetSalesLargeDataByValue",
                SpQueryEnums.SpLargeDataGreaterThanWithIndex => "GetSalesLargeDataByValueWithIndex",
                SpQueryEnums.SpLargeDataGreaterThanWithIndexSecond => "GetSalesLargeDataByValueWithIndex",
                _ => throw new ArgumentOutOfRangeException(nameof(request.QueryType), "Unknown query type")
            };

            Log.Information("Executing " + request.QueryType.GetDescription());
            using var command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            if (storedProcedureName == "GetSalesPerformance")
            {
                var startDate = new DateTime(2012, 1, 1);
                var endDate = new DateTime(2014, 12, 31);
                command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate });
                command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate });
            }

            if (request.QueryType.Equals(SpQueryEnums.SpLargeDataGreaterThanWithIndex))
            {
                var minValue = 4917;
                command.Parameters.Add(new SqlParameter("@MinValue", SqlDbType.Decimal) { Value = minValue });
            }
            if (request.QueryType.Equals(SpQueryEnums.SpLargeDataGreaterThanWithIndexSecond))
            {
                var minValue = 1000;
                command.Parameters.Add(new SqlParameter("@MinValue", SqlDbType.Decimal) { Value = minValue });
            }

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
