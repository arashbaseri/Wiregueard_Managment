using Microsoft.EntityFrameworkCore;
using WSM.Domain.Entities;
using WSM.Domain.Repositories;
using WSM.Infrastructure.DatabaseContext;
namespace WSM.Infrastructure.Repositories
{
    public class MikrotikCHRRepository : IMikrotikCHRRepository
    {
        private readonly ILogger<MikrotikCHRRepository> _logger;
        private readonly AppDbContext _dbContext;

        public MikrotikCHRRepository(AppDbContext dbContext, ILogger<MikrotikCHRRepository> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<bool> MikrotkCHRExists(string ipAddress)
        {
            try
            {

                return await _dbContext.MikrotikCHRs.AnyAsync(u => u.IpAddress == ipAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikCHRRepository)}.{nameof(MikrotkCHRExists)}");
                return false;
            }
        }

        public async Task<int> CreateMikrotikCHR(MikrotikCHR mikrotikCHR)
        {
            try
            {
                _dbContext.MikrotikCHRs.Add(mikrotikCHR);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikCHRRepository)}.{nameof(CreateMikrotikCHR)}");
                _logger.LogError($"Error is :{ex.Message}");
                return 0;
            }
        }

        public async Task<int> DeleteMikrotkCHR(MikrotikCHR mikrotikCHR)
        {
            try
            {
                _dbContext.MikrotikCHRs.Remove(mikrotikCHR);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikCHRRepository)}.{nameof(DeleteMikrotkCHR)}");
                _logger.LogError($"Error is :{ex.Message}");
                return 0;
            }
        }

        public async Task<MikrotikCHR?> GetMikrotikCHRById(string id)
        {
            try
            {
                return await _dbContext.MikrotikCHRs.FindAsync(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikCHRRepository)}.{nameof(DeleteMikrotkCHR)}");
                _logger.LogError($"Error is :{ex.Message}");
                throw;
            }
        }

        public async Task<List<MikrotikCHR>?> GetMikrotkCHRs()
        {
            try
            {
                return await _dbContext.MikrotikCHRs.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikCHRRepository)}.{nameof(GetMikrotkCHRs)}");
                _logger.LogError($"Error is :{ex.Message}");
                return null;
            }
        }

        public async Task<int> UpdateMikrotikCHR(MikrotikCHR mikrotikCHR)
        {
            try
            {
                _dbContext.MikrotikCHRs.Update(mikrotikCHR);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikCHRRepository)}.{nameof(GetMikrotkCHRs)}");
                _logger.LogError($"Error is :{ex.Message}");
                return 0;
            }
        }
        public async Task<MikrotikCHR?> GetMikrotikCHRByWgInterface(string wgInterface)
        {

            try
            {
                var mikrotikCHR = await _dbContext.MikrotikCHRs
                    .FirstOrDefaultAsync(chr => chr.DefaultWgInterface == wgInterface);

                if (mikrotikCHR == null)
                {
                    _logger.LogWarning($"No MikrotikCHR found with DefaultWgInterface: {wgInterface}");
                    // You can choose to return null, throw an exception, or return a default value
                    return null; // or throw new EntityNotFoundException("MikrotikCHR not found");
                }

                return mikrotikCHR;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(MikrotikCHRRepository)}.{nameof(GetMikrotikCHRByWgInterface)}");
                _logger.LogError($"Error is :{ex.Message}");
                throw;
            }


        }

    }
}