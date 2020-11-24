using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace StayLogged.LogsWriter
{
    public class LogsProducer : IDisposable
    {
        private readonly string machineName;
        private readonly Timer timer;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly LogsReader logsReader;

        public LogsProducer(string operatingSystem, string machineName)
        {
            this.machineName = machineName;
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost")
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            logsReader = new LogsReader(operatingSystem);

            const int loggingIntervalInMilliseconds = 5000;
            timer = new Timer { Interval = loggingIntervalInMilliseconds };
            timer.Elapsed += PublishLogs;
        }

        public void Start(CancellationToken cancellationToken)
        {
            timer.Start();

            WaitHandle.WaitAny(new[] { cancellationToken.WaitHandle });
        }

        private void PublishLogs(object sender, ElapsedEventArgs e)
        {
            IBasicProperties properties = channel.CreateBasicProperties();
            List<Log> logs = logsReader.Read();

            foreach (Log log in logs)
            {
                if (string.IsNullOrEmpty(log.Data))
                {
                    continue;
                }

                PublishLog(log, properties);
            }
        }

        private void PublishLog(Log log, IBasicProperties properties)
        {
            string[] splittedDataByRows = log.Data.Split("\n");
            if (!splittedDataByRows.Any())
            {
                splittedDataByRows = log.Data.Split("\r\n");
            }

            foreach (var dataByRow in splittedDataByRows)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes($"<{machineName}> {Enum.GetName(typeof(LogType), log.Type)?.ToUpper()}: {dataByRow}");
                const string exchangeName = "logs-exchange";

                channel.BasicPublish(exchangeName,
                    Enum.GetName(typeof(LogType), log.Type)?.ToLower(),
                    properties,
                    messageBytes);
            }
        }

        public void Dispose()
        {
            connection?.Close();
            connection?.Dispose();
            channel?.Close();
            channel?.Dispose();
            timer?.Close();
            timer?.Dispose();
        }
    }
}