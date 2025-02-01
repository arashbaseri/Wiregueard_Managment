
using AutoMapper;
using Microsoft.Extensions.Logging;
using WSM.Application.DTOs;
using WSM.Application.Interfaces;
using WSM.Application.Services;
using WSM.Domain.Entities;
using WSM.Domain.Interfaces;
using WSM.Domain.Repositories;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using Telegram.Bot.Types;

namespace WSM.Application.Service
{

    public class WgLinuxService : IWgLinuxService
    {
        private readonly IWgKeyGenerationService _wgKeyGenerationService;
        private readonly IMikrotikEndpointRepository _mikrotikEndpointRepository;
        private readonly ILogger<MikrotikEndpointService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMikrotikCHRRepository _mikrotikCHRRepository;
        private readonly IMikrotikApiService _mikrotikApiService;
        private readonly IQrcodeGeneratorService _qrcodegeneratorService;

        public WgLinuxService(IWgKeyGenerationService wgKeyGenerationService, ILogger<MikrotikEndpointService> logger
            , IMikrotikEndpointRepository mikrotikEndpointRepository, IMapper mapper, IUserRepository userRepository
               , IMikrotikCHRRepository mikrotikCHRRepository, IMikrotikApiService mikrotikApiService
                , IQrcodeGeneratorService qrcodegeneratorService)
        {
            _wgKeyGenerationService = wgKeyGenerationService;
            _logger = logger;
            _mikrotikEndpointRepository = mikrotikEndpointRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _mikrotikCHRRepository = mikrotikCHRRepository;
            _mikrotikApiService = mikrotikApiService;
            _qrcodegeneratorService = qrcodegeneratorService;
        }

        public async Task<OperationResult<WgReadDto?>> CreatePeer(MikrotikEndpointCreateDto mikrotikEndpointCreateDto)
        {
            _logger.LogInformation($"Enter CreatePeer:");
            var (privateKey, publicKey) = _wgKeyGenerationService.GenerateWgKeyPair();
            _logger.LogInformation($"Private Key: {privateKey}");
            //check if public already exists
            var existingMikrotikEndpoint = await _mikrotikEndpointRepository.GetMikrotikEndpointByPublicKey(publicKey);
            if (existingMikrotikEndpoint != null)
            {
                _logger.LogInformation($"MikrotikEndpoint with the same public key already exists.");
                return OperationResult<WgReadDto?>.ErrorResult("MikrotikEndpoint with the same public key already exists.");
            }
            //get User Id
            var existUser = await _userRepository.GetUserByTelegramId(mikrotikEndpointCreateDto.TelegramId);
            if (existUser == null)
            {
                _logger.LogInformation($"User with the  telegram id does not exist.");
                return OperationResult<WgReadDto?>.ErrorResult("User  does not exist.");
            }
            if (existUser.CountRemaining == 0)
            {
                _logger.LogInformation($"user {existUser.UserName} capacity exceeded.");
                return OperationResult<WgReadDto?>.ErrorResult("User capacity exceeded.");
            }
            //get CHR Id
            var existMikrotikCHR = await _mikrotikCHRRepository.GetMikrotikCHRByWgInterface(mikrotikEndpointCreateDto.MikrotikInterface);
            if (existMikrotikCHR == null)
            {
                _logger.LogInformation($"server with the  interface={mikrotikEndpointCreateDto.MikrotikInterface} does not exist.");
                return OperationResult<WgReadDto?>.ErrorResult("CHR  does not exist.");
            }

            var CidServer = "10.10.10.0/24";
            var IPs = await _mikrotikEndpointRepository.GetAvailableIpsAsync(CidServer, existMikrotikCHR.Id);
            if (IPs == null)
            {
                _logger.LogInformation($"No available IPs.");
                return OperationResult<WgReadDto?>.ErrorResult("No available IPs.");
            }


            MikrotikEndpoint? mikrotikEndpoint = _mapper.Map<MikrotikEndpoint>(mikrotikEndpointCreateDto);

            mikrotikEndpoint.Id = Guid.NewGuid();
            mikrotikEndpoint.PublicKey = publicKey;
            mikrotikEndpoint.PrivateKey = privateKey;
            mikrotikEndpoint.UserId = existUser.Id;
            mikrotikEndpoint.MikrotikServerId = existMikrotikCHR.Id;
            mikrotikEndpoint.MikrotikInterface = existMikrotikCHR.DefaultWgInterface;
            mikrotikEndpoint.AllowedAddress = IPs.FirstOrDefault();
            WgCreateDto? newPeer = _mapper.Map<WgCreateDto>(mikrotikEndpoint);
            // create new MikrotikEndpoint in Mikrotik CHR
            var SShClient = new MikrotikSsh(
                                    existMikrotikCHR.IpAddress,
                                    existMikrotikCHR.Username,
                                    existMikrotikCHR.Password,
                                    existMikrotikCHR.WinboxPort ?? 22
                                );
            string msg = SShClient.CreatePeer(mikrotikEndpoint.PublicKey, mikrotikEndpoint.AllowedAddress);
            if (msg != "")
            {
                _logger.LogInformation("Response data is null.");
                return OperationResult<WgReadDto?>.ErrorResult($"Error creating Peer inlinux:{msg}");
            }

            var res =  new WgReadDto() { AllowedAddress = mikrotikEndpoint.AllowedAddress };

            var result = await _mikrotikEndpointRepository.CreateMikrotikEndpoint(mikrotikEndpoint);
            if (result == -1)
            {
                _logger.LogInformation("Error creating MikrotikEndpoint in the repository.");
                return OperationResult<WgReadDto?>.ErrorResult("Error creating MikrotikEndpoint in the repository.");
            }
            return OperationResult<WgReadDto?>.SuccessResult(res);
        }
    }
}
