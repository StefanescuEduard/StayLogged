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
            IEnumerable<string> logsToWrite;

            lock (Locker)
            {
                string[] currentLogs = File.ReadAllLines(logsFilePath);
                logsToWrite = currentLogs.Union(logs);
            }

            lock (Locker)
            {
                File.WriteAllLines(logsFilePath, logsToWrite);
            }
        }
    }
}
