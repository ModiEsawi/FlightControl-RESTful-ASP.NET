using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

//Server class
namespace FlightControlWeb.Models
{
    public class Server
    {
        [Key]
        [JsonPropertyName("ServerId")]
        [JsonProperty("ServerId")]
        [Required]
        public string Id { set; get; }

        [JsonPropertyName("ServerURL")]
        [JsonProperty("ServerURL")]
        [Required]
        public string Url { set; get; }
    }
}
