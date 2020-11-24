﻿using System.Diagnostics;

namespace StayLogged.LogsWriter
{
    public class Log
    {
        public string Type { get; }


        public string Data { get; }

        public Log(LogType type, string data)
        {
            Type = type.ToString();
            Data = data;
        }
        public Log(EventLogEntryType type, string data)
        {
            Type = type.ToString();
            Data = data;
        }
    }
}