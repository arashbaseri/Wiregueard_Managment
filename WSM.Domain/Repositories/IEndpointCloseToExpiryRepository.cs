
using WSM.Domain.Entities;

namespace WSM.Domain.Repositories
{
    public interface IEndpointCloseToExpiryRepository
    {
        Task<List<EndpointCloseToExpiry>?> GetItemsCloseToExpire(TimeSpan threshold);



    }
}
