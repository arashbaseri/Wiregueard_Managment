using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSM.Domain.Entities
{
    public class User:BaseEntity
    {
        
        public string UserName { get; set; }
        public long TelegramId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string?  Email { get; set; }
  
        public int CountRemaining { get; set; }


    }

}
