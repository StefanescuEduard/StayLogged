using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace StayLogged.LogsWriter
{
    public class LogsReader
    {
        private readonly string operatingSystem;

        public LogsReader(string operatingSystem)
        {
            this.operatingSystem = operatingSystem;
        }

        public List<Log> Read()
        {
            var logs = new List<Log>();

            if (operatingSystem.Equals("linux"))
            {
                logs.Add(new Log(LogType.Info, File.ReadAllText("/var/log/messages")));
                logs.Add(new Log(LogType.Error, File.ReadAllText("/var/log/syslog")));
            }
            else if (operatingSystem.Equals("windows"))
            {
                logs.Add(new Log(LogType.Info, GetWindowsMessages(EventLogEntryType.Information)));
                logs.Add(new Log(LogType.Error, GetWindowsMessages(EventLogEntryType.Error)));
            }

            return logs;
        }

        private static string GetWindowsMessages(EventLogEntryType entryType)
        {
            EventLog[] eventLogs = EventLog.GetEventLogs();

            return string.Join(Environment.NewLine, eventLogs.Where(e => e.Source == "Application")
                .SelectMany(eventLog => eventLog.Entries.Cast<EventLogEntry>(),
                    (eventLog, eventLogEntry) => new { eventLog, eventLogEntry })
                .Where(@t => @t.eventLogEntry.EntryType == entryType)
                .Select(@t => @t.eventLogEntry.Message).ToList());
        }
    }
}