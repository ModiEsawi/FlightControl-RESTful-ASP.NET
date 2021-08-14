using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Repository.IRepository
{
    /*
    * IServerRepository class.
    * handels servers functions 
    */
    public interface IServerRepository
    {
         /*
         * create server in database from a given Server object.
         * return true if action success , false otherwise.
         */
        Task<bool> AddServer(Server server);

        /*
         * return all servers from db.
         */
        Task<ICollection<Server>> GetAllServers();

        /*
         * given serverID , check if it exists in db,
         * if exists return true, false otherwise.
         */
        Task<bool> ServerIsExistAsync(string serverId);

        /*
         * five serverID , delete this server from db.
         * returns true if server deleted successfully, false otherwise.
         */
        Task<bool> DeleteServer(string serverId);

        /*
         * given a url , sends GET request to all servers,
         * and return all responses
         */
        Task<ICollection<string>> GetRequestToAllServersAsync(string route);
    }
}
