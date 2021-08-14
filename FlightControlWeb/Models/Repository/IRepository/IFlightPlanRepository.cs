using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Repository.IRepository
{
    /*
     * IFlightPlanRepository class.
     * handels flight plans functions 
     */
    public interface IFlightPlanRepository
    {
        /*
         * create flightplan in database from a given flightplan object.
         * return true if action success , false otherwise.
         */
        Task<bool> CreateFlightPlan(FlightPlan flightplan);

        /*
        * Given a flightplan id, return object from db that match this id.
        * return : null , if there is no flightplan with this id , and there is no
        * server hold this flightplan, 
        * return the flightplan  with this id otherwise.
        * async function.
         */
        Task<FlightPlan> GetFlightPlanAsync(string flightPlanId);

        /*
         * get all flightplans from database.
         */
        ICollection<FlightPlan> GetAllFlightPlans();

        /*
         * given a flightplan's id , return true if it exists in db , false otherwise.
         */
        Task<bool> FlightPlanExists(string flightPlanId);

        /*
         * given a flightplan's id , return true if obejct deleted successfully
         * , false otherwise.
         */
        Task<bool> DeletePlanFlgihtAsync(string flightPlanId);

      
    }
}
