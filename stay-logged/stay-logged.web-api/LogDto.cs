using System;

namespace StayLogged.WebApi
{
    public class LogDto
    {
        public string Type { get; set; }

        public DateTime DateTime { get; set; }

        public string Source { get; set; }

        public string Description { get; set; }

        public string Ip { get; set; }
    }

    public class ChartLogDto
    {
        public string Ip { get; set; }

        public int Count { get; set; }
    }
}
