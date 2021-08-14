using FlightControlWeb.Models;
using FlightControlWeb.Models.DTOs;
using FlightControlWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlightControlWeb.Controllers
{
    [Route("api/flightplan")]
    [ApiController]
    public class FlightPlanController : Controller
    {
        private readonly IFlightPlanRepository flightPlanRepo;

        //ctor
        public FlightPlanController(IFlightPlanRepository flightPlanRepository)
        {
            this.flightPlanRepo = flightPlanRepository;
        }

        //given id , return flightplan with that id/
        [HttpGet("{id}",Name ="GetFlightPlan")]
        public async Task<IActionResult> CreateFlightPlan(string id)
        {
            var fp = await flightPlanRepo.GetFlightPlanAsync(id);
            if(fp == null) //id does not match any flightplan
            {
                ModelState.AddModelError("", "wrong flight plan id!");
                return StatusCode(404, ModelState);
            }
            return Ok(new FlightPlanDto(fp));
        }


        //create new flightplan,given flightplan object(in json) in req's body
        [HttpPost]
        public async Task<IActionResult> CreateFlightPlan
            ([FromBody] FlightPlan fp)
        {
            var created = await this.flightPlanRepo.CreateFlightPlan(fp);
            if (!created) //problem in creating flightplan
            {
                ModelState.AddModelError("", "Flight plan was not created!");
                return StatusCode(500, ModelState);
            }
            //created successfully, returns 201 and the object
            return CreatedAtRoute("GetFlightPlan", new { id = fp.Id },
                new FlightPlanDto(fp));
        }
    }
}