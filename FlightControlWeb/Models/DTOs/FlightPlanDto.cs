using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models.DTOs
{
    // "partial" class of flightplan class , that not require all flightplan properties.
    public class FlightPlanDto
    {
        [JsonPropertyName("company_name")]
        public string CompanyName { set; get; }
        [JsonPropertyName("passengers")]
        public int Passengers { set; get; }
        [JsonPropertyName("initial_location")]
        public LocationDto InitialLocation { set; get; }
        [JsonPropertyName("segments")]
        public ICollection<SegmentDto> Segments { set; get; }

        //initialize flightplan DTO from a given flightplan
        public FlightPlanDto(FlightPlan flightPlan)
        {
            this.CompanyName = flightPlan.CompanyName;
            this.Passengers = flightPlan.Passengers;

            this.InitialLocation = new LocationDto( flightPlan.InitialLocation);
            this.Segments = new List<SegmentDto>();
            if(flightPlan.Segments != null)
            {
                foreach (var segment in flightPlan.Segments)
                    this.Segments.Add(new SegmentDto(segment));
            }

        }
    }
}
