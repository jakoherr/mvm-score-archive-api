using Mvm.Score.Archive.Repository;
using Mvm.Score.Archive.Service;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            "AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddServices(builder.Configuration);

    var app = builder.Build();

    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();

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