using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WeatherKnockKnock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBotController : ControllerBase
    {
        //Webhook https://api.telegram.org/bot123456:ABCDEF-1234/setWebhook?url=https://你的DomainName/你的程式路徑
        private readonly ILogger<TelegramBotController> _logger;
        private readonly IConfiguration _configuration;
        string token = "5626168623:AAE7A5it2ayPeI3fNf43FADjTQCdeZ97xSk";
        string url = "https://api.telegram.org/bot";

        public TelegramBotController(ILogger<TelegramBotController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            
        }
        // Webhook -> sendMessage
        [HttpGet("SetWebhook")]
        public async Task<String> SetWebhook()
        {
            //Set Webhook https://api.telegram.org/bot123456:ABCDEF-1234/setWebhook?url=https://你的DomainName/你的程式路徑
            string webhookUrl = "https://weatherknockknock-kuo25a367a-as.a.run.app/api/weather/weather";
            string jsonResult = "";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            var task = httpClient.GetAsync(url + token + "/setWebhook?url=" + webhookUrl);
            var httpResponseMessage = await task.ConfigureAwait(false);
            if (httpResponseMessage.Content != null)
            {
                jsonResult = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            return jsonResult;
        }
        [HttpGet("GetWebhook")]
        public async Task<String> GetWebhookInfo()
        {
            //Get Webhook Information https://api.telegram.org/bot123456:ABCDEF-1234/getWebhookInfo
            string jsonResult = "";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            var task = httpClient.GetAsync(url + token + "/getWebhookInfo");
            var httpResponseMessage = await task.ConfigureAwait(false);
            if (httpResponseMessage.Content != null)
            {
                jsonResult = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            return jsonResult;
        }
        [HttpGet("DeleteWebhook")]
        public async Task<String> DeleteWebhook()
        {
            //Delete(Remove) Webhook https://api.telegram.org/bot123456:ABCDEF-1234/deleteWebhook
            string jsonResult = "";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            var task = httpClient.GetAsync(url + token + "/deleteWebhook");
            var httpResponseMessage = await task.ConfigureAwait(false);
            if (httpResponseMessage.Content != null)
            {
                jsonResult = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            return jsonResult;
        }
    }
}
