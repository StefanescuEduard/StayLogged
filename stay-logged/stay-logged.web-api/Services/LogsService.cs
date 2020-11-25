using StayLogged.WebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace StayLogged.WebApi.Services
{
    public class LogsService
    {
        public IEnumerable<Log> ReadLogs()
        {
            string logsFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "logs-reader",
                "logs.txt");

            var logs = new List<Log>();
            if (File.Exists(logsFilePath))
            {
                string[] readLogs = File.ReadAllLines(logsFilePath);


                foreach (var log in readLogs)
                {
                    if (string.IsNullOrEmpty(log.Trim()))
                    {
                        continue;
                    }

                    logs.Add(new Log
                    {
                        MachineName = GetElementFromLog(log, "<", ">"),
                        LogType = GetElementFromLog(log, "> ", ": "),
                        Message = GetMessage(log)
                    });
                }

                logs.Reverse();
            }

            return logs;
        }

        private string GetElementFromLog(string log, string fromCharacter, string toCharacter)
        {
            int from = log.IndexOf(fromCharacter, StringComparison.InvariantCultureIgnoreCase);
            int to = log.IndexOf(toCharacter, StringComparison.InvariantCultureIgnoreCase);

            if (!CanSubstring(from, to, log))
            {
                return string.Empty;
            }

            return log.Substring(from, to - from).Trim('<', '>');
        }

        private string GetMessage(string log)
        {
            int from = log.IndexOf(": ", StringComparison.InvariantCultureIgnoreCase);

            if (!CanSubstring(from, log.Length - 1, log))
            {
                return string.Empty;
            }

            return log.Substring(from, log.Length - from).TrimStart(':', ' ');
        }

        private bool CanSubstring(int from, int to, string log)
        {
            return from != -1 && to != -1 && from < log.Length && to < log.Length;
        }
    }
}
