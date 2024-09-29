using AdventureWorksQueryPerformance.Result;
using System.Text;

namespace AdventureWorksQueryPerformance.Service
{
    public class GenerateBarChartHtmlService : IGenerateBarChartHtmlService
    {
        public void GenerateBarChartHtml(List<TaskResult> results, string filePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='UTF-8'>");
            sb.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            sb.AppendLine("<title>Query Performance Results</title>");
            sb.AppendLine("<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h1>Query Performance Results</h1>");

            // Overall bar chart
            sb.AppendLine("<canvas id='myBarChart' width='800' height='400'></canvas>");
            sb.AppendLine("<script>");
            sb.AppendLine("const ctxBar = document.getElementById('myBarChart').getContext('2d');");
            sb.AppendLine("const myBarChart = new Chart(ctxBar, {");
            sb.AppendLine("    type: 'bar',");
            sb.AppendLine("    data: {");
            sb.AppendLine("        labels: [");

            for (int i = 0; i < results.Count; i++)
            {
                sb.Append($"'{results[i].TaskName}'");
                if (i < results.Count - 1)
                    sb.Append(", ");
            }
            sb.AppendLine("],");

            sb.AppendLine("        datasets: [{");
            sb.AppendLine("            label: 'Elapsed Time (ms)',");
            sb.AppendLine("            data: [");
            for (int i = 0; i < results.Count; i++)
            {
                sb.Append(results[i].ElapsedMilliseconds);
                if (i < results.Count - 1)
                    sb.Append(", ");
            }
            sb.AppendLine("],");
            sb.AppendLine("            backgroundColor: 'rgba(75, 192, 192, 0.2)',");
            sb.AppendLine("            borderColor: 'rgba(75, 192, 192, 1)',");
            sb.AppendLine("            borderWidth: 1");
            sb.AppendLine("        }]");
            sb.AppendLine("    },");
            sb.AppendLine("    options: {");
            sb.AppendLine("        scales: {");
            sb.AppendLine("            y: {");
            sb.AppendLine("                beginAtZero: true");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("});");
            sb.AppendLine("</script>");

            // Generate bar charts for each query type
            GenerateGroupedBarChart(sb, "SalesPerformance", results, new[] {
                "SpSalesPerformance", "EfSalesPerformance", "RawSalesPerformance", "DapperSalesPerformance"
            });

            GenerateGroupedBarChart(sb, "LargeData", results, new[] {
                "SpLargeData", "EfLargeData", "RawLargeData", "DapperLargeData"
            });

            GenerateGroupedBarChart(sb, "LargeDataGreaterThan", results, new[] {
                "SpLargeDataGreaterThan", "EfLargeDataGreaterThan", "RawLargeDataGreaterThan", "DapperLargeDataGreaterThan"
            });

            GenerateGroupedBarChart(sb, "LargeDataGreaterThanWithIndex", results, new[] {
                "SpLargeDataGreaterThanWithIndex", "EfLargeDataGreaterThanWithIndex", "RawLargeDataGreaterThanWithIndex", "DapperLargeDataGreaterThanWithIndex"
            });

            GenerateGroupedBarChart(sb, "LargeDataGreaterThanWithIndexSecond", results, new[] {
                "SpLargeDataGreaterThanWithIndexSecond", "EfLargeDataGreaterThanWithIndexSecond", "RawLargeDataGreaterThanWithIndexSecond", "DapperLargeDataGreaterThanWithIndexSecond"
            });

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            File.WriteAllText(filePath, sb.ToString());
        }

        private void GenerateGroupedBarChart(StringBuilder sb, string chartId, List<TaskResult> results, string[] taskNames)
        {
            sb.AppendLine($"<canvas id='{chartId}' width='800' height='400'></canvas>");
            sb.AppendLine("<script>");
            sb.AppendLine($"const ctx{chartId} = document.getElementById('{chartId}').getContext('2d');");
            sb.AppendLine($"const {chartId}Chart = new Chart(ctx{chartId}, {{");
            sb.AppendLine("    type: 'bar',");
            sb.AppendLine("    data: {");
            sb.AppendLine("        labels: [");

            // Labels
            for (int i = 0; i < taskNames.Length; i++)
            {
                sb.Append($"'{taskNames[i]}'");
                if (i < taskNames.Length - 1)
                    sb.Append(", ");
            }
            sb.AppendLine("],");

            sb.AppendLine("        datasets: [{");
            sb.AppendLine("            label: 'Elapsed Time (ms)',");
            sb.AppendLine("            data: [");

            // Data
            for (int i = 0; i < taskNames.Length; i++)
            {
                var result = results.FirstOrDefault(r => r.TaskName == taskNames[i]);
                sb.Append(result != null ? result.ElapsedMilliseconds.ToString() : "0");
                if (i < taskNames.Length - 1)
                    sb.Append(", ");
            }
            sb.AppendLine("],");
            sb.AppendLine("            backgroundColor: 'rgba(153, 102, 255, 0.2)',");
            sb.AppendLine("            borderColor: 'rgba(153, 102, 255, 1)',");
            sb.AppendLine("            borderWidth: 1");
            sb.AppendLine("        }]");
            sb.AppendLine("    },");
            sb.AppendLine("    options: {");
            sb.AppendLine("        scales: {");
            sb.AppendLine("            y: {");
            sb.AppendLine("                beginAtZero: true");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("});");
            sb.AppendLine("</script>");
        }
    }
}
