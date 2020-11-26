using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
                    log = new Log("error", lastLine,getSource(lastLine));
                else
                    log = new Log("information", lastLine,getSource(lastLine));
                Console.WriteLine(lastLine);
                sendLogs.PublishLog(log);

            }
        }

        private string getSource(string line)
        {
            Regex regex = new Regex(@"(?<=(\w+\S+\s){4})(\w+)");
            Console.WriteLine(regex.Match(line).ToString());
            return regex.Match(line).ToString();
        }



    }
}
