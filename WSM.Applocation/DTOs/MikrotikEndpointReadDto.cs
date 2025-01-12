
using System.ComponentModel.DataAnnotations.Schema;
using WSM.Domain.Entities;
using WSM.Domain.ValueObjects;

namespace WSM.Application.DTOs
{
    public class MikrotikEndpointReadDto : BaseEntity
    {
        public Guid MikrotikServerId { get; set; }
        public Guid? UserId { get; set; }
        public string MikrotikInterface { get; set; }
        
        public IpAddress? AllowedAddress { get; set; }
        public string? name { get; set; }
        public string? Comment { get; set; }
        
        public Base64EncodedKey? PublicKey { get; set; }
        
        public Base64EncodedKey? PrivateKey { get; set; }
        public DateTime? RenewDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysToRenew { get; set; }
        public bool Disabled { get; set; }



    }
}