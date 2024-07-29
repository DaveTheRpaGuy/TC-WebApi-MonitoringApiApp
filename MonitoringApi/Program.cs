using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MonitoringApi.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddCheck<RandomHealthCheck>("Site Health Check")
    .AddCheck<RandomHealthCheck>("Database Health Check");

builder.Services.AddHealthChecksUI(opts =>
{
    opts.AddHealthCheckEndpoint("api", "/health"); // setting this to monitor itself basically
    opts.SetEvaluationTimeInSeconds(10); // this is aggressive, in prod maybe 5 minutes
}).AddInMemoryStorage(); // could be put in sql server or whatever here instead of in memory

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

app.Run();
