using WSM.Application.DTOs;
using WSM.Domain.Entities;
namespace WSM.Application.Interfaces
{

    public interface IMikrotikEndpointService
    {

        Task<OperationResult<WgReadDto?>> CreateMikrotikEndpoint(MikrotikEndpointCreateDto mikrotikEndpointCreateDto);
        Task<OperationResult<MemoryStream?>> GetMikrotikEndpointQrcode(WgReadDto wgReadDto);
        Task<string> GetInterfaceIp(MikrotikCHR chr);
        Task<OperationResult<MemoryStream?>> GetMikrotikEndpointQrcode(Guid id);
    }
}
