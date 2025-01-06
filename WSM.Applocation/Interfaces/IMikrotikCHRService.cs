using WSM.Application.DTOs;
using WSM.Domain.Entities;
namespace WSM.Application.Interfaces
{
    public interface IMikrotikCHRService
    {
        Task<OperationResult<Guid?>> CreateMikrotikCHR(MikrotikCHRCreateDto cardCreate);
        Task<int> DeleteMikrotikCHR(string id);
    }
}

