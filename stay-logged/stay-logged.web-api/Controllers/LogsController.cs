using Microsoft.AspNetCore.Mvc;
using StayLogged.Domain;
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
            IEnumerable<Log> logs = logsService.ReadLogs();

            return Ok(logs);
        }
    }
}
