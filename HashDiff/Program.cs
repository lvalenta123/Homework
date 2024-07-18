using HashDiff.Extensions;
using HashDiff.Formatters;
using Serilog;
using Serilog.Context;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] <{TraceIdentifier}> {Message:lj}{NewLine}{Exception}"));


    builder.Services.AddControllers(o => o.InputFormatters.Add(new Base64InputFormatter()));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.RegisterDependencies();
    
    builder.AddDbConnection();
    
    await using var app = builder.Build();
    
    app.UseSerilogRequestLogging(options =>
    {
        options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;
    
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("TraceIdentifier", httpContext.TraceIdentifier);
        };
    });
    
    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.Use(async (context, next) =>
    {
        using (LogContext.PushProperty("TraceIdentifier", context.TraceIdentifier))
        {
            await next(context);
        }
    });

    app.MapControllers();

    await app.EnsureDBCreation();

    await app.RunAsync();

    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}