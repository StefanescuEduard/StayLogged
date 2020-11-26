using System;
using System.IO;
using System.Linq;

namespace StayLogged.LogsWriter
{
    class LinuxLogReader
    {
        private string path = "/var/log";
        private string file = "syslog";
        private Log log;
        private FileSystemWatcher fileSystemWatcher;
        private SendLogs sendLogs;

        public LinuxLogReader(SendLogs sendLogs)
        {
            this.sendLogs = sendLogs;
            
            fileSystemWatcher = new FileSystemWatcher
            {
                Path = path,
                Filter = file
            };
            fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
            fileSystemWatcher.EnableRaisingEvents = true;
            Console.WriteLine("Star Log Watcher for Linux"+fileSystemWatcher.Path);
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Syslog changed"+e.FullPath);
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                string lastLine = File.ReadLines(e.FullPath).Last();
                if (lastLine.Contains("Failed") || lastLine.Contains("Error") || lastLine.Contains("failed") || lastLine.Contains("error"))
                    log = new Log("error", lastLine);
                else
                    log = new Log("information", lastLine);
                Console.WriteLine(lastLine);
                sendLogs.PublishLog(log);

            }
        }



    }
}
