using WSM.Domain.Entities;

namespace WSM.Domain.Repositories
{
    public interface IEndpointUsageRepository
    {
        Task<int> InsertAll(string endpointUsage,string ipAddress);
        Task<int> Create(EndpointUsage endpointUsage);
    }
}
