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
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    var cancellationTokenSource = new CancellationTokenSource();

                    using var logsProducer = new LogsProducer(o.OperatingSystem);
                    logsProducer.Start(cancellationTokenSource.Token);

                    Console.ReadKey();

                    cancellationTokenSource.Cancel();
                });
        }
    }
}
