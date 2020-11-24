using CommandLine;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace StayLogged.LogsWriter
{
    public class Program
    {
    
        public static void Main(string[] args)
        {
            string os = "";
            var hostname = Dns.GetHostName();
            var hostinfo = Dns.GetHostEntry(hostname);
            var fqdn = hostinfo.HostName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) os = OSPlatform.Linux.ToString();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) os = OSPlatform.Windows.ToString();
            Console.WriteLine(os);
            Console.WriteLine(fqdn);

            var cancellationTokenSource = new CancellationTokenSource();

            using var logsProducer = new LogsProducer(os, fqdn);
           // logsProducer.Start(cancellationTokenSource.Token);

            Console.ReadKey();

            cancellationTokenSource.Cancel();
               
        }
    }
}
