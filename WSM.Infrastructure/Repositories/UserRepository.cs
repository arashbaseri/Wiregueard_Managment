using Microsoft.EntityFrameworkCore;
using WSM.Domain.Entities;
using WSM.Domain.Repositories;
using WSM.Infrastructure.DatabaseContext;



namespace WSM.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<MikrotikCHRRepository> _logger;
        private readonly AppDbContext _dbContext;

        public UserRepository(ILogger<MikrotikCHRRepository> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByTelegramId(long telegramId)
        {
            try
            {
                return await _dbContext.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
                    


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in the method {nameof(User)}.{nameof(GetUserByTelegramId)}");
                _logger.LogError($"Error is :{ex.Message}");
                throw;
            }
        }
    }
}
