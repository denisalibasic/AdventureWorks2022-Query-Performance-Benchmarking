using AdventureWorksQueryPerformance.Enums;
using AdventureWorksQueryPerformance.Request;
using AdventureWorksQueryPerformance.Result;
using System.Diagnostics;

namespace AdventureWorksQueryPerformance.Service
{
    public class QueryPerformanceService : IQueryPerformanceService
    {
        private readonly IClearCacheService _clearCacheService;
        private readonly IExecuteAndMeasureTimeService _executeAndMeasureTimeService;
        private readonly IGenerateBarChartHtmlService _generateBarChartHtmlService;

        public QueryPerformanceService(IClearCacheService clearCacheService
            , IExecuteAndMeasureTimeService executeAndMeasureTimeService
            , IGenerateBarChartHtmlService generateBarChartHtml)
        {
            _clearCacheService = clearCacheService;
            _executeAndMeasureTimeService = executeAndMeasureTimeService;
            _generateBarChartHtmlService = generateBarChartHtml;
        }

        public async Task<List<TaskResult>> RunQueriesSequentially()
        {
            _executeAndMeasureTimeService.ClearResults();

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpGetTopHundred }, SpQueryEnums.SpGetTopHundred.GetDescription()));
            /*await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpCursor }, SpQueryEnums.SpCursor.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawGetTopHundred }, EfRawQueryEnums.RawGetTopHundred.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfForEach }, EfQueryEnums.EfForEach.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfGetTopHundred }, EfQueryEnums.EfGetTopHundred.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperGetTopHundred }, DapperQueryEnums.DapperGetTopHundred.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpSalesPerformance }, SpQueryEnums.SpSalesPerformance.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfSalesPerformance }, EfQueryEnums.EfSalesPerformance.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawSalesPerformance }, EfRawQueryEnums.RawSalesPerformance.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperSalesPerformance }, DapperQueryEnums.DapperSalesPerformance.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeData }, SpQueryEnums.SpLargeData.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeData }, EfQueryEnums.EfLargeData.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeData }, EfRawQueryEnums.RawLargeData.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperLargeData }, DapperQueryEnums.DapperLargeData.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeDataGreaterThan }, SpQueryEnums.SpLargeDataGreaterThan.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeDataGreaterThan }, EfQueryEnums.EfLargeDataGreaterThan.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeDataGreaterThan }, EfRawQueryEnums.RawLargeDataGreaterThan.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperLargeDataGreaterThan }, DapperQueryEnums.DapperLargeDataGreaterThan.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeDataGreaterThanWithIndex }, SpQueryEnums.SpLargeDataGreaterThanWithIndex.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() =>_executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeDataGreaterThanWithIndex }, EfQueryEnums.EfLargeDataGreaterThanWithIndex.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeDataGreaterThanWithIndex }, EfRawQueryEnums.RawLargeDataGreaterThanWithIndex.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperLargeDataGreaterThanWithIndex }, DapperQueryEnums.DapperLargeDataGreaterThanWithIndex.GetDescription()));

            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new StoredProcedureQueryRequest { QueryType = SpQueryEnums.SpLargeDataGreaterThanWithIndexSecond }, SpQueryEnums.SpLargeDataGreaterThanWithIndexSecond.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new EFQueryRequest { QueryType = EfQueryEnums.EfLargeDataGreaterThanWithIndexSecond }, EfQueryEnums.EfLargeDataGreaterThanWithIndexSecond.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new RawSQLQueryRequest { QueryType = EfRawQueryEnums.RawLargeDataGreaterThanWithIndexSecond }, EfRawQueryEnums.RawLargeDataGreaterThanWithIndexSecond.GetDescription()));
            await _clearCacheService.ClearCacheAndExecuteAsync(() => _executeAndMeasureTimeService.ExecuteAndMeasureTimeAsync(new DapperQueryRequest { QueryType = DapperQueryEnums.DapperLargeDataGreaterThanWithIndexSecond }, DapperQueryEnums.DapperLargeDataGreaterThanWithIndexSecond.GetDescription()));*/

            return _executeAndMeasureTimeService.GetResults();
        }
    }
}
