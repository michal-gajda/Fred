namespace Fred.Server.WebApi;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Rebus.Config;
using Rebus.Kafka;
using Rebus.OpenTelemetry.Configuration;

public class Program
{
    private const string SERVICE_NAME = "server-webapi";
    private const string SERVICE_NAMESPACE = "fred";
    private const string SERVICE_VERSION = "1.0.0";
    private const string SERVICE_INSTANCE_ID = "instance-1";

    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(SERVICE_NAME, SERVICE_NAMESPACE, SERVICE_VERSION, autoGenerateServiceInstanceId: false, serviceInstanceId: SERVICE_INSTANCE_ID);

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.AddOtlpExporter();
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.SetResourceBuilder(resourceBuilder);
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.Filter = context => !context.Request.Path.StartsWithSegments("/health");
                })
                .AddHttpClientInstrumentation(options => options.RecordException = true)
                .AddRebusInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRebusInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter());

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var queueName = "fred-queue";

        builder.Services.AddRebus(configure => configure
            .Options(options => options.EnableDiagnosticSources())
            .Sagas(sagas => sagas.StoreInSqlServer(builder.Configuration.GetConnectionString("SqlServer"), dataTableName: "Sagas", indexTableName: "SagaIndexes"))
            .Timeouts(timeouts => timeouts.StoreInSqlServer(builder.Configuration.GetConnectionString("SqlServer"), tableName: "Timeouts"))
            .Transport(transport => transport.UseKafka(builder.Configuration.GetConnectionString("Kafka"), queueName))
        );

        builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

        var app = builder.Build();

        app.UseHealthChecks("/healthz");

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Fred-Server API v1");
            });
        }

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();

        return Environment.ExitCode;
    }
}
