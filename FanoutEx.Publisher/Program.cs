using System;
using System.Text;
using RabbitMQ.Client;

namespace FanoutEx.Publisher
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
            channel.QueueDeclare("FanoutEx_Q2", false, false, false, null);

            channel.QueueBind("FanoutEx_Q1", "CS1225_FanoutEx", "");
            channel.QueueBind("FanoutEx_Q2", "CS1225_FanoutEx", "q2"); //The routing key is ignored in Fanout Exchanges


            string message = "Hello World";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("CS1225_FanoutEx", "", null, body);
            channel.BasicPublish("CS1225_FanoutEx", "q2", null, body); //Both messages are received by both queues

            Console.WriteLine("FanoutEx.Publisher Sent {0}", message);
        }
    }
}
        

