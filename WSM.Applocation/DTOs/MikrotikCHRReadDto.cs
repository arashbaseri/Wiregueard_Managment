
namespace WSM.Application.DTOs
{
    public class MikrotikCHRReadDto
    {
        public Guid id { get; set; }


        public string? Name { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int WinboxPort { get; set; }
        public int WWWPort { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
