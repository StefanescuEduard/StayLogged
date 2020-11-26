using Microsoft.AspNetCore.Mvc;
using StayLogged.WebApi.Services;
using System.Collections.Generic;

namespace StayLogged.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly LogsService logsService;

        public LogsController(LogsService logsService)
        {
            this.logsService = logsService;
        }

        [HttpGet]
        public IActionResult GetLogs()
        {
            IEnumerable<LogDto> logs = logsService.ReadLogs();

            return Ok(logs);
        }

        [HttpGet("{type}")]
        public IActionResult GetChartData(string type)
        {
            IEnumerable<ChartLogDto> chartLogs = logsService.GetErrorLogs(type);

            return Ok(chartLogs);
        }
    }
}
