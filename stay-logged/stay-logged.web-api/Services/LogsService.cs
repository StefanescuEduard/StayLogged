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

        public IEnumerable<ChartLogDto> GetErrorLogs(string type)
        {
            var logs = ReadLogs().Where(l => l.Type == type).GroupBy(l => l.Ip);
            var chart = new List<ChartLogDto>();

            foreach (var log in logs)
            {
                var foundLog = chart.FirstOrDefault(l => l.Ip == log.Key);

                if (foundLog != null)
                {
                    foundLog.Count++;
                }
                else
                {
                    foundLog = new ChartLogDto
                    {
                        Ip = log.Key,
                        Count = 1
                    };

                    chart.Add(foundLog);
                }
            }

            return chart;
        }
    }
}
