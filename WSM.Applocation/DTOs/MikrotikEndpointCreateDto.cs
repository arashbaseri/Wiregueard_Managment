using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WSM.Domain.Entities;
using WSM.Domain.ValueObjects;

namespace WSM.Application.DTOs
{
    public class MikrotikEndpointCreateDto 
    {
       
        public long TelegramId { get; set; }

        public string MikrotikInterface { get; set; }
    
     //   public IpAddress AllowedAddress { get; set; }
        
        public string Comment { get; set; }
        
    //    public Base64EncodedKey PublicKey { get; set; }
        
  //      public Base64EncodedKey PrivateKey { get; set; }
        
        public int? DaysToRenew { get; set; }
        

    }
}
