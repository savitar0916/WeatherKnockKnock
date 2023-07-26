using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using WeatherKnockKnock.Entity;

namespace WeatherKnockKnock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherController(ILogger<WeatherController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // Webhook -> sendMessage
        [HttpGet("CheckBot")]
        public async Task<String> CheckBot()
        {
            TelegramBotClient botClient = new("5626168623:AAE7A5it2ayPeI3fNf43FADjTQCdeZ97xSk");
            //取得機器人基本資訊
            var bot = await botClient.GetMeAsync();

            _logger.LogInformation(nameof(CheckBot));
            Console.WriteLine($"安安，我是個很讚的機器人，編號是{bot.Id}，名字是{bot.FirstName}!");

            //回傳取得的機器人基本資訊
            return $"安安，我是個很讚的機器人，編號是{bot.Id}，名字是{bot.FirstName}!";
        }
        [HttpPost("Weather")]
        public async Task<WeatherKnockData> GetWeatherAsync()
        {
            //三十六小時天氣預報
            String Url = "https://opendata.cwb.gov.tw/api";
            String ActionUrl = "/v1/rest/datastore/F-C0032-001";
            String Authorization = _configuration["Authorization"];
            String jsonResult = "";
            string city = "臺中市";
            WeatherKnockData weatherKnockData = new WeatherKnockData();
            WeatherData? weatherData = new WeatherData();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("accept", "application/json");
                var task = httpClient.GetAsync(Url + ActionUrl + $"?Authorization={Authorization}"
                 + $"&format=JSON&locationName={System.Web.HttpUtility.UrlEncode(city)}&elementName=Wx&sort=time");
                var httpResponseMessage = await task.ConfigureAwait(false);
                if (httpResponseMessage.Content != null)
                {
                    jsonResult = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                if (jsonResult != null)
                {
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResult);
                }

                if (weatherData != null)
                {
                    weatherKnockData.datasetDescription = weatherData.records.datasetDescription;
                    if (weatherData.records != null)
                    {
                        if (weatherData.records.location != null)
                        {
                            foreach (var location in weatherData.records.location)
                            {
                                weatherKnockData.cityName = location.locationName;

                                if (location.weatherElement != null)
                                {
                                    foreach (var weatherElement in location.weatherElement)
                                    {
                                        if (weatherElement.time != null)
                                        {
                                            foreach (var time in weatherElement.time)
                                            {
                                                WeatherKnockTime weatherKnockTime = new WeatherKnockTime();
                                                weatherKnockTime.startTime = time.startTime;
                                                weatherKnockTime.endTime = time.endTime;
                                                weatherKnockTime.weatherStatus = time.parameter.parameterName;
                                                weatherKnockData.weatherKnockTimes.Add(weatherKnockTime);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error:" + e.Message);
            }

            return weatherKnockData;
        }
    }
}
