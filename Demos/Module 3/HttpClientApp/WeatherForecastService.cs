using Newtonsoft.Json;

namespace HttpClientApp;

// Dit is een proxy
public class WeatherForecastService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherForecastService(HttpClient httpClient, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClient;
        _httpClientFactory = httpClientFactory;
    }

    public IEnumerable<WeatherForecast>? GetWeather()
    {
        //var client = _httpClientFactory.CreateClient();
        var response = _httpClient.GetAsync("WeatherForecast").Result;
        if (response.IsSuccessStatusCode)
        {
            var strData = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<WeatherForecast>>(strData);
        }
        return null;
    }
}
