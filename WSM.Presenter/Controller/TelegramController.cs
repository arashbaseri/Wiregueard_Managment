using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WSM.Application.Interfaces;


namespace WSM.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelegramController : ControllerBase
    {
        private readonly ITelegramBotService _botService;
        private readonly ILogger<TelegramController> _logger;
        public TelegramController(ITelegramBotService botService, ILogger<TelegramController> logger)
        {
            _botService = botService;
            _logger = logger;
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Update update)
        //{
        //    _logger.LogInformation("Received update: {@Update}", update);
        //    await _botService.HandleUpdateAsync(update);
        //    return Ok();
        //}
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            // Read the raw request body
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var requestBody = await reader.ReadToEndAsync();
                _logger.LogInformation("Received raw request body: {RequestBody}", requestBody);

                // Deserialize the request body to Update object
                try
                {
                    var update = //JsonSerializer.Deserialize<Update>(requestBody);
                    System.Text.Json.JsonSerializer.Deserialize<Update>(requestBody, JsonBotAPI.Options);
                    if (update == null)
                    {
                        _logger.LogError("Failed to deserialize request body to Update object.");
                        return BadRequest("Invalid request body.");
                    }
                    if (update.Type == UpdateType.InlineQuery)
                    {
                        await _botService.HandleInlineQueryAsync(update.InlineQuery);
                    }
                    else
                          if (update.Type == UpdateType.CallbackQuery)
                    {
                        await _botService.HandleCallbackQueryAsync(update);
                    }
                    else 
                    {
                        _logger.LogInformation("Received update: {@Update}", update);
                        await _botService.HandleUpdateAsync(update);
                    }

                    return Ok();
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "JSON deserialization error.");
                    //return BadRequest("Invalid JSON format.");
                    return Ok();
                }
            }
        }
    }
    [ApiController]
    [Route("api/[controller]")]
    public class TelegramTestController : ControllerBase
    {
        private readonly ITelegramBotService _botService;
        private readonly ILogger<TelegramController> _logger;
        public TelegramTestController(ITelegramBotService botService, ILogger<TelegramController> logger)
        {
            _botService = botService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            _logger.LogInformation("Received update: {@Update}", update);
            await _botService.HandleUpdateAsync(update);
            return Ok();
        }
    }
}