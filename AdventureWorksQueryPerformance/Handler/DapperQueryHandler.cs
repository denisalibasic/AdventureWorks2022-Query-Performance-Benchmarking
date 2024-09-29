using AdventureWorksQueryPerformance.Enums;
using AdventureWorksQueryPerformance.Queries.DapperQuery;
using AdventureWorksQueryPerformance.Request;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Data;

namespace AdventureWorksQueryPerformance.Handler
{
    public class DapperQueryHandler : IRequestHandler<DapperQueryRequest, Unit>
    {
        private readonly string _connectionString;

        public DapperQueryHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksDb");
        }

        public async Task<Unit> Handle(DapperQueryRequest request, CancellationToken cancellationToken)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            await dbConnection.OpenAsync(cancellationToken);

            string query = request.QueryType switch
            {
                DapperQueryEnums.DapperGetTopHundred => DapperQueries.GetTopCustomersDetailedQuery(),
                DapperQueryEnums.DapperSalesPerformance => DapperQueries.GetSalesPerformance(),
                DapperQueryEnums.DapperLargeData => DapperQueries.GetSalesLargeDataAllRows(),
                DapperQueryEnums.DapperLargeDataGreaterThan => DapperQueries.GetSalesLargeDataByValue(),
                DapperQueryEnums.DapperLargeDataGreaterThanWithIndex => DapperQueries.GetSalesLargeDataByValueWithIndex(),
                DapperQueryEnums.DapperLargeDataGreaterThanWithIndexSecond => DapperQueries.GetSalesLargeDataByValueWithIndexSecond(),
                _ => throw new ArgumentOutOfRangeException(nameof(request.QueryType), "Unknown query type")
            };

            Log.Information("Executing " + request.QueryType.GetDescription());

            if (request.QueryType == DapperQueryEnums.DapperSalesPerformance)
            {
                var startDate = new DateTime(2012, 1, 1);
                var endDate = new DateTime(2014, 12, 31);
                return await ExecuteQueryWithParameters(dbConnection, query, new { StartDate = startDate, EndDate = endDate });
            }

            if (request.QueryType == DapperQueryEnums.DapperLargeDataGreaterThanWithIndex)
            {
                var minValue = 4917;
                return await ExecuteQueryWithParameters(dbConnection, query, new { MinValue = minValue });
            }

            if (request.QueryType == DapperQueryEnums.DapperLargeDataGreaterThanWithIndexSecond)
            {
                var minValue = 1000;
                return await ExecuteQueryWithParameters(dbConnection, query, new { MinValue = minValue });
            }

            await dbConnection.QueryAsync(query);
            return Unit.Value;
        }

        private async Task<Unit> ExecuteQueryWithParameters(IDbConnection dbConnection, string query, object parameters)
        {
            await dbConnection.QueryAsync(query, parameters);
            return Unit.Value;
        }
    }
}
