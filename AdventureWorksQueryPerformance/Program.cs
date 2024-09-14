using AdventureWorksQueryPerformance.DBContext;
using AdventureWorksQueryPerformance.Handler;
using AdventureWorksQueryPerformance.Service;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);

// Register the connection string as a singleton
services.AddSingleton(provider => configuration.GetConnectionString("AdventureWorksDb"));

// Register DBContext
services.AddDbContext<AdventureWorksDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("AdventureWorksDb")));

// Register MediatR
services.AddMediatR(Assembly.GetExecutingAssembly());

// Handlers
services.AddTransient<EFQueryHandler>();
services.AddTransient<RawSQLQueryHandler>();
services.AddTransient<StoredProcedureQueryHandler>();

services.AddTransient<QueryPerformanceService>();
services.AddTransient<ClearCacheService>();

var serviceProvider = services.BuildServiceProvider();
var queryService = serviceProvider.GetRequiredService<QueryPerformanceService>();

await queryService.RunQueriesSequentiallyAsync();