using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StayLogged.LogsReader
{
    public class LogsWriter
    {
        private readonly string[] logs;
        private string logsFilePath;
        private static readonly object Locker = new object();


        public LogsWriter(string[] logs)
        {
            this.logs = logs;

            string logsDirectoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "logs-reader");
            logsFilePath = Path.Combine(logsDirectoryPath, "logs.txt");

            if (!Directory.Exists(logsDirectoryPath))
            {
                lock (Locker)
                {
                    Directory.CreateDirectory(logsDirectoryPath);
                }
            }

            if (!File.Exists(logsFilePath))
            {
                lock (Locker)
                {
                    using FileStream fileCreateStream = File.Create(logsFilePath);
                }
            }
        }

        public void Write()
        {
            string fullLogsPath = Path.Combine(Environment.CurrentDirectory, logsFilePath);
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
