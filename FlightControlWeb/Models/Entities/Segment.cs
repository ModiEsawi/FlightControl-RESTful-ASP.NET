using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

//Segment class
namespace FlightControlWeb.Models
{
    public class Segment
    {
        [Key]
        public int Id { set; get; }
        [JsonPropertyName("longitude")]
        [JsonProperty("longitude")]

        public double Longitude { set; get; }
        [JsonPropertyName("latitude")]
        [JsonProperty("latitude")]

        public double Latitude { get; set; }
        [JsonPropertyName("timespan_seconds")]
        [JsonProperty("timespan_seconds")]

        public int TimeSpanInSeconds { get; set; }
    }
}
