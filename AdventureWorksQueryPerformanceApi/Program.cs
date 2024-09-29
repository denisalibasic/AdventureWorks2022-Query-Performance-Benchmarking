using AdventureWorksQueryPerformanceApi;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IQueryPerformanceApiAdapter, QueryPerformanceApiAdapter>();
builder.Services.AddAdventureWorksServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/run-queries", async (IQueryPerformanceApiAdapter queryPerformanceApiAdapter) =>
{
    var queryResults = await queryPerformanceApiAdapter.ExecuteQueries();
    return Results.Ok(queryResults);
});

app.Run();
