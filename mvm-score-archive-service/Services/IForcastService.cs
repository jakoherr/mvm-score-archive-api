
namespace Mvm.Score.Archive.Service.Services;

public interface IForcastService
{
    IEnumerable<WeatherForecast> GetForcast();
}