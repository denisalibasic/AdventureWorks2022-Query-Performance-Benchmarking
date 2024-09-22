using AdventureWorksQueryPerformance.Enums;
using AdventureWorksQueryPerformance.Request;
using MediatR;
using System.Diagnostics;
using System.Text.Json;

namespace AdventureWorksQueryPerformance.Service
{
    public class QueryPerformanceService
    {
        private readonly ClearCacheService _clearCacheService;
        private readonly ExecuteAndMeasureTimeService _executeAndMeasureTimeService;
        private readonly GenerateBarChartHtmlService _generateBarChartHtmlService;

        public QueryPerformanceService(ClearCacheService clearCacheService
            ,ExecuteAndMeasureTimeService executeAndMeasureTimeService
            ,GenerateBarChartHtmlService generateBarChartHtml)
        {
            _clearCacheService = clearCacheService;
            _executeAndMeasureTimeService = executeAndMeasureTimeService;
            _generateBarChartHtmlService = generateBarChartHtml;
        }

        public async Task RunQueriesSequentially()
        {
            _executeAndMeasureTimeService.ClearResults();

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpGetTopHundred }, SpQueryEnums.SpGetTopHundred.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpCursor }, SpQueryEnums.SpCursor.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawGetTopHundred }, EfRawQueryEnums.RawGetTopHundred.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfForEach }, EfQueryEnums.EfForEach.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfGetTopHundred }, EfQueryEnums.EfGetTopHundred.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpSalesPerformance }, SpQueryEnums.SpSalesPerformance.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfSalesPerformance }, EfQueryEnums.EfSalesPerformance.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawSalesPerformance }, EfRawQueryEnums.RawSalesPerformance.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeData }, SpQueryEnums.SpLargeData.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeData }, EfQueryEnums.EfLargeData.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeData }, EfRawQueryEnums.RawLargeData.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeDataGreaterThan }, SpQueryEnums.SpLargeDataGreaterThan.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeDataGreaterThan }, EfQueryEnums.EfLargeDataGreaterThan.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeDataGreaterThan }, EfRawQueryEnums.RawLargeDataGreaterThan.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeDataGreaterThanWithIndex }, SpQueryEnums.SpLargeDataGreaterThanWithIndex.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeDataGreaterThanWithIndex }, EfQueryEnums.EfLargeDataGreaterThanWithIndex.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeDataGreaterThanWithIndex }, EfRawQueryEnums.RawLargeDataGreaterThanWithIndex.GetDescription()));

            var results = _executeAndMeasureTimeService.GetResults();

            string filePath = "QueryPerformanceResults.html";
            _generateBarChartHtmlService.GenerateBarChartHtml(results, filePath);

            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
    }
}
