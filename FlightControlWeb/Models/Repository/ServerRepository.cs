using FlightControlWeb.Models.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Repository
{
    /*
     * ServerRepository class.
     * handle servers functions . implement IServerRepository interface.
     */
    public class ServerRepository : IServerRepository
    {
        private readonly DatabaseContext db;
        private static readonly HttpClient client = new HttpClient();

        //ctor
        public ServerRepository(DatabaseContext db)
        {
            this.db = db;
        }

        /*
         * create server in database from a given Server object.
         * return true if action success , false otherwise.
         */
        public async Task<bool> AddServer(Server server)
        {
            await db.Servers.AddAsync(server);
            return await SaveChanges();
        }

        /*
         * five serverID , delete this server from db.
         * returns true if server deleted successfully, false otherwise.
        */
        public async Task<bool> DeleteServer(string serverId)
        {
            if (! await ServerIsExistAsync(serverId)) //check if it exists
                return false;
            var server = await GetServer(serverId); //get the server
            db.Servers.Remove(server);
            return await SaveChanges();
        }


        /*
        * return all servers from db.
        */
        public async Task<ICollection<Server>> GetAllServers()
        {
            return await db.Servers.ToListAsync();
        }

        /*
         * return specific server that match a given id.
         */
        private async Task<Server> GetServer(string serverId)
        {
            return await db.Servers.FirstOrDefaultAsync<Server>(s => s.Id == serverId);
        }

        /*
        * given serverID , check if it exists in db,
        * if exists return true, false otherwise.
        */
        public async Task<bool> ServerIsExistAsync(string serverId)
        {
            return await db.Servers.AnyAsync(a => a.Id.ToLower().Trim() 
            == serverId.ToLower().Trim()); 
        }

        /*
         * given a server and url , sends GET request to this server,
         * and return the returened response.
         */
        private async Task<string> GetRequestToServerAsync(Server server
            , string route)
        {
            try
            {
                //string to = "http://" + server.Url + "/" + route;
                string to =  server.Url + "/" + route;
                //send
                HttpResponseMessage response = await client.GetAsync(to);
                response.EnsureSuccessStatusCode();
                //get body as string
                string responseBody = await response 
                    .Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
        * given a url , sends GET request to all servers,
        * and return all responses,
        */
        public async Task<ICollection<string>> GetRequestToAllServersAsync(string route)
        {
            int timeout = 2500;
            //get all servers
            List<Server> servers = (await GetAllServers()).ToList();
            // to keep all responses
            List<string> responses = new List<string>();
            //send GET request to each server
            foreach (Server server in servers)
            {
                var task = GetRequestToServerAsync(server,route);
                //wait maximum 2.5 seconds if there is no answer.
                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    var result = task.Result; //response.
                    if (result != null) 
                        responses.Add(task.Result);
                }
            }
            return responses;
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
