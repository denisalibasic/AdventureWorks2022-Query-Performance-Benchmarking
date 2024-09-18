using AdventureWorksQueryPerformance.DBContext;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksQueryPerformance.Service
{
    public class ClearCacheService
    {
        private readonly AdventureWorksDbContext _dbContext;

        public ClearCacheService(AdventureWorksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ClearCache()
        {
            // Flush the plan cache for the entire instance and suppress the regular completion message
            var queryFlush = $@"
                DBCC FREEPROCCACHE WITH NO_INFOMSGS;";
            _dbContext.Database.ExecuteSqlRaw(queryFlush);

            // Clears the buffer cache, which removes all data pages from the buffer pool.
            // This forces SQL Server to read data from disk rather than from the cache, ensuring that data is freshly read.
            var queryCache = $@"
                CHECKPOINT;
                DBCC DROPCLEANBUFFERS;";
            _dbContext.Database.ExecuteSqlRaw(queryCache);

            // Flushes all procedure cache for the current database.
            // This command ensures that cached query plans specific to the database are also removed.
            var databaseId = _dbContext.Database.GetDbConnection().Database;
            var query = $@"
                DECLARE @db_id INT;
                SELECT @db_id = database_id
                FROM sys.databases
                WHERE name = '{databaseId}';
                DBCC FLUSHPROCINDB(@db_id);";

            _dbContext.Database.ExecuteSqlRaw(query);
        }
    }
}
