
namespace WSM.Application.DTOs
{
    public class MikrotikCHRCreateDto
    {
        public string? Name { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? WinboxPort { get; set; }
        public int? WWWPort { get; set; }
    }
}