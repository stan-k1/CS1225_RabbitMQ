using System;
using System.Text;
using RabbitMQ.Client;

namespace HeadersEx.Publisher
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
            channel.QueueDeclare(queue: "BlackSquares", false, false, false, null);
            channel.QueueBind("BlackSquares", "CS1225_HeadersEx", "", bindHeaders);

            string message = "A Black Square has been generated!";
            var body = Encoding.UTF8.GetBytes(message);
            
            IBasicProperties props = channel.CreateBasicProperties();
            Dictionary<string, object> msgHeaders = new()
            {
                { "shape", "square" },
                { "color", "black" }
            };
            props.Headers = msgHeaders;

            channel.BasicPublish("CS1225_HeadersEx", "", props, body);
            Console.WriteLine($"HeadersEx.Publisher sent message to the headers exchange.");
        }
    }
}
