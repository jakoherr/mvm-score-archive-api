using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
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

    var app = builder.Build();

    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSerilogRequestLogging();

    app.MapControllers();

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