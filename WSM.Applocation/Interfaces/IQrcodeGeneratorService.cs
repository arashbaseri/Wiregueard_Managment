using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSM.Application.Interfaces
{
    public interface IQrcodeGeneratorService

    {
        Task<MemoryStream?> GenerateQrCodeAsync(string text);
    }
}
