using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSM.Application.DTOs;
using WSM.Domain.Entities;

namespace WSM.Application.Interfaces
{
    public interface IWgLinuxService
    {
        Task<OperationResult<WgReadDto?>> CreatePeer(MikrotikEndpointCreateDto mikrotikEndpointCreateDto);

    }
}
