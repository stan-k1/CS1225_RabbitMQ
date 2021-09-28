using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DirectEx.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.ExchangeDeclare("CS1225_DirectEx",  ExchangeType.Direct);
            channel.QueueDeclare( "Green", true, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            channel.BasicConsume(queue: "Green", true, consumer);

            Console.WriteLine("Listening for messages. Press ENTER anytime to quit...");
            Console.ReadLine();
        }
    }
}
