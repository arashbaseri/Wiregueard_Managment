using Microsoft.EntityFrameworkCore;
using WSM.Domain.Entities;
using WSM.Domain.Repositories;
using WSM.Infrastructure.DatabaseContext;

namespace WSM.Infrastructure.Repositories
{
    public class MikrotikEndpointRepository : IMikrotikEndpointRepository
    {
        private readonly ILogger<MikrotikEndpointRepository> _logger;
        private readonly AppDbContext _dbContext;

        public MikrotikEndpointRepository(AppDbContext dbContext, ILogger<MikrotikEndpointRepository> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<int> CreateMikrotikEndpoint(MikrotikEndpoint newItem)
        {
            throw new NotImplementedException();
        }



        public async Task<int> UpdateMikrotikEndpointR(MikrotikEndpoint newItem)
        {
            throw new NotImplementedException();
        }
    }
}
