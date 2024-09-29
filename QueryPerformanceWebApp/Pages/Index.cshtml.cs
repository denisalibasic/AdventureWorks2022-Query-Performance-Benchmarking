using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QueryPerformanceWebApp.Pages
{
    public class IndexModel : PageModel
    {

        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetRunQueries()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5141/run-queries");
                response.EnsureSuccessStatusCode();

                var queryResults = await response.Content.ReadFromJsonAsync<List<QueryResult>>();
                return new JsonResult(queryResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while running queries.", error = ex.Message });
            }
        }
    }

    public class QueryResult
    {
        public string TaskName { get; set; } = string.Empty;
        public long ElapsedMilliseconds { get; set; }
    }
}
