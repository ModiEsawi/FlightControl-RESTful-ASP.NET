using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.ComTypes;
using FlightControlWeb.Repository.IRepository;
using FlightControlWeb.Models.Repository.IRepository;
using FlightControlWeb.Models;
using System.Web;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace FlightControlWeb.Controllers
{
    [Route("api/Flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightPlanRepository flightPlanRepo;
        private readonly IFlightsRepository flightsRepo;

        //ctor
        public FlightsController(IFlightPlanRepository flightPlanRepos
            , IFlightsRepository flightRepos)
        {
            this.flightPlanRepo = flightPlanRepos;
            this.flightsRepo = flightRepos;
        }

        //delete flightplan given it's id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlighPlanAsync(string id)
        {
            //check if it exists
            if (!await flightPlanRepo.FlightPlanExists(id))
            {
                ModelState.AddModelError("", "wrong flight plan id!");
                return StatusCode(404, ModelState);
            }
            bool deleted = await flightPlanRepo.DeletePlanFlgihtAsync(id);
            //bool deleted = false;
            if (!deleted) //error occured when trying to delete 
            {
                ModelState.AddModelError("","something wrong happened " +
                    "when trying to delete flight plan "+ id);
                return StatusCode(500, ModelState);
            }
            return NoContent(); //deleted successfully
        }

        //get all active flights
        [HttpGet]
        public async Task<IActionResult> GetFlight() 
        {
           var request = Request.Query; 
           //the request conatins relative_to key only
           if (request.Count ==1 && request.ContainsKey("relative_to"))
            {
                try
                {
                    //get relative_to value from a request 
                    var relativeDate = request["relative_to"].First();
                    //from string to datetime object
                    DateTime dt = DateTime.Parse(relativeDate, null,
                        System.Globalization.DateTimeStyles.RoundtripKind);
                    //get all active flights
                    var flights = this.flightsRepo.GetActiveFlights
                        (flightPlanRepo.GetAllFlightPlans(), dt);
                    return Ok(new DataMapper<Flight>() { Data = flights });
                }
                catch (Exception) //problem occured
                {
                    return BadRequest();
                }
            }
           //the request contains relative_to and sync_all keys
           else if (request.Count == 2 && request.ContainsKey("relative_to") && 
                request.ContainsKey("sync_all") && request["sync_all"] == "")
            {
                //get relative_to value from a request 
                var relativeDate = request["relative_to"].First();
                //from string to datetime object
                DateTime dt = DateTime.Parse(relativeDate, null, System.Globalization.DateTimeStyles.RoundtripKind);
                //get all internal active flights
                List<Flight> flights = this.flightsRepo.GetActiveFlights(flightPlanRepo.GetAllFlightPlans(), dt).ToList();
                //get all external active flights
                List<Flight> external_flights = (await this.flightsRepo.GetActiveExternalFlights(dt)).ToList();
                //concatenate two lists
                flights.AddRange(external_flights);
                return Ok(new DataMapper<Flight>() { Data = flights });
            }
            else
            {
                return BadRequest();
            }
           
        }
    }
}