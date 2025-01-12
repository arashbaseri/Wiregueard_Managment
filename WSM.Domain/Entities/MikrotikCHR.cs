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
        public string DefaultWgInterface { get; set; }
        public string ConfigEndPoint { get; set; }
        public int ConfigEndPointPort { get; set; }
        public string ConfigPublicKey { get; set; }


    }
}
