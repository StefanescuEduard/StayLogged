using CommandLine;
using System;
using System.Threading;

namespace StayLogged.LogsWriter
{
    public class Program
    {
        public class Options
        {
            [Option('o', "operatingSystem", Required = true, HelpText = "Provide the operating system name.")]
            public string OperatingSystem { get; set; }

            [Option('n', "machineName", Required = true, HelpText = "Provide the machine name.")]
            public string MachineName { get; set; }
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    var cancellationTokenSource = new CancellationTokenSource();

                    using var logsProducer = new LogsProducer(o.OperatingSystem, o.MachineName);
                    logsProducer.Start(cancellationTokenSource.Token);

                    Console.ReadKey();

                    cancellationTokenSource.Cancel();
                });
        }
    }
}
