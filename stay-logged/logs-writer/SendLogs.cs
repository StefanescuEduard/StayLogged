using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace StayLogged.LogsWriter
{
    public class SendLogs : IDisposable
    {
        private readonly string machineName;
        private readonly IConnection connection;
        private readonly IModel channel;

        public SendLogs(string operatingSystem, string machineName)
        {
            this.machineName = machineName;
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@192.168.1.129")
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            switch(operatingSystem){
                case "WINDOWS":
                    new WindowsLogReader(this);
                    break;
                case "LINUX":
                    new LinuxLogReader(this);
                    break;
                default:break;
            }
        }

        public void PublishLog(Log log)
        {
            IBasicProperties properties = channel.CreateBasicProperties();
            byte[] messageBytes = Encoding.UTF8.GetBytes($"<{machineName}> { log.Type.ToUpper()}: {log.Data}");
            const string exchangeName = "logs-exchange";
            channel.BasicPublish(exchangeName, log.Type.ToLower(), properties, messageBytes);
            
        }

        public void Dispose()
        {
            connection?.Close();
            connection?.Dispose();
            channel?.Close();
            channel?.Dispose();
        }
    }
}