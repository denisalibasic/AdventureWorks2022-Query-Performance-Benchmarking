namespace AdventureWorksQueryPerformance.Service
{
    public interface IClearCacheService
    {
        Task ClearCacheAndExecuteAsync(Func<Task> action);
        void ClearCache();
    }
}
