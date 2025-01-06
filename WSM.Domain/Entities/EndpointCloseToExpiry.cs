using System.ComponentModel.DataAnnotations.Schema;
using WSM.Domain.ValueObjects;

namespace WSM.Domain.Entities
{
    public class EndpointCloseToExpiry
    {
        
        public string MikrotikInterface { get; set; }
        [Column(TypeName = "varchar")]
        public IpAddress AllowedAddress { get; set; }
        
        //public DateTime? RenewDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public int? DaysToRenew { get; set; }
        //public bool Disabled { get; set; }
        public long TelegramId { get; set; }



    }
}
