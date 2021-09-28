using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FanoutEx.Subscriber
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

            channel.ExchangeDeclare("CS1225_FanoutEx", ExchangeType.Fanout);
            channel.QueueDeclare("FanoutEx_Q1", false, false, false, null);
            channel.QueueBind("FanoutEx_Q1", "CS1225_FanoutEx", "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"FanoutEx.Subscriber received {message}");
            };

            channel.BasicConsume("FanoutEx_Q1",  false, consumer);

            Console.WriteLine("Listening for messages. Press ENTER anytime to quit...");
            Console.ReadLine();
        }
    }
}

