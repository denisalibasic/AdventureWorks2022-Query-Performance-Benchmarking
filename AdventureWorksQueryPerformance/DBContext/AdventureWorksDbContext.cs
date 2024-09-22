using AdventureWorksQueryPerformance.Model;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksQueryPerformance.DBContext
{
    public class AdventureWorksDbContext : DbContext
    {
        public AdventureWorksDbContext(DbContextOptions<AdventureWorksDbContext> options)
            : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public DbSet<LargeDataTest> LargeDataTests { get; set; }
        public DbSet<LargeDataTestWithIndex> LargeDataTestWithIndices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesOrderHeader>()
                .ToTable("SalesOrderHeader", "Sales");

            modelBuilder.Entity<SalesOrderDetail>()
                .ToTable("SalesOrderDetail", "Sales");

            modelBuilder.Entity<Product>()
                .ToTable("Product", "Production");

            modelBuilder.Entity<Customer>()
                .ToTable("Customer", "Sales");

            modelBuilder.Entity<LargeDataTest>()
                .ToTable("LargeDataTest", "dbo");

            modelBuilder.Entity<LargeDataTestWithIndex>()
                .ToTable("LargeDataTestWithIndex", "dbo");

            modelBuilder.Entity<SalesOrderDetail>()
                .HasKey(sod => new { sod.SalesOrderID, sod.ProductID });

            modelBuilder.Entity<SalesOrderHeader>()
                .HasKey(soh => soh.SalesOrderID);

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerID);

            modelBuilder.Entity<LargeDataTest>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<SalesOrderHeader>()
                .HasMany(soh => soh.SalesOrderDetails)
                .WithOne(sod => sod.SalesOrderHeader)
                .HasForeignKey(sod => sod.SalesOrderID);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.SalesOrderHeaders)
                .WithOne(soh => soh.Customer)
                .HasForeignKey(soh => soh.CustomerID);
        }
    }
}
