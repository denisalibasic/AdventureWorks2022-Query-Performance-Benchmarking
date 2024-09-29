using AdventureWorksQueryPerformance.Result;
using MediatR;

namespace AdventureWorksQueryPerformance.Service
{
    public interface IExecuteAndMeasureTimeService
    {
        Task ExecuteAndMeasureTimeAsync(IRequest<Unit> request, string queryType);
        List<TaskResult> GetResults();
        void ClearResults();
    }
}
