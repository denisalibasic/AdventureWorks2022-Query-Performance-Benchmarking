using AdventureWorksQueryPerformance.Result;

namespace AdventureWorksQueryPerformance.Service
{
    public interface IGenerateBarChartHtmlService
    {
        void GenerateBarChartHtml(List<TaskResult> results, string filePath);
    }
}
