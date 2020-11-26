using RabbitMQ.Client;
using StayLogged.DataAccess;
using StayLogged.Domain;
using System;
using System.Text;
using System.Text.RegularExpressions;
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
            string[] queueNames = { "information", "error" };

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
            LogRepository logRepository = new LogRepository();
            Log log = new Log();
            Host host = new Host();
            Regex regexbefore = new Regex(@"([^:]+)");
            Regex regexafter = new Regex(@"(?<=@Data@).*");
            while (!cancellationToken.IsCancellationRequested)
            {
                BasicGetResult result = channel.BasicGet(queueName, false);
                if (result != null)
                {
                    string rawLogs = Encoding.UTF8.GetString(result.Body.ToArray());
                    string rawinfo = regexbefore.Match(rawLogs).ToString();
                    string data = regexafter.Match(rawLogs).ToString();
                    string[] info = rawinfo.Split();
                    host.Name = info[0];
                    Console.WriteLine(host.Name);
                    host.Ip = info[3];
                    Console.WriteLine(host.Ip);
                    log.Host = host;
                    log.Type = info[1];
                    Console.WriteLine(log.Type);
                    log.Source = info[2];
                    Console.WriteLine(log.Source);
                    log.Descriptions = data;
                    Console.WriteLine(log.Descriptions);
                    log.DateTime = DateTime.Now;
                    Console.WriteLine(rawLogs);
                    logRepository.Add(log);

                    channel.BasicAck(result.DeliveryTag, false);
                }
            }
        }
    }
}
