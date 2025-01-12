using WSM.Application.Interfaces;
using WSM.Domain.Interfaces;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WSM.Infrastructure.Services
{
    public class QrcodeGeneratorService:IQrcodeGeneratorService
    {
        public Task<MemoryStream?> GenerateQrCodeAsync(string text)
        {
  
            if (string.IsNullOrEmpty(text))
            {
                Log.Information("Text for creating QrCode is not define");
                return null;
            }

            try
            {
                var qrCode = new QRCodeGenerator();
                var qrCodeData = qrCode.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                var qrCodeImage = new PngByteQRCode(qrCodeData);
                //var qrCodeImageAsBitmap = qrCodeImage.GetGraphic(20);
                var qrCodeImageAsBytes = qrCodeImage.GetGraphic(1);
                var memoryStream = new MemoryStream(qrCodeImageAsBytes);
                memoryStream.Position = 0;
                return Task.FromResult(memoryStream);
       
            }
            catch (Exception ex)
            {
                Log.Information("Error in generating Qrcode. {}",ex.Message);
                return null;
            }
        }
    }   
}
