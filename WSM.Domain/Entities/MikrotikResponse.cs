namespace WSM.Domain.Entities
{
    public class MikrotikResponse
    {
        public Guid Id { get; set; }
        public string? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
}