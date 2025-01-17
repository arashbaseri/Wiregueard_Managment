
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
namespace WSM.Application.Service
{
    public class MikrotikEndpointService : IMikrotikEndpointService
    {
        private readonly IWgKeyGenerationService _wgKeyGenerationService;
        private readonly IMikrotikEndpointRepository _mikrotikEndpointRepository;
        private readonly ILogger<MikrotikEndpointService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMikrotikCHRRepository _mikrotikCHRRepository;
        private readonly IMikrotikApiService _mikrotikApiService;
        private readonly IQrcodeGeneratorService _qrcodegeneratorService
            ;



        public MikrotikEndpointService(IWgKeyGenerationService wgKeyGenerationService, ILogger<MikrotikEndpointService> logger
                    , IMikrotikEndpointRepository mikrotikEndpointRepository, IMapper mapper,IUserRepository userRepository
                       ,IMikrotikCHRRepository mikrotikCHRRepository,IMikrotikApiService mikrotikApiService
                        ,IQrcodeGeneratorService qrcodegeneratorService)
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

        public async Task<OperationResult<WgReadDto?>> CreateMikrotikEndpoint(MikrotikEndpointCreateDto mikrotikEndpointCreateDto)
        {
            _logger.LogInformation($"Enter CreateMikrotikEndpoint:");
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
            if (existUser.CountRemaining == 0 )
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

            var CidServer = await GetInterfaceIp(existMikrotikCHR);
            var IPs = await _mikrotikEndpointRepository.GetAvailableIpsAsync(CidServer);
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
            mikrotikEndpoint.AllowedAddress= IPs.FirstOrDefault();
            WgCreateDto? newPeer = _mapper.Map<WgCreateDto>(mikrotikEndpoint);
            // create new MikrotikEndpoint in Mikrotik CHR
            var wgCreateDtoJson = JsonSerializer.Serialize(newPeer);
            var response = await _mikrotikApiService.MikrotikPutSetComand(existMikrotikCHR, "rest/interface/wireguard/peers", wgCreateDtoJson);

            if (response.Data == null)
            {
                _logger.LogInformation("Response data is null.");
                return OperationResult<WgReadDto?>.ErrorResult("Error creating MikrotikEndpoint in Mikrotik CHR: Response data is null.");
            }

            var res= JsonSerializer.Deserialize<WgReadDto>(response.Data);
            if (res.Id == null)
            {
                _logger.LogInformation($"Error creating MikrotikEndpoint in Mikrotik CHR.");
                return OperationResult<WgReadDto?>.ErrorResult("Error creating MikrotikEndpoint in Mikrotik CHR.");
            }

            var result=await _mikrotikEndpointRepository.CreateMikrotikEndpoint(mikrotikEndpoint);
            if (result == -1)
            {
                _logger.LogInformation("Error creating MikrotikEndpoint in the repository.");
                return OperationResult<WgReadDto?>.ErrorResult("Error creating MikrotikEndpoint in the repository.");
            }
            return OperationResult<WgReadDto?>.SuccessResult(res);
        }
        
        public async Task<OperationResult<MemoryStream?>> GetMikrotikEndpointQrcode(WgReadDto wgReadDto)
        {
            var configClass = await _mikrotikEndpointRepository.GetDataForConfig(wgReadDto.MikrotikInterface, wgReadDto.AllowedAddress);

            string config = "[Interface]\n" +
                            $"PrivateKey = {configClass.PrivateKey}\n" +
                            $"Address = {configClass.AllowedAddress}\n" +
                            "DNS = 8.8.8.8, 4.2.2.1, 1.1.1.1, 4.2.2.4\n\n" +
                            "[Peer]\n" +
                            $"PublicKey = {configClass.ConfigPublicKey}\n" +
                            "AllowedIPs = 0.0.0.0/0\n" +
                            $"Endpoint = {configClass.ConfigEndPoint}:{configClass.ConfigEndPointPort}\n" +
                            "PersistentKeepalive = 21";
            _logger.LogInformation($"QR code config: {config}");
            var qrcode = await _qrcodegeneratorService.GenerateQrCodeAsync(config);
            if (qrcode == null)
            {
                _logger.LogInformation("Error generating QR code.");
                return OperationResult<MemoryStream?>.ErrorResult("Error generating QR code.");
            }

            return OperationResult<MemoryStream?>.SuccessResult(qrcode);
        }
        public async Task<OperationResult<MemoryStream?>> GetMikrotikEndpointQrcode(Guid id)
        {
            var configClass = await _mikrotikEndpointRepository.GetDataForConfig(id);

            string config = "[Interface]\n" +
                            $"PrivateKey = {configClass.PrivateKey}\n" +
                            $"Address = {configClass.AllowedAddress}\n" +
                            "DNS = 8.8.8.8, 4.2.2.1, 1.1.1.1, 4.2.2.4\n\n" +
                            "[Peer]\n" +
                            $"PublicKey = {configClass.ConfigPublicKey}\n" +
                            "AllowedIPs = 0.0.0.0/0\n" +
                            $"Endpoint = {configClass.ConfigEndPoint}:{configClass.ConfigEndPointPort}\n" +
                            "PersistentKeepalive = 21";
            _logger.LogInformation($"QR code config: {config}");
            var qrcode = await _qrcodegeneratorService.GenerateQrCodeAsync(config);
            if (qrcode == null)
            {
                _logger.LogInformation("Error generating QR code.");
                return OperationResult<MemoryStream?>.ErrorResult("Error generating QR code.");
            }

            return OperationResult<MemoryStream?>.SuccessResult(qrcode);
        }
        public async Task<string> GetInterfaceIp(MikrotikCHR chr)
        {
            string body = "{\".query\":[\"interface=" + chr.DefaultWgInterface + "\"]}";
            var response = await _mikrotikApiService.MikrotikPostSetComand(chr, "rest/ip/address/print", body);
            if (response.Data == null)
            {
                _logger.LogError($"Error in getting interface ip for {chr.IpAddress}");
                return "";
            }
            if (response.Data == "[]")
            {
                _logger.LogError($" interface ip not found for  {chr.IpAddress}_{chr.DefaultWgInterface}");
                return "";
            }
            var res = JsonSerializer.Deserialize<List<InterfaceIPReadDto>>(response.Data);
            if (res != null && res[0].Id == null)
            {
                _logger.LogInformation($"Error in JsonSerializer from MikrotikCHR");
                return "";
            }
            return res[0].Address;
        }


    }
}
