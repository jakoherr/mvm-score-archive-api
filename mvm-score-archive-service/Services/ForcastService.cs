using Microsoft.Extensions.Logging;

namespace Mvm.Score.Archive.Service.Services;

public class ForcastService : IForcastService
{
    private readonly ILogger<ForcastService> logger;

    public ForcastService(
        ILogger<ForcastService> logger)
    {
        this.logger = logger;
    }

    public IEnumerable<WeatherForecast> GetForcast()
    {
        logger.LogError("test");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = "Test",
        })
        .ToArray();
    }
}
