using FlightControlWeb.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Repository
{
    /*
     * FlightsRepository class.
     * handle flights functions . implement IFlightsRepository intergace.
     */
    public class FlightsRepository : IFlightsRepository
    {
        private readonly IServerRepository serverRepo;

        //ctor
        public FlightsRepository(IServerRepository serverRepo)
        {
            this.serverRepo = serverRepo;
        }

        /*
         * Given collection of flightplans , return active flights 
         * accordings to the given date. - internal flightplans-
         */
        public ICollection<Flight> GetActiveFlights
            (ICollection<FlightPlan> flightPlans, DateTime relativeTo)
        {
            //get flightplans that active now .
            List<FlightPlan> activeFlightPlans = GetActiveFlightPlans(flightPlans, relativeTo).ToList();
            //to keep active flights
            List<Flight> activeFlights = new List<Flight>();
            //extract flight object from active flightplan
            foreach (FlightPlan flightPlan in activeFlightPlans)
            {
                Flight temp = GetActiveFlight(flightPlan, relativeTo);
                if((temp.Latitude>= -90 && temp.Latitude <=90) && (temp.Longitude >= -180 && temp.Longitude <= 180))
                    activeFlights.Add(temp);
            }
            return activeFlights;
        }


        /*
         * Given collection of flightplans and dateTame, 
         * return active flightplans according to "relativeTo" datetime.
         */
        private ICollection<FlightPlan> GetActiveFlightPlans
            (ICollection<FlightPlan> flightPlans, DateTime relativeTo)
        {
            List<FlightPlan> activeFlightPlans = new List<FlightPlan>();
            //iterate over each flightplan
            foreach (FlightPlan flightPlan in flightPlans)
            {
                //flight start time.
                DateTime initialDt = flightPlan.InitialLocation.Date;
                if (relativeTo < initialDt)  //It's not started yet 
                    continue; //not active
                DateTime finishDt = initialDt;
                //count finish time of a flight
                foreach (Segment segment in flightPlan.Segments)
                    finishDt = finishDt.AddSeconds(segment.TimeSpanInSeconds);
                if (relativeTo < finishDt) //it's not finished yet
                    activeFlightPlans.Add(flightPlan); //so it's active
            }
            return activeFlightPlans;
        }

        /*
         * extract active flight from a given flightplan
         */
        private Flight GetActiveFlight(FlightPlan flightPlan
            , DateTime dateTime)
        {
            List<Segment> segments = flightPlan.Segments.ToList();
            if (segments.Count == 0) //there is no segments.
                return null;
            Flight flight = new Flight(flightPlan); //initialize
            double startSegmentPointLat = 0, startSegmentPointLong = 0;
            double endSegmentPointLat = 0, endSegmentPointLong = 0;
            int currentSigment = 0;
            DateTime tempDate = flightPlan.InitialLocation.Date;
            for(int i=0; i<segments.Count; i++)
            { //iterate over each segment , found active segment.
                tempDate = tempDate.AddSeconds(segments[i].TimeSpanInSeconds);
                if (tempDate < dateTime)
                    continue;
                if (i == 0) 
                {
                   startSegmentPointLat = flightPlan.InitialLocation.Latitude;
                   startSegmentPointLong = flightPlan.InitialLocation.Longitude;
                }
                else
                {
                    startSegmentPointLat = segments[i - 1].Latitude;
                    startSegmentPointLong = segments[i - 1].Longitude;
                }
                endSegmentPointLat = segments[i].Latitude;
                endSegmentPointLong = segments[i].Longitude;
                currentSigment = i;
                break;
            }
            //time passed in segment as datetime object
            var timePassed = (dateTime - tempDate.AddSeconds
                (-1 * segments[currentSigment].TimeSpanInSeconds));
            //time passes in segment as seconds.
            double timePassedInSegment = timePassed.TotalSeconds;
            double pathCompletePercentage = timePassedInSegment 
                / segments[currentSigment].TimeSpanInSeconds;
            //calculate rightnow longitude and latitude
            flight.Longitude = startSegmentPointLong + pathCompletePercentage
                * (endSegmentPointLong - startSegmentPointLong);
            flight.Latitude = startSegmentPointLat + pathCompletePercentage 
                * (endSegmentPointLat - startSegmentPointLat);
            return flight; 
        }

        /*
        * get active flights plans according to a given date from all servers.
        */
        public async Task<ICollection<Flight>> GetActiveExternalFlights
            (DateTime relativeTo)
        {
            //from datetime object to string
            string dateString = relativeTo.ToString("yyyy-MM-dd'T'HH:mm:ssK"
                , CultureInfo.InvariantCulture);
            //send to servers and wait for responses
            List<string> responses = (await serverRepo
                .GetRequestToAllServersAsync("api/flights?relative_to="
                + dateString)).ToList();
            //to keep flights
            List<Flight> activeFlights = new List<Flight>();
            //get the flight from a response
            foreach (string response in responses)
            {
                //from flight to response
                DataMapper<Flight>  flights = Newtonsoft.Json.JsonConvert
                    .DeserializeObject<DataMapper<Flight>>(response);
                foreach (Flight flight in flights.Data)
                    flight.IsExternal = true;
                //add to list
                activeFlights.AddRange(flights.Data);
            }
            return activeFlights;
        }
    }




}
