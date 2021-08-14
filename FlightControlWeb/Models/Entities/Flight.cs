using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
namespace FlightControlWeb.Models
{
    public class Flight
    {
        [JsonPropertyName("flight_id")]
        [JsonProperty("flight_id")]
        public string FlightPlanId { get; set; }

        [JsonPropertyName("longitude")]
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("latitude")]
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("passengers")]
        [JsonProperty("passengers")]
        public int Passengers { get; set; }

        [JsonPropertyName("company_name")]
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonPropertyName("date_time")]
        [JsonProperty("date_time")]
        public DateTime DateTime { set; get; }

        [JsonPropertyName("is_external")]
        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }

        // default constructor ; for empty object
        public Flight(){}

        // given flight plan , initliaze it's compatible flight
        public Flight(FlightPlan flightPlan)
        {
            this.FlightPlanId = flightPlan.Id;
            this.Longitude = 0;
            this.Latitude = 0;
            this.Passengers = flightPlan.Passengers;
            this.CompanyName = flightPlan.CompanyName;
            this.DateTime = flightPlan.InitialLocation.Date;
            this.IsExternal = false;
        }
    }
}
