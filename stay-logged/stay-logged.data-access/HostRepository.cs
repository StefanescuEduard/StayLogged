using Microsoft.EntityFrameworkCore;
using stay_logged.data_access;
using StayLogged.Domain;
using System.Collections.Generic;
using System.Linq;

namespace StayLogged.DataAccess
{
    public class HostRepository
    {
        private readonly StayLoggedContext stayLoggedContext;

        public HostRepository()
        {
            var contextFactory = new StayLoggedContextFactory();
            stayLoggedContext = contextFactory.CreateDbContext(new[] { string.Empty });
        }

        public Host GetHost(string ip)
        {
            return stayLoggedContext
                .Hosts
                .Include(h => h.Logs)
                .SingleOrDefault(h => h.Ip == ip);
        }

        public IEnumerable<Host> GetHosts()
        {
            return stayLoggedContext
                .Hosts
                .Include(h => h.Logs);
        }

        public void Add(Host host)
        {
            stayLoggedContext.Hosts.Add(host);

            stayLoggedContext.SaveChanges();
        }

        public void UpdateLog(Host host, Log log)
        {
            host.Logs.Add(log);

            stayLoggedContext.SaveChanges();
        }
    }
}
