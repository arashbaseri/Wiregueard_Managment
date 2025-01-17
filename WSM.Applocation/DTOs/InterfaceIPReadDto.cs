using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WSM.Application.DTOs
{
    public class InterfaceIPReadDto
    {
        [Required]
        [JsonPropertyName(".id")]
        public string Id { get; set; }
        [Required]
        [JsonPropertyName("actual-interface")]
        public string ActualInterface { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
    }
}
