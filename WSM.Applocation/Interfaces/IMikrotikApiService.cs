using WSM.Domain.Entities;  
namespace WSM.Application.Interfaces
{
    public interface IMikrotikApiService
    {
         Task<MikrotikResponse> MikrotikApiFetch(MikrotikCHR mikrotikCHR,string command);
         Task<MikrotikResponse> MikrotikSetComand(MikrotikCHR mikrotikCHR, string command,string body);
    }
}