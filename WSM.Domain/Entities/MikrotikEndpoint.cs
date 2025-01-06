using System.ComponentModel.DataAnnotations.Schema;
using WSM.Domain.ValueObjects;

namespace WSM.Domain.Entities
{
    public class MikrotikEndpoint : BaseEntity
    {
        public Guid MikrotikServerId { get; set; }
        public Guid? UserId { get; set; }
        public string MikrotikInterface { get; set; }
        [Column(TypeName = "varchar")]
        public IpAddress AllowedAddress { get; set; }
        public string? name { get; set; }
        public string? Comment { get; set; }
        [Column(TypeName = "varchar")]
        public Base64EncodedKey? PublicKey { get; set; }
        [Column(TypeName = "varchar")]
        public Base64EncodedKey? PrivateKey { get; set; }
        public DateTime? RenewDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysToRenew { get; set; }
        public bool Disabled { get; set; }



    }
}
