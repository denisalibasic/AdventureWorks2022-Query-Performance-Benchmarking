using System.Text.Json;

namespace AdventureWorksQueryPerformance.Service
{
    public class DisplayResultsService
    {
        public void DisplayResultsAsJson(List<TaskResult> results)
        {
            var json = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
