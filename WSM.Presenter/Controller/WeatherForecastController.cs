using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace WSM.Presenter.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class TelegramController : ControllerBase
    //{
    //    private readonly TelegramBotService _botService;

    //    public TelegramController(TelegramBotService botService)
    //    {
    //        _botService = botService;
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Post([FromBody] Update update)
    //    {
    //        //if (update.Message != null)
    //        //{
    //        //    var chatId = update.Message.Chat.Id;
    //        //    var messageText = update.Message.Text;

    //        //    // Echo the received message
    //        //    await _botService.SendMessageAsync(chatId, $"You said: {messageText}");
    //        //}
    //        return Ok();
    //    }
    //}
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecasto")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
