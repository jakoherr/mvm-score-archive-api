using Microsoft.Extensions.Options;
using Mvm.Score.Archive.Api.Helpers;
using Mvm.Score.Archive.Api.Helpers.ErrorHandling;
using Mvm.Score.Archive.Repository;
using Mvm.Score.Archive.Service;
using Serilog;
using static System.Net.WebRequestMethods;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

    builder.Host.UseSerilog();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    builder.Services.AddFileSettingsConfiguration(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddSwagger();

    // wichtig: Mapping setzen: frontendclient --> client scopes --> dedicated scope --> add mapper --> by configuration --> audience
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", opt =>
        {
            opt.Authority = "http://localhost:8888/realms/musikverein";

            opt.RequireHttpsMetadata = false; // nur lokal
            opt.Audience = "dotnet-api"; // Client ID deines Backends
        });

    builder.Services.AddServices(builder.Configuration);
    builder.Services.ConfigureProblemDetails();

    var app = builder.Build();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseExceptionHandler();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Score API");
        c.RoutePrefix = "api";
    });

    app.UseCors("AllowAll");

    app.Use(async (context, next) =>
    {
        if (context.Request.Path.Value is "/favicon.ico")
        {
            context.Response.StatusCode = 204;
            return;
        }

        await next();
    });

    app.UseSerilogRequestLogging();

    app.MapControllers();

    app.Logger.LogInformation("Running migrations");
    app.Services.RunMigrations();

    app.Logger.LogInformation("Application started");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "App failed to start");
}
finally
{
    Log.CloseAndFlush();
}