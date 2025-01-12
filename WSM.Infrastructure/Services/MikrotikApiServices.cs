using Serilog;
using System.Text;

using WSM.Application.Interfaces;
using WSM.Domain.Entities;

namespace WSM.Infrastructure.Services
{
    public class MikrotikApiService : IMikrotikApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MikrotikApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;


        }


        public async Task<MikrotikResponse> MikrotikSetComand(MikrotikCHR mikrotikCHR, string command, string body)
        {
            try
            {
                Log.Information("Entering SetDataAsync with command: {Command}", command);
                var client = _httpClientFactory.CreateClient("MyApiClient");
                var credentials = $"{mikrotikCHR.Username}:{mikrotikCHR.Password}";
                var byteArray = Encoding.ASCII.GetBytes(credentials);

                client.BaseAddress = new Uri($"http://{mikrotikCHR.IpAddress}:{mikrotikCHR.WWWPort}/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PutAsync(command, new StringContent(body, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                //Log.Information("Setting Mikrotik: {Command}", responseData);
                return new MikrotikResponse
                {
                    Id = Guid.NewGuid(),
                    Data = responseData,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred in MikrotikSetComand ");
                return new MikrotikResponse
                {
                    Id = Guid.NewGuid(),
                    Data = null,
                    Timestamp = DateTime.UtcNow
                };
            }
        }
        public async Task<MikrotikResponse> MikrotikApiFetch(MikrotikCHR mikrotikCHR, string command)
        {
            try
            {
                Log.Information("Entering FetchDataAsync with command: {Command}", command);
                var client = _httpClientFactory.CreateClient("MyApiClient");
                var credentials = $"{mikrotikCHR.Username}:{mikrotikCHR.Password}";
                var byteArray = Encoding.ASCII.GetBytes(credentials);

                client.BaseAddress = new Uri($"http://{mikrotikCHR.IpAddress}:{mikrotikCHR.WWWPort}/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var response = await client.GetAsync(command);
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                //Log.Information("Fetching from Mikrotik: {Command}", responseData);
                return new MikrotikResponse
                {
                    Id = Guid.NewGuid(),
                    Data = responseData,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred in MikrotikApiFetch ");
                return new MikrotikResponse
                {
                    Id = Guid.NewGuid(),
                    Data = null,
                    Timestamp = DateTime.UtcNow
                };
            }
        }
    }
}
