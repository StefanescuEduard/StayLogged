using Microsoft.EntityFrameworkCore;
using stay_logged.data_access;
using StayLogged.Domain;
using System.Collections.Generic;
using System.Linq;

namespace StayLogged.DataAccess
{
    public class LogRepository
    {
        private readonly StayLoggedContext stayLoggedContext;

        public LogRepository()
        {
            var contextFactory = new StayLoggedContextFactory();
            stayLoggedContext = contextFactory.CreateDbContext(new[] { string.Empty });
        }

        public IEnumerable<Log> ReadAllLogs()
        {
            return stayLoggedContext.Logs.Include(l => l.Host).AsEnumerable();
        }

        public void Add(Log log)
        {
            stayLoggedContext.Logs.Add(log);

            stayLoggedContext.SaveChanges();
        }

        public void AddRange(List<Log> logs)
        {
            stayLoggedContext.Logs.AddRange(logs);

            stayLoggedContext.SaveChanges();
        }
    }
}
