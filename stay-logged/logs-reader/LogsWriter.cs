using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StayLogged.LogsReader
{
    public class LogsWriter
    {
        private readonly string[] logs;
        private const string LogsFilePath = "logs.txt";
        private static readonly object Locker = new object();


        public LogsWriter(string[] logs)
        {
            this.logs = logs;

            if (!File.Exists(LogsFilePath))
            {
                lock (Locker)
                {
                    using FileStream fileCreateStream = File.Create(LogsFilePath);
                }
            }
        }

        public void Write()
        {
            string fullLogsPath = Path.Combine(Environment.CurrentDirectory, LogsFilePath);
            IEnumerable<string> logsToWrite;

            lock (Locker)
            {
                string[] currentLogs = File.ReadAllLines(fullLogsPath);
                logsToWrite = currentLogs.Union(logs);
            }

            lock (Locker)
            {
                File.WriteAllLines(fullLogsPath, logsToWrite);
            }
        }
    }
}
