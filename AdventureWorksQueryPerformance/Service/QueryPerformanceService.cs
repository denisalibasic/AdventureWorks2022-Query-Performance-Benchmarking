using AdventureWorksQueryPerformance.Enums;
using AdventureWorksQueryPerformance.Request;
using AdventureWorksQueryPerformance.Result;
using Azure.Core;
using MediatR;
using System.Diagnostics;

namespace AdventureWorksQueryPerformance.Service
{
    public class QueryPerformanceService : IQueryPerformanceService
    {
        private readonly IClearCacheService _clearCacheService;
        private readonly IExecuteAndMeasureTimeService _executeAndMeasureTimeService;
        private readonly IGenerateBarChartHtmlService _generateBarChartHtmlService;
        private readonly IRabbitMqService _rabbitMqService;

        public QueryPerformanceService(IClearCacheService clearCacheService
            , IExecuteAndMeasureTimeService executeAndMeasureTimeService
            , IGenerateBarChartHtmlService generateBarChartHtml
            , IRabbitMqService rabbitMqService)
        {
            _clearCacheService = clearCacheService;
            _executeAndMeasureTimeService = executeAndMeasureTimeService;
            _generateBarChartHtmlService = generateBarChartHtml;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<List<TaskResult>> RunQueriesSequentially()
        {
            _executeAndMeasureTimeService.ClearResults();

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpGetTopHundred }, SpQueryEnums.SpGetTopHundred.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ SpQueryEnums.SpGetTopHundred.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpCursor }, SpQueryEnums.SpCursor.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ SpQueryEnums.SpCursor.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawGetTopHundred }, EfRawQueryEnums.RawGetTopHundred.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ EfRawQueryEnums.RawGetTopHundred.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfForEach }, EfQueryEnums.EfForEach.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ EfQueryEnums.EfForEach.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfGetTopHundred }, EfQueryEnums.EfGetTopHundred.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ EfQueryEnums.EfGetTopHundred.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperGetTopHundred }, DapperQueryEnums.DapperGetTopHundred.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ DapperQueryEnums.DapperGetTopHundred.GetDescription());

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpSalesPerformance }, SpQueryEnums.SpSalesPerformance.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ SpQueryEnums.SpSalesPerformance.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfSalesPerformance }, EfQueryEnums.EfSalesPerformance.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ EfQueryEnums.EfSalesPerformance.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawSalesPerformance }, EfRawQueryEnums.RawSalesPerformance.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ EfRawQueryEnums.RawSalesPerformance.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperSalesPerformance }, DapperQueryEnums.DapperSalesPerformance.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed "+ DapperQueryEnums.DapperSalesPerformance.GetDescription());

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeData }, SpQueryEnums.SpLargeData.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + SpQueryEnums.SpLargeData.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeData }, EfQueryEnums.EfLargeData.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + EfQueryEnums.EfLargeData.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeData }, EfRawQueryEnums.RawLargeData.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + EfRawQueryEnums.RawLargeData.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperLargeData }, DapperQueryEnums.DapperLargeData.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + DapperQueryEnums.DapperLargeData.GetDescription());

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeDataGreaterThan }, SpQueryEnums.SpLargeDataGreaterThan.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + SpQueryEnums.SpLargeDataGreaterThan.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeDataGreaterThan }, EfQueryEnums.EfLargeDataGreaterThan.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + EfQueryEnums.EfLargeDataGreaterThan.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeDataGreaterThan }, EfRawQueryEnums.RawLargeDataGreaterThan.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + EfRawQueryEnums.RawLargeDataGreaterThan.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperLargeDataGreaterThan }, DapperQueryEnums.DapperLargeDataGreaterThan.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + DapperQueryEnums.DapperLargeDataGreaterThan.GetDescription());

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeDataGreaterThanWithIndex }, SpQueryEnums.SpLargeDataGreaterThanWithIndex.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + SpQueryEnums.SpLargeDataGreaterThanWithIndex.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeDataGreaterThanWithIndex }, EfQueryEnums.EfLargeDataGreaterThanWithIndex.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + EfQueryEnums.EfLargeDataGreaterThanWithIndex.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeDataGreaterThanWithIndex }, EfRawQueryEnums.RawLargeDataGreaterThanWithIndex.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + EfRawQueryEnums.RawLargeDataGreaterThanWithIndex.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperLargeDataGreaterThanWithIndex }, DapperQueryEnums.DapperLargeDataGreaterThanWithIndex.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + DapperQueryEnums.DapperLargeDataGreaterThanWithIndex.GetDescription());

            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeDataGreaterThanWithIndexSecond }, SpQueryEnums.SpLargeDataGreaterThanWithIndexSecond.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + SpQueryEnums.SpLargeDataGreaterThanWithIndexSecond.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeDataGreaterThanWithIndexSecond }, EfQueryEnums.EfLargeDataGreaterThanWithIndexSecond.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + EfQueryEnums.EfLargeDataGreaterThanWithIndexSecond.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeDataGreaterThanWithIndexSecond }, EfRawQueryEnums.RawLargeDataGreaterThanWithIndexSecond.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + EfRawQueryEnums.RawLargeDataGreaterThanWithIndexSecond.GetDescription());
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperLargeDataGreaterThanWithIndexSecond }, DapperQueryEnums.DapperLargeDataGreaterThanWithIndexSecond.GetDescription()));
            _rabbitMqService.PublishMessage($"Executed " + DapperQueryEnums.DapperLargeDataGreaterThanWithIndexSecond.GetDescription());

            var results = _executeAndMeasureTimeService.GetResults();
            string filePath = "QueryPerformanceResults.html";
            _generateBarChartHtmlService.GenerateBarChartHtml(results, filePath);
            return results;
        }
    }
}
