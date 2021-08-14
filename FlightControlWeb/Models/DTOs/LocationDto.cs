using System;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models.DTOs
{
    // "partial" class of Location class , that not require all Location properties.

    public class LocationDto
    {
        [JsonPropertyName("longitude")]
        public double Longitude { set; get; }
        [JsonPropertyName("latitude")]
        public double Latitude { set; get; }
        [JsonPropertyName("date_time")]
        public DateTime Date { set; get; }

        //initialize a locationDTO from a given location object.
        public LocationDto(Location location)
        {
            if (location!= null) {
                this.Longitude = location.Longitude;
                this.Latitude = location.Latitude;
                this.Date = location.Date;
            }

        }
    }
}
