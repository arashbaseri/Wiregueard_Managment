
using WSM.Domain.Entities;
using WSM.Domain.ValueObjects;

namespace WSM.Domain.Repositories
{
    public interface IMikrotikEndpointRepository
    {
        Task<int> CreateMikrotikEndpoint(MikrotikEndpoint newItem);
        Task<int> UpdateMikrotikEndpoint(MikrotikEndpoint newItem);
        Task<List<MikrotikEndpoint>> GetMikrotikEndpointByTelegramId(long telegramId, string filterComment="");
        Task<MikrotikEndpoint?> GetMikrotikEndpointById(long telegramId, Guid id);
        Task<MikrotikEndpoint?> GetMikrotikEndpointByPublicKey(Base64EncodedKey publicKey);
        Task<List<string>> GetAvailableIpsAsync(string cidServer);
        Task<MikrotikEndpointMakeConfig> GetDataForConfig(string interfacename, string allowedIp);



    }
}
