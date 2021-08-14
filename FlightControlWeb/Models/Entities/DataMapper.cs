using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
    public class DataMapper<T>
    {
        [JsonPropertyName("data")]
        public ICollection<T> Data { get; set; }
    }
}
