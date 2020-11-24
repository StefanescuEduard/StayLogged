using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace StayLogged.LogsReader
{
    public class Program
    {
        private static IConnection connection;
        private static IModel channel;

        public static void Main()
        {
            const string exchangeName = "logs-exchange";
            string[] queueNames = { "info", "error" };

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost")
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            CreateExchange(exchangeName);
            CreateQueues(queueNames, exchangeName);

            foreach (string queueName in queueNames)
            {
                var thread = new Thread(() => WriteLog(queueName, cancellationToken));
                thread.Start();
            }

            Console.ReadKey();

            cancellationTokenSource.Cancel();

            WaitHandle.WaitAny(new[] { cancellationToken.WaitHandle });

            channel.Close();
            channel.Dispose();
            connection.Close();
            connection.Dispose();
        }

        private static void CreateExchange(string exchangeName)
        {
            channel.ExchangeDelete(exchangeName);
            channel.ExchangeDeclare(exchangeName, "direct");
        }

        private static void CreateQueues(string[] queueNames, string exchangeName)
        {
            foreach (var queueName in queueNames)
            {
                channel.QueueDelete(queueName);
                channel.QueueDeclare(queueName,
                    false,
                    false,
                    false,
                    null);

                channel.QueueBind(queueName,
                    exchangeName,
                    queueName);
            }
        }

        private static void WriteLog(string queueName, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                BasicGetResult result = channel.BasicGet(queueName, false);
                if (result != null)
                {
                    string rawLogs = Encoding.UTF8.GetString(result.Body.ToArray());
                    string[] logs = rawLogs.Split(Environment.NewLine);
                    var logsWriter = new LogsWriter(logs);
                    logsWriter.Write();

                    channel.BasicAck(result.DeliveryTag, false);
                }
            }
        }
    }
}
