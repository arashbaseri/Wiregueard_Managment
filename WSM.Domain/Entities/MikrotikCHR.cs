using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSM.Domain.Entities
{
    public class MikrotikCHR:BaseEntity
    {
        
        public string? Name { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? WinboxPort { get; set; }
        public int? WWWPort { get; set; }
        

    }
}
