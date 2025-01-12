using WSM.Domain.Entities;

namespace WSM.Domain.Repositories
{
    public interface IMikrotikCHRRepository
    {
        
        Task<int> CreateMikrotikCHR(MikrotikCHR mikrotikCHR);
        Task<int> UpdateMikrotikCHR(MikrotikCHR mikrotikCHR);
        Task<MikrotikCHR?> GetMikrotikCHRById(string id);
        Task<MikrotikCHR?> GetMikrotikCHRByWgInterface(string wgInterface);
        Task<List<MikrotikCHR>?> GetMikrotkCHRs ();
        Task<int> DeleteMikrotkCHR(MikrotikCHR mikrotikCHR);

        Task<bool> MikrotkCHRExists(string ipAddress);
    }
}
