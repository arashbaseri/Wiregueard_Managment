using WSM.Domain.Entities;  
namespace WSM.Application.Interfaces
{
    public interface IMikrotikApiService
    {
         Task<MikrotikResponse> MikrotikApiFetch(MikrotikCHR mikrotikCHR,string command);
         Task<MikrotikResponse> MikrotikPutSetComand(MikrotikCHR mikrotikCHR, string command,string body);
        Task<MikrotikResponse> MikrotikPostSetComand(MikrotikCHR mikrotikCHR, string command, string body);
    }
}