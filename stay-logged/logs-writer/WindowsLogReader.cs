using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StayLogged.LogsWriter
{
    class WindowsLogReader
    {
        private EventLog log;
        private SendLogs sendLogs;
        public WindowsLogReader(SendLogs sendLogs)
        {
            this.sendLogs = sendLogs;   
            log = new EventLog();
            log.Log = "Application";
            log.EntryWritten += new EntryWrittenEventHandler(ReadLog);
            log.EnableRaisingEvents = true;
        }

        private void ReadLog(object sender, EntryWrittenEventArgs e)
        {
            Console.WriteLine("Event Raised: |Message:{0}|Source:{1}|EventID:{2}|Type:{3}", e.Entry.Message, e.Entry.Source, e.Entry.InstanceId, e.Entry.EntryType);
            Log log = new Log(e.Entry.EntryType, e.Entry.Message);
            sendLogs.PublishLog(log);
        }
    }
}
