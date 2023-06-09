using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(DiagnosticsConfig.ActivitySource.Name)
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
             .AddOtlpExporter( options =>
             {
                 // if you are using otelcollector directly use {base_url}/v1/traces
                 // via fluent bit use only baseurl
                 options.Endpoint = new Uri("http://localhost:4317");
                 options.HttpClientFactory = () =>
                 {
                     return new HttpClient(new HttpClientHandler()
                     {
                         ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                     }); ;
                 };
                 //options.Endpoint = new Uri("http://fluentbit:3000");
             })
            .AddConsoleExporter());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


public static class DiagnosticsConfig
{
    public const string ServiceName = "ApidemoApp";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
}