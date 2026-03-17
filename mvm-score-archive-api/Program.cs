using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mvm.Score.Archive.Api.Helpers;
using Mvm.Score.Archive.Api.Helpers.ErrorHandling;
using Mvm.Score.Archive.Repository;
using Mvm.Score.Archive.Service;
using Serilog;

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

    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authorization:Authority"];
        options.Audience = builder.Configuration["Authorization:Audience"];

        options.RequireHttpsMetadata = true;

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
        };
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