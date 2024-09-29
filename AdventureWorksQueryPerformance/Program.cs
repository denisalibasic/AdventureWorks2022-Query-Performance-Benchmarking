using AdventureWorksQueryPerformance.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

Log.Information("Application execution Starting...");

var services = new ServiceCollection();

services.AddAdventureWorksServices(configuration);

var serviceProvider = services.BuildServiceProvider();
var queryService = serviceProvider.GetRequiredService<IQueryPerformanceService>();

await queryService.RunQueriesSequentially();

Log.Information("Application execution done...");