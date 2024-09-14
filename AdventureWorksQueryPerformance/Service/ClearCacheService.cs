using AdventureWorksQueryPerformance.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Clears the procedure cache, removing all cached execution plans.
            // This ensures that SQL Server does not reuse cached plans, forcing a recompile of the queries.
            _dbContext.Database.ExecuteSqlRaw("DBCC FREEPROCCACHE;");

            // Clears the buffer cache, which removes all data pages from the buffer pool.
            // This forces SQL Server to read data from disk rather than from the cache, ensuring that data is freshly read.
            _dbContext.Database.ExecuteSqlRaw("DBCC DROPCLEANBUFFERS;");

            // Flushes all procedure cache for the current database.
            // This command ensures that cached query plans specific to the database are also removed.
            _dbContext.Database.ExecuteSqlRaw("DBCC FLUSHPROCINDB;");
        }
    }
}
