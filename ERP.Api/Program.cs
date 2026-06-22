using ERP.Business;
using ERP.Data;
using ERP.Data.Persistence;
using ERP.Data.Seed;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddBusinessServices();
builder.Services.AddDataServices(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ERPDbContext>();
    await ERPDbSeeder.SeedAsync(dbContext);

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "SmartMarket ERP API";
        options.Theme = ScalarTheme.Default;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
