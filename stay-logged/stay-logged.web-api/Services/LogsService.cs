using StayLogged.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace StayLogged.WebApi.Services
{
    public class LogsService
    {
        private readonly HostRepository hostRepository;

        public LogsService()
        {
            hostRepository = new HostRepository();
        }

        public IEnumerable<LogDto> ReadLogs()
        {
            return hostRepository.GetHosts()
                .SelectMany(host => host.Logs,
                    (host, hostLog) => new LogDto
                    {
                        Type = hostLog.Type,
                        Ip = host.Ip,
                        DateTime = hostLog.DateTime,
                        Description = hostLog.Description,
                        Source = hostLog.Source
                    }).ToList();
        }

        public IEnumerable<ChartLogDto> GetErrorLogs()
        {
            var logs = ReadLogs().GroupBy(l => l.Ip);
            var chart = new List<ChartLogDto>();


            return chart;
        }
    }
}
