using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace StayLogged.LogsWriter
{
    public class LogsProducer : IDisposable
    {
        private readonly Timer timer;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly LogsReader logsReader;

        public LogsProducer(string operatingSystem)
        {
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

                byte[] messageBytes = Encoding.UTF8.GetBytes($"{Enum.GetName(typeof(LogType), log.Type)?.ToUpper()}: {log.Data}");
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