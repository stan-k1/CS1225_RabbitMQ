using System;
using System.Text;
using RabbitMQ.Client;

namespace DirectEx.Publisher
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
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.ExchangeDeclare("CS1225_DirectEx", ExchangeType.Direct);

            channel.QueueDeclare(queue: "Green", true, false, false,null);
            channel.QueueBind("Green", "CS1225_DirectEx", "green");

            channel.QueueDeclare(queue: "Red", true, false, false, null);
            channel.QueueBind("Red", "CS1225_DirectEx", "red");

            string message = "Hello World to Green!!!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish( "CS1225_DirectEx", "green", properties, body);

            Console.WriteLine($"DirectEx.Publisher sent {message}");
        }
    }
}