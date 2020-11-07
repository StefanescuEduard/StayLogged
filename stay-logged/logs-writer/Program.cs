using System.Linq;

namespace StayLogged.LogsWriter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Any())
            {
                string operatingSystemName = GetOperatingSystemName(args.First());

                using var logsProducer = new LogsProducer(operatingSystemName);
                logsProducer.Start();
            }
        }

        private static string GetOperatingSystemName(string argument)
        {
            if (argument.Equals("linux"))
            {
                return "linux";
            }
            if (argument.Equals("windows"))
            {
                return "windows";
            }

            return string.Empty;
        }
    }
}
