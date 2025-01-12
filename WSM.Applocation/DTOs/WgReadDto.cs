

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WSM.Application.DTOs
{
    //this class is used for reading data after creating a peer in  mikrotikto valid creation is happend in mikrotik.
    // and for showing Qrcode is generated for the peer. It is Input of the GetMikrotikEndpointQrcode method in IMikrotikEndpointService
    public class WgReadDto
    {
        [Required]
        [JsonPropertyName(".id")]
        public string Id { get; set; }
        [Required]
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        [JsonPropertyName("allowed-address")]
        public string AllowedAddress { get; set; }
        //[JsonPropertyName("client-endpoint")]
        //string ClientEndpoint { get; set; }
        //[JsonPropertyName("current-endpoint-address")]
        //string CurrentEndpointAddress { get; set; }
        //[JsonPropertyName("current-endpoint-port")]
        //string CurrentEndpointPort { get; set; }
        //[JsonPropertyName("disabled")]
        //string Disabled { get; set; }
        //[JsonPropertyName("name")]
        //string Name { get; set; }
        //[JsonPropertyName("preshared-key")]
        //string PresharedKey { get; set; }

        //[JsonPropertyName("preshared-key")]
        //string PrivateKey { get; set; }
        //[JsonPropertyName("private-key")]
        //string PublicKey { get; set; }
        //[JsonPropertyName("rx")]
        //string rx { get; set; }
        [JsonPropertyName("interface")]
        public string MikrotikInterface { get; set; }   


    }

}
