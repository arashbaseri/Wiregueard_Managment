using WSM.Application.DTOs;
using WSM.Domain.Entities;
namespace WSM.Application.Interfaces
{

    public interface IMikrotikEndpointService
    {

        Task<OperationResult<Guid?>> CreateMikrotikEndpoint(MikrotikEndpointCreateDto mikrotikEndpointCreateDto);
        Task<OperationResult<MemoryStream?>> GetMikrotikEndpointQrcode(WgReadDto wgReadDto);

    }
}
