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
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
