using System.Text.Json.Serialization;

namespace FlightControlWeb.Models.DTOs
{
    // "partial" class of Segment class , that not require all Segment properties.
    public class SegmentDto
    {
        [JsonPropertyName("longitude")]
        public double Longitude { set; get; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("timespan_seconds")]
        public int TimeSpanInSeconds { get; set; }

        //initialize Segment DTO from a given Segment object/
        public SegmentDto(Segment segment)
        {
            if (segment != null)
            {
                this.Longitude = segment.Longitude;
                this.Latitude = segment.Latitude;
                this.TimeSpanInSeconds = segment.TimeSpanInSeconds;
            }
        }
    }
}