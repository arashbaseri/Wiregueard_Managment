using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSM.Domain.Interfaces
{
    public interface IWgKeyGenerationService
    {
        (string PrivateKey, string PublicKey) GenerateWgKeyPair();
    }

}
