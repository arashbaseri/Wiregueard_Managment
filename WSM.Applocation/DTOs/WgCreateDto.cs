using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WSM.Domain.Entities;
using WSM.Domain.ValueObjects;

namespace WSM.Application.DTOs
{
    public class WgCreateDto
    {
        [JsonPropertyName("interface")]
        public string MikrotikInterface { get; set; }
        [JsonPropertyName("allowed-address")]
        public string AllowedAddress { get; set; }
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        [JsonPropertyName("public-key")]
        public string PublicKey { get; set; }

        

        

    }
}