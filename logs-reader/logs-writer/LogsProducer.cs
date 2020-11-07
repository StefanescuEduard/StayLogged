using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

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
            const int loggingIntervalInMilliseconds = 5000;
            timer = new Timer { Interval = loggingIntervalInMilliseconds };
            timer.Elapsed += ReceiveLogs;

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost"),
                ContinuationTimeout = TimeSpan.MaxValue
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            logsReader = new LogsReader(operatingSystem);
        }

        public void Start()
        {
            timer.Start();
        }

        private void ReceiveLogs(object sender, ElapsedEventArgs e)
        {
            IBasicProperties properties = channel.CreateBasicProperties();
            List<Log> logs = logsReader.Read();

            foreach (Log log in logs)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(log.Data);
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