namespace Mvm.Score.Archive.Api;

public class CorsConfig
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        // ... Weitere ConfigureServices-Konfigurationen
    }

    // Configure-Methode
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // ... Andere Middleware-Konfigurationen

        app.UseCors("AllowAll");

        // ... Andere Middleware-Konfigurationen
    }
}
