using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Services;

namespace Mvm.Score.Archive.Api.Controllers;

[ApiController]
[Produces("application/json", "text/json")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> logger;
    private readonly IForcastService forcastService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IForcastService forcastService)
    {
        this.logger = logger;
        this.forcastService = forcastService;
    }

    [HttpGet("Forecast")]
    public IActionResult Get()
    {
        var forecasts = this.forcastService.GetForcast();

        return this.Ok(forecasts);
    }
}
