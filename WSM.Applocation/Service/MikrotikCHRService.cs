using Microsoft.Extensions.Options;
using AutoMapper;

using WSM.Domain.Entities;
using Microsoft.Extensions.Logging;
using WSM.Application.Interfaces;
using WSM.Domain.Repositories;
using WSM.Application.DTOs;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace WSM.Application.Services
{
    public class MikrotikCHRService : IMikrotikCHRService
    {
        private readonly IMikrotikCHRRepository _mikrotikCHRRepository;
        private readonly ILogger<MikrotikCHRService> _logger;
        private readonly IMapper _mapper;
        private readonly IMikrotikApiService _mikrotikApiService;
        public MikrotikCHRService(ILogger<MikrotikCHRService> logger, IMikrotikCHRRepository mikrotikCHRRepository,IMapper mapper
            , IMikrotikApiService mikrotikApiService)
        {
            _logger = logger;
            _mikrotikCHRRepository = mikrotikCHRRepository;
            _mapper = mapper;
            _mikrotikApiService = mikrotikApiService;
        }
        public async Task<OperationResult<Guid?>> CreateMikrotikCHR(MikrotikCHRCreateDto newCHR)
        {
            try
            {
                var existingCHR = await _mikrotikCHRRepository.MikrotkCHRExists(newCHR.IpAddress);
                if (existingCHR)
                {
                    return OperationResult<Guid?>.ErrorResult("CHR with the same name already exists.");
                }
            

                MikrotikCHR? mikrotikCHR = _mapper.Map<MikrotikCHR>(newCHR);
                mikrotikCHR.Id = Guid.NewGuid();
                mikrotikCHR.CreatedAt = DateTime.UtcNow;

                int result = await _mikrotikCHRRepository.CreateMikrotikCHR(mikrotikCHR);
                return result > 0 ? OperationResult<Guid?>.SuccessResult(mikrotikCHR.Id) : OperationResult<Guid?>.ErrorResult("Failed to create mikrotikCHR. Error in Database");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikCHRService)}.{nameof(CreateMikrotikCHR)}");
                return OperationResult<Guid?>.ErrorResult("An unexpected error occurred.");
            }
        }

        public async Task<int> DeleteMikrotikCHR(string id)
        {
            throw new NotImplementedException();
        }

        
    }
}
