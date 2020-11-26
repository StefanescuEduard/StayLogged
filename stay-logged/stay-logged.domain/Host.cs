using System.Collections.Generic;

namespace StayLogged.Domain
{
    public class Host
    {
        public string Name { get; set; }

        public string Ip { get; set; }

        public List<Log> Logs { get; set; }
    }
}
