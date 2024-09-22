using AdventureWorksQueryPerformance.DBContext;
using AdventureWorksQueryPerformance.Handler;
using AdventureWorksQueryPerformance.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

Log.Information("Application Starting...");

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);

// Register the connection string as a singleton
services.AddSingleton(provider => configuration.GetConnectionString("AdventureWorksDb"));

// Register DBContext
services.AddDbContext<AdventureWorksDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("AdventureWorksDb")));
services.AddDbContextFactory<AdventureWorksDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("AdventureWorksDb")));

// Register MediatR
services.AddMediatR(Assembly.GetExecutingAssembly());

// Handlers
services.AddTransient<EFQueryHandler>();
services.AddTransient<RawSQLQueryHandler>();
services.AddTransient<StoredProcedureQueryHandler>();

services.AddTransient<QueryPerformanceService>();
services.AddTransient<ClearCacheService>();
services.AddTransient<ExecuteAndMeasureTimeService>();
services.AddTransient<DisplayResultsService>();
services.AddTransient<GenerateBarChartHtmlService>();

var serviceProvider = services.BuildServiceProvider();
var queryService = serviceProvider.GetRequiredService<QueryPerformanceService>();

await queryService.RunQueriesSequentially();

Log.Information("Application execution done...");