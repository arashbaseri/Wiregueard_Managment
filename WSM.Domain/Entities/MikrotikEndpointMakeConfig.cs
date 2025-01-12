using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSM.Domain.ValueObjects;

namespace WSM.Domain.Entities
{
    public class MikrotikEndpointMakeConfig

    {
        public string AllowedAddress { get; set; }

        public string PrivateKey { get; set; }
        public string ConfigEndPoint { get; set; }
        public int  ConfigEndPointPort { get; set; }
        public string ConfigPublicKey { get; set; }


    }
}
