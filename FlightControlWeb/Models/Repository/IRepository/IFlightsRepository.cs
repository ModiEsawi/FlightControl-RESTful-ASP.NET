using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Repository.IRepository
{
    public interface IFlightsRepository
    {

        /*
         * Given collection of flightplans , return active flights 
         * accordings to the given date. - internal flightplans-
         */
        ICollection<Flight> GetActiveFlights
            (ICollection<FlightPlan> flightPlans, DateTime relativeTo);
        
        /*
         * get active flights plans according to a given date from all servers.
         */
        Task<ICollection<Flight>> GetActiveExternalFlights(DateTime relativeTo);

    }
}
