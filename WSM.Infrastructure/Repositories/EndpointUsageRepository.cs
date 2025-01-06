//using ProjectAndTaskManagement.Application.Interfaces;
//using ProjectAndTaskManagement.Application.Services;
//using ProjectAndTaskManagement.Application.Utilities;

using WSM.Domain.Entities;
using WSM.Domain.Repositories;
using WSM.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Npgsql;
namespace WSM.Infrastructure.Repositories
{
    public class EndpointUsageRepository : IEndpointUsageRepository
    {
        private readonly ILogger<EndpointUsageRepository> _logger;
        private readonly AppDbContext _dbContext;

        public EndpointUsageRepository(ILogger<EndpointUsageRepository> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> InsertAll(string endpointUsage, string ipAddress)
        {
            try
            {
                var result = await _dbContext.Database.ExecuteSqlRawAsync(
                    "SELECT insert_endpoint_usages({0}::jsonb,{1})", endpointUsage, ipAddress);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(EndpointUsageRepository)}.{nameof(InsertAll)}: {ex.Message}");
                // Log the connection string to verify the database name
                var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
                _logger.LogInformation($"Connected to database: {connectionString}");
                return 0;
            }

        }

    //public async Task<Card?> GetCard(string id)
    //{
    //    try
    //    {
    //          return await _dbContext.Cards.FindAsync(Guid.Parse(id));
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"Error occurred in the method {nameof(EndpointUsageRepository)}.{nameof(GetCard)}");
    //        throw;
    //    }
    //}

    public async Task<int> Create(EndpointUsage endpointUsage)
    {
        try
        {
            _dbContext.EndpointUsages.Add(endpointUsage);
            return await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred in the method {nameof(EndpointUsageRepository)}.{nameof(Create)}");
            return 0;
        }
    }


}
}
