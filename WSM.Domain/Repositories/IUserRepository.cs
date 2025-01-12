
using WSM.Domain.Entities;


namespace WSM.Domain.Repositories
{

    public interface IUserRepository
    {

        Task<User?> GetUserByTelegramId(long telegramId);

    }
}
