using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace FlightControlWeb.Models
{
    public class FlightPlan
    {
        [Key]
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { set; get; }

        [JsonPropertyName("company_name")]
        [JsonProperty("company_name")]
        [Required]
        public string CompanyName { set; get; }
 
        [Required]
        [JsonPropertyName("passengers")]
        [JsonProperty("passengers")]
        public int Passengers { set; get; }

        [Required]
        [JsonPropertyName("initial_location")]
        [JsonProperty("initial_location")]
        [ForeignKey("initialLocationId")]
        public Location InitialLocation { set; get; }

        [Required]
        [JsonPropertyName("segments")]
        [JsonProperty("segments")]
        public ICollection<Segment> Segments { set; get; }
    }
}
