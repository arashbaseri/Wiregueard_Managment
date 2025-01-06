using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using WSM.Application.Interfaces;

using Swashbuckle.AspNetCore.Annotations;
using WSM.Application.DTOs;

namespace WSM.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MikrotikCHRController : ControllerBase
    {
        private readonly IMikrotikCHRService _mikrotikCHRService;
        private readonly ILogger<TelegramController> _logger;
        public MikrotikCHRController(IMikrotikCHRService mikrotikCHRService, ILogger<TelegramController> logger)
        {
            _mikrotikCHRService = mikrotikCHRService;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Get all auth card", Description = "Retrieves a list of all auth card.")]
        [SwaggerResponse(200, "List of card retrieved successfully", typeof(MikrotikCHRCreateDto))]
        [SwaggerResponse(404, "Card not found")]
        [SwaggerResponse(401, "Invalid token")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> CreateMikrotikCHR([FromBody] MikrotikCHRCreateDto mikrotikCHRCreateDto)
        {
            try
            {
                if (mikrotikCHRCreateDto is null)
                {
                    return BadRequest("Invalid input");
                }

                var result = await _mikrotikCHRService.CreateMikrotikCHR(mikrotikCHRCreateDto);
                if (result.Success)
                {
                    return Created(string.Empty, new { id = result.Data.ToString(), message = "MikrotikCHR added successfully" });
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating a new MikrotikCHR in {nameof(MikrotikCHRController)}.{nameof(CreateMikrotikCHR)}");
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message ?? "Internal server error");
            }
        }
    }
}