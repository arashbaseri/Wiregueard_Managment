using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSM.Domain.Entities
{
    public class UserEndpoint

    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EndpointId { get; set; }

    }
}
