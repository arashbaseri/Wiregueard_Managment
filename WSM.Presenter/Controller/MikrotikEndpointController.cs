using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using WSM.Application.Interfaces;

using Swashbuckle.AspNetCore.Annotations;
using WSM.Application.DTOs;
using WSM.Application.Services;

namespace WSM.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MikrotikEndpointController : ControllerBase
    {
        private readonly IMikrotikEndpointService _mikrotikEndpointService;
        private readonly ILogger<MikrotikEndpointController> _logger;

        public MikrotikEndpointController(IMikrotikEndpointService mikrotikEndpointServicee, ILogger<MikrotikEndpointController> logger)
        {
            _mikrotikEndpointService = mikrotikEndpointServicee;
            _logger = logger;
        }



        [HttpPost("generate-qrcode")]
        [SwaggerOperation(Summary = "Generate QR code for Mikrotik endpoint", Description = "Generates a QR code for the specified Mikrotik endpoint.")]
        [SwaggerResponse(200, "QR code generated successfully", typeof(FileStreamResult))]
        [SwaggerResponse(404, "Endpoint not found")]
        [SwaggerResponse(401, "Invalid token")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> GenerateMikrotikEndpointQrcode([FromBody] WgReadDto wgReadDto)
        {
            try
            {
                if (wgReadDto is null)
                {
                    return BadRequest("Invalid input");
                }

                var result = await _mikrotikEndpointService.GetMikrotikEndpointQrcode(wgReadDto);
                if (result.Success && result.Data != null)
                {
                    result.Data.Position = 0;
                    return File(result.Data, "image/png", "qrcode.png");
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while generating QR code in {nameof(MikrotikEndpointController)}.{nameof(GenerateMikrotikEndpointQrcode)}");
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message ?? "Internal server error");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Get all auth card", Description = "Retrieves a list of all auth card.")]
        [SwaggerResponse(200, "List of card retrieved successfully", typeof(MikrotikEndpointCreateDto))]
        [SwaggerResponse(404, "Card not found")]
        [SwaggerResponse(401, "Invalid token")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> CreateMikrotikEndpoint([FromBody] MikrotikEndpointCreateDto mikrotikEndpointCreateDto)
        {
            try
            {
                if (mikrotikEndpointCreateDto is null)
                {
                    return BadRequest("Invalid input");
                }

                var result = await _mikrotikEndpointService.CreateMikrotikEndpoint(mikrotikEndpointCreateDto);
                if (result.Success)
                {
                    return Created(string.Empty, new { id = result.Data.ToString(), message = "Endpoint added successfully" });
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating a new Endpoint in {nameof(MikrotikEndpointController)}.{nameof(CreateMikrotikEndpoint)}");
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message ?? "Internal server error");
            }
        }
    }



}