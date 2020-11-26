using System;

namespace StayLogged.Domain
{
    public class Log
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime DateTime { get; set; }

        public string Source { get; set; }

        public string Description { get; set; }
    }
}