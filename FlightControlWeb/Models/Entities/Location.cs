using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
    public class Location
    {
        [Key]
        public int Id { set; get; }

        [JsonPropertyName("longitude")]
        [JsonProperty("longitude")]
        public double Longitude { set; get; }

        [JsonPropertyName("latitude")]
        [JsonProperty("latitude")]
        public double Latitude { set; get; }

        [JsonPropertyName("date_time")]
        [JsonProperty("date_time")]
        public DateTime Date { set; get; }
    }
}
