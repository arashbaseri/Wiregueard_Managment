using Microsoft.EntityFrameworkCore;
using WSM.Domain.Entities;
using WSM.Domain.Repositories;
using WSM.Domain.ValueObjects;
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
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.MikrotikEndpoints.AddAsync(newItem);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    var user = await _dbContext.Users.FindAsync(newItem.UserId);
                    if (user != null && user.CountRemaining > 0)
                    {
                        user.CountRemaining -= 1; // Update user table as needed
                        _dbContext.Users.Update(user);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error occurred in the method {nameof(MikrotikEndpointRepository)}.{nameof(CreateMikrotikEndpoint)}--{ex.Message}");
                return -1;
            }
        }



        public async Task<int> UpdateMikrotikEndpoint(MikrotikEndpoint newItem)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MikrotikEndpoint>> GetMikrotikEndpointByTelegramId(long telegramId, string filterComment = "")
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
            if (user != null && user.CountRemaining == -1)
            {
                var queryAdmin = _dbContext.MikrotikEndpoints.AsQueryable();
         
                if (!string.IsNullOrEmpty(filterComment))
                {
                    queryAdmin = queryAdmin.Where(endpoint => EF.Functions.Like(endpoint.Comment, $"%{filterComment}%"));
                }

                return await queryAdmin.ToListAsync();
                

            }
            else
            {
                var query = _dbContext.MikrotikEndpoints
                    .Join(_dbContext.Users,
                          endpoint => endpoint.UserId,
                          user => user.Id,
                          (endpoint, user) => new { endpoint, user })
                    .Where(joined => joined.user.TelegramId == telegramId);

                if (!string.IsNullOrEmpty(filterComment))
                {
                    query = query.Where(joined => EF.Functions.Like(joined.endpoint.Comment, $"%{filterComment}%"));
                }

                return await query.Select(joined => joined.endpoint).ToListAsync();
            }
        }

        public async Task<MikrotikEndpoint?> GetMikrotikEndpointById(long telegramId, Guid id)
        {
            return await _dbContext.MikrotikEndpoints
                .Join(_dbContext.Users,
                      endpoint => endpoint.UserId,
                      user => user.Id,
                      (endpoint, user) => new { endpoint, user })
                .Where(joined => joined.user.TelegramId == telegramId && joined.endpoint.Id == id)
                .Select(joined => joined.endpoint)
                .FirstOrDefaultAsync();
        }

        public async Task<MikrotikEndpoint?> GetMikrotikEndpointByPublicKey(Base64EncodedKey publicKey)
        {
            try
            {
                return await Task.Run(() => _dbContext.MikrotikEndpoints
                          .AsEnumerable() // Switch to client-side evaluation
                          .FirstOrDefault(endpoint => endpoint.PublicKey.ToString() == publicKey.ToString()));


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikEndpointRepository)}.{nameof(GetMikrotikEndpointByPublicKey)}--{ex.Message}");
                return null;

            }
        }
        public async Task<List<string>> GetAvailableIpsAsync(string cidServer)
        {

            try
            {
                var result = await _dbContext.Database.SqlQuery<string>($"SELECT * FROM get_available_ips({cidServer})").ToListAsync();
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikEndpointRepository)}.{nameof(GetMikrotikEndpointByPublicKey)}--{ex.Message}");
                return null;

            }
        }
        public async Task<MikrotikEndpointMakeConfig> GetDataForConfig(string interfacename, string allowedIp)
        {
            try
            {
                var result = await _dbContext.MikrotikEndpoints
                    .Join(_dbContext.Users,
                          endpoint => endpoint.UserId,
                          user => user.Id,
                          (endpoint, user) => new { endpoint, user })
                    .Join(_dbContext.MikrotikCHRs,
                          joined => joined.endpoint.MikrotikServerId,
                          chr => chr.Id,
                          (joined, chr) => new { joined.endpoint, chr })
                    .Where(joined => joined.endpoint.MikrotikInterface == interfacename && joined.endpoint.AllowedAddress.Value == allowedIp)
                    .Select(joined => new MikrotikEndpointMakeConfig
                    {
                        AllowedAddress = joined.endpoint.AllowedAddress.Value,
                        PrivateKey = joined.endpoint.PrivateKey.EncodedKey,
                        ConfigEndPoint = joined.chr.ConfigEndPoint,
                        ConfigEndPointPort = joined.chr.ConfigEndPointPort,
                        ConfigPublicKey = joined.chr.ConfigPublicKey
                    })
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikEndpointRepository)}.{nameof(GetDataForConfig)}--{ex.Message}");
                return null;
            }
        }
        public async Task<MikrotikEndpointMakeConfig> GetDataForConfig(Guid id)
        {
            try
            {
                var result = await _dbContext.MikrotikEndpoints
                    .Join(_dbContext.Users,
                          endpoint => endpoint.UserId,
                          user => user.Id,
                          (endpoint, user) => new { endpoint, user })
                    .Join(_dbContext.MikrotikCHRs,
                          joined => joined.endpoint.MikrotikServerId,
                          chr => chr.Id,
                          (joined, chr) => new { joined.endpoint, chr })
                    .Where(joined => joined.endpoint.Id == id)
                    .Select(joined => new MikrotikEndpointMakeConfig
                    {
                        AllowedAddress = joined.endpoint.AllowedAddress.Value,
                        PrivateKey = joined.endpoint.PrivateKey.EncodedKey,
                        ConfigEndPoint = joined.chr.ConfigEndPoint,
                        ConfigEndPointPort = joined.chr.ConfigEndPointPort,
                        ConfigPublicKey = joined.chr.ConfigPublicKey
                    })
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikEndpointRepository)}.{nameof(GetDataForConfig)}--{ex.Message}");
                return null;
            }
        }
    }
}
