using AdventureWorksQueryPerformance.DBContext;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksQueryPerformance.Service
{
    public class ClearCacheService : IClearCacheService
    {
        private readonly IDbContextFactory<AdventureWorksDbContext> _dbContext;

        public ClearCacheService(IDbContextFactory<AdventureWorksDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ClearCacheAndExecuteAsync(Func<Task> action)
        {
            ClearCache();
            await action();
        }

        public void ClearCache()
        {
            using var context = _dbContext.CreateDbContext();
            // Flush the plan cache for the entire instance and suppress the regular completion message
            var queryFlush = $@"
                DBCC FREEPROCCACHE WITH NO_INFOMSGS;";
            context.Database.ExecuteSqlRaw(queryFlush);

            // Clears the buffer cache, which removes all data pages from the buffer pool.
            // This forces SQL Server to read data from disk rather than from the cache, ensuring that data is freshly read.
            var queryCache = $@"
                CHECKPOINT;
                DBCC DROPCLEANBUFFERS;";
            context.Database.ExecuteSqlRaw(queryCache);

            // Flushes all procedure cache for the current database.
            // This command ensures that cached query plans specific to the database are also removed.
            var databaseId = context.Database.GetDbConnection().Database;
            var query = $@"
                DECLARE @db_id INT;
                SELECT @db_id = database_id
                FROM sys.databases
                WHERE name = '{databaseId}';
                DBCC FLUSHPROCINDB(@db_id);";

            context.Database.ExecuteSqlRaw(query);

            //Ensures that statistics are updated by scanning all rows in the table leading to more accurate statistics
            //var queryStatistics = "EXEC sp_MSforeachtable 'UPDATE STATISTICS ? WITH FULLSCAN';";
            //context.Database.ExecuteSqlRaw(queryStatistics);
        }
    }
}
