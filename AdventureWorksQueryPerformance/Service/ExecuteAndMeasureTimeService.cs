using MediatR;
using System.Diagnostics;

namespace AdventureWorksQueryPerformance.Service
{
    public class ExecuteAndMeasureTimeService
    {
        private readonly IMediator _mediator;
        private readonly List<TaskResult> _results = new();

        public ExecuteAndMeasureTimeService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task ExecuteAndMeasureTimeAsync(IRequest<Unit> request, string queryType)
        {
            var stopwatch = Stopwatch.StartNew();
            await _mediator.Send(request);
            stopwatch.Stop();

            var elapsedTime = stopwatch.ElapsedMilliseconds;
            _results.Add(new TaskResult
            {
                TaskName = queryType,
                ElapsedMilliseconds = elapsedTime
            });
        }

        public List<TaskResult> GetResults()
        {
            return _results;
        }

        public void ClearResults()
        {
            _results.Clear();
        }
    }

    public class TaskResult
    {
        public string TaskName { get; set; } = string.Empty;
        public long ElapsedMilliseconds { get; set; }
    }
}
