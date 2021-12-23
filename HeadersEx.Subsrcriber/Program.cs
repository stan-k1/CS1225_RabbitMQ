using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HeadersEx.Subscriber
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
            channel.ExchangeDeclare("CS1225_HeadersEx", ExchangeType.Headers);

            Dictionary<string, object> bindHeaders = new()
            {
                { "x-match", "all" },
                { "shape", "square" },
                { "color", "black" }
            };
            channel.QueueDeclare(queue: "BlackSquares", false, false, false, bindHeaders);
            channel.QueueBind("BlackSquares", "CS1225_HeadersEx", "", bindHeaders);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"HeadersEx.Subscriber received '{message}' from BlackSquares Queue");
            };
            channel.BasicConsume("BlackSquares", true, consumer);

            Console.WriteLine("Listening for messages. Press ENTER anytime to quit...");
            Console.ReadLine();
        }
    }
}

