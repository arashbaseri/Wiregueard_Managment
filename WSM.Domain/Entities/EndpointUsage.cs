using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSM.Domain.Entities
{
    public class EndpointUsage:BaseEntity

    {
        public Guid? MikrotikEndpointId { get; set; }
        public Guid MikrotikServerId { get; set; }

        public long BytesIn { get; set; }
        public long BytesOut { get; set; }
        public long? PacketsIn { get; set; }
        public long? PacketsOut { get; set; }
        public long? BytesTotal { get; set; }
        public long? PacketsTotal { get; set; }
        public bool? IsReset { get; set; }
    }
}
