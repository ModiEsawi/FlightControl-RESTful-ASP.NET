using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using FlightControlWeb.Models.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/servers")]
    [ApiController]
    public class ServersController : Controller
    {
        private readonly IServerRepository serverRepos;

        //ctor
        public ServersController(IServerRepository serverRepos)
        {
            this.serverRepos = serverRepos;
        }

        //given server object(from json) from string , create it in db.
        [HttpPost]
        public async Task<IActionResult> AddServer([FromBody] Server server)
        {
            bool added = await this.serverRepos.AddServer(server);
            if (!added) //problem occured in creating the object
            {
                ModelState.AddModelError("", "server was not added!");
                return StatusCode(500, ModelState);
            }
            //created successfully.
            return CreatedAtRoute("GetAllServers", server);
        }

        //get all servers
        [HttpGet(Name ="GetAllServers")]
        public async Task<IActionResult> GetAllServers()
        {
            var servers = await this.serverRepos.GetAllServers();
            return Ok(servers);
        }

        //delete specific server given it's id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(string id)
        {
            //check if it exists
            if (!await serverRepos.ServerIsExistAsync(id))
            {
                ModelState.AddModelError("", "wrong server id!");
                return StatusCode(404, ModelState);
            }
            //try to delete
            if (!await serverRepos.DeleteServer(id))
            {
                ModelState.AddModelError("", "something wrong happened when trying to delete the server "+id);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}