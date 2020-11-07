namespace StayLogged.LogsWriter
{
    public class Log
    {
        public LogType Type { get; }

        public string Data { get; }

        public Log(LogType type, string data)
        {
            Type = type;
            Data = data;
        }
    }
}