using FlightControlWeb.Models;
using FlightControlWeb.Models.DTOs;
using FlightControlWeb.Models.Helpers;
using FlightControlWeb.Models.Repository.IRepository;
using FlightControlWeb.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Repository
{
    /*
     * class that handle flightplans functions . 
     * implement IFlightPlanRepository.
     */
    public class FlightPlanRepository : IFlightPlanRepository
    {
        private readonly DatabaseContext db;
        private readonly IServerRepository serverRepo;

        //ctor
        public FlightPlanRepository(DatabaseContext db, IServerRepository serverRepo)
        {
            this.db = db;
            this.serverRepo = serverRepo;
        }

        /*
         * create flightplan in database from a given flightplan object.
         * return true if action success , false otherwise.
         */
        public async Task<bool> CreateFlightPlan(FlightPlan flightplan)
        {
            //gen a random id.
            flightplan.Id = IDGenerator.GenerateFlightPlanId(flightplan.InitialLocation.Date, flightplan.CompanyName);
            await db.FlightPlans.AddAsync(flightplan); //trying to create it db.
            return await SaveChanges();
        }

        /*
        * Given a flightplan id, return object from db that match this id.
        * return : null , if there is no flightplan with this id , and there is no
        * server hold this flightplan, 
        * return the flightplan  with this id otherwise.
        * async function.
        */
        public async Task<FlightPlan> GetFlightPlanAsync(string flightPlanId)
        {
            //get the appropriate flightplan from db.
            FlightPlan fp = db.FlightPlans.Include(d => d.Segments) //get it's segments from Segment table
                .Include(d=>d.InitialLocation) //get it's location from Locations table
                .Where(r => r.Id == flightPlanId).FirstOrDefault(); //match the id/
            if (fp == null) //there is no match
            {
                //try to get it from another server.
                fp = await GetExternalFlightPlan(flightPlanId); 
            }
            return fp;
        }

        /*
         * get all flightplans from database.
        */
        public ICollection<FlightPlan> GetAllFlightPlans()
        {
            return db.FlightPlans.Include(d => d.Segments)
                .Include(d => d.InitialLocation).ToList();
        }

        /*
         * given a flightplan's id , return true if it exists in db , false otherwise.
         */
        public async Task<bool> FlightPlanExists(string flightPlanId)
        {
            return await db.FlightPlans.AnyAsync(a => a.Id.ToLower().Trim()
            == flightPlanId.ToLower().Trim());
        }

        /*
         * given a flightplan's id , return true if obejct deleted successfully
         * , false otherwise.
        */
        public async Task<bool> DeletePlanFlgihtAsync(string flightPlanId)
        {
            if (!await FlightPlanExists(flightPlanId)) //check if it exists.
                return false;
            var flightPlan = await GetFlightPlanAsync(flightPlanId); //get flightplan
            //delete the location from Location table
            db.Location.Remove(flightPlan.InitialLocation); 
            //delete all segments from Segment table
            foreach(var segment in flightPlan.Segments)
                db.Segment.Remove(segment);
            //delete the flightplan from Flightplans table.
            db.FlightPlans.Remove(flightPlan);
            return await SaveChanges();
        }

        /*
         * given a flightplan id, return flightplan with this id,
         * that exist in any server.
         */
        private async Task<FlightPlan> GetExternalFlightPlan(string flightPlanId)
        {
            //send the query to all servers
            List<string> responses = (await serverRepo.GetRequestToAllServersAsync
                ("api/flightplan/" + flightPlanId)).ToList();
            //if there is no server hold flightplan with this id
            if (responses.Count == 0)
                return null;
            //build flightplan from returned json string.
            FlightPlan flightPlan = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightPlan>(responses[0]);
            return flightPlan;
        }

        /*
         * save changes in database.
         */
        private async Task<bool> SaveChanges()
        {
            try
            {
                return await db.SaveChangesAsync() >= 0 ? true : false; //save changes in db.
            }
            catch (DbUpdateException) //problem in saving the object.
            {
                return false;
            }
        }

    }
}
