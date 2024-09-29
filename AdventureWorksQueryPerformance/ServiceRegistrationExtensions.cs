using AdventureWorksQueryPerformance.DBContext;
using AdventureWorksQueryPerformance.Handler;
using AdventureWorksQueryPerformance.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

public static class ServiceRegistrationExtensions
{
    public static void AddAdventureWorksServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);

        services.AddDbContext<AdventureWorksDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AdventureWorksDb")));

        services.AddDbContextFactory<AdventureWorksDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("AdventureWorksDb")), ServiceLifetime.Scoped);

        services.AddMediatR(typeof(QueryPerformanceService).Assembly);

        services.AddTransient<EFQueryHandler>();
        services.AddTransient<RawSQLQueryHandler>();
        services.AddTransient<StoredProcedureQueryHandler>();
        services.AddTransient<DapperQueryHandler>();

        services.AddTransient<IQueryPerformanceService, QueryPerformanceService>();
        services.AddTransient<IClearCacheService, ClearCacheService>();
        services.AddTransient<IExecuteAndMeasureTimeService, ExecuteAndMeasureTimeService>();
        services.AddTransient<IGenerateBarChartHtmlService, GenerateBarChartHtmlService>();
    }

    public static ServiceProvider BuildServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddAdventureWorksServices(configuration);

        return services.BuildServiceProvider();
    }
}
