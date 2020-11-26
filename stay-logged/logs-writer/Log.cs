using System.Diagnostics;

namespace StayLogged.LogsWriter
{
    public class Log
    {
        public string Type { get; }


        public string Data { get; }

        public string Source { get; }

        public string Date { get; }

        
        public Log(string type, string data,string source)
        {
            Type = type.ToString();
            Data = data;
            Source = source;
        }
    }
}