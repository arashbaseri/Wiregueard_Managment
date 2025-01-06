
using WSM.Domain.Entities;

namespace WSM.Domain.Repositories
{
    public interface IMikrotikEndpointRepository
    {
        Task<int> CreateMikrotikEndpoint(MikrotikEndpoint newItem);
        Task<int> UpdateMikrotikEndpointR(MikrotikEndpoint newItem);
        



    }
}
