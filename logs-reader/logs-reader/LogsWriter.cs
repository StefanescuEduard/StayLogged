using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StayLogged.LogsReader
{
    public class LogsWriter
    {
        private readonly string[] logs;

        public LogsWriter(string[] logs)
        {
            this.logs = logs;
        }

        public void Write()
        {
            const string logsFile = "logs.txt";
            string logsPath = Path.Combine(Environment.CurrentDirectory, logsFile);
            string[] currentLogs = File.ReadAllLines(logsPath);
            IEnumerable<string> logsToWrite = currentLogs.Intersect(logs);

            File.WriteAllLines(logsPath, logsToWrite);
        }
    }
}