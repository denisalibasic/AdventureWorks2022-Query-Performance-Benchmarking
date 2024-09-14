using Microsoft.EntityFrameworkCore;

namespace AdventureWorksQueryPerformance.DBContext
{
    public class AdventureWorksDbContext : DbContext
    {
        public AdventureWorksDbContext(DbContextOptions<AdventureWorksDbContext> options)
            : base(options)
        {
        }

        // TODO DBSET

        // TODO ONMODEL
    }
}
