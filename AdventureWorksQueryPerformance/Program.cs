using AdventureWorksQueryPerformance.DBContext;
using AdventureWorksQueryPerformance.Handler;
using AdventureWorksQueryPerformance.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);

services.AddDbContext<AdventureWorksDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("AdventureWorksDb")));
services.AddMediatR(typeof(Program));

services.AddTransient<RawSQLQueryHandler>(provider =>
    new RawSQLQueryHandler(configuration.GetConnectionString("AdventureWorksDb")));
services.AddTransient<StoredProcedureQueryHandler>(provider =>
    new StoredProcedureQueryHandler(configuration.GetConnectionString("AdventureWorksDb")));
services.AddTransient<BulkInsertHandler>(provider =>
    new BulkInsertHandler(configuration.GetConnectionString("AdventureWorksDb")));

services.AddTransient<QueryPerformanceService>();

var serviceProvider = services.BuildServiceProvider();
var queryService = serviceProvider.GetRequiredService<QueryPerformanceService>();

//await queryService.RunQueriesSequentiallyAsync();