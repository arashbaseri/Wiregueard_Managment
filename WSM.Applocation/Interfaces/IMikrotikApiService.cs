using WSM.Domain.Entities;  
namespace WSM.Application.Interfaces
{
    public interface IMikrotikApiService
    {
        public Task<MikrotikResponse> MikrotikApiFetch(MikrotikCHR mikrotikCHR,string command);
    }
}