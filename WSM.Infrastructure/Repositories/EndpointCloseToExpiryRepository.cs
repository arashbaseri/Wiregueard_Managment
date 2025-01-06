using Microsoft.EntityFrameworkCore;
using WSM.Domain.Entities;
using WSM.Domain.Repositories;
using WSM.Infrastructure.DatabaseContext;
namespace WSM.Infrastructure.Repositories
{
    public class EndpointCloseToExpiryRepository : IEndpointCloseToExpiryRepository

    {
        private readonly ILogger<EndpointCloseToExpiryRepository> _logger;
        private readonly AppDbContext _dbContext;

        public EndpointCloseToExpiryRepository(AppDbContext dbContext, ILogger<EndpointCloseToExpiryRepository> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<List<EndpointCloseToExpiry>?> GetItemsCloseToExpire(TimeSpan threshold)
        {
            try
            {
                var result = await _dbContext.EndpointCloseToExpiry
                    .FromSqlRaw("SELECT * FROM Get_Endpoints_Close_To_Expire({0})", threshold)
                    .ToListAsync();

                if (result == null)
                {
                    _logger.LogWarning("No items found close to expire.");
                    return null;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikEndpointRepository)}.{nameof(GetItemsCloseToExpire)}: {ex.Message}");
                // Log the connection string to verify the database name
                var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
                _logger.LogInformation($"Connected to database: {connectionString}");
                return null;
            }
        }


    }
}
