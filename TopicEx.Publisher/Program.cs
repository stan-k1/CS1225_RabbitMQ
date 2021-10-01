using System;
using System.Text;
using RabbitMQ.Client;

namespace TopicEx.Publisher
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
            channel.ExchangeDeclare("CS1225_TopicEx", ExchangeType.Topic);

            channel.QueueDeclare(queue: "BikesSales", false, false, false, null);
            channel.QueueBind("BikesSales", "CS1225_TopicEx", "CRM.Sales.Bikes");
            channel.QueueDeclare(queue: "ClothingSales", false, false, false, null);
            channel.QueueBind("ClothingSales", "CS1225_TopicEx", "CRM.Sales.Clothing");

            channel.QueueDeclare(queue: "AllSales", false, false, false, null);
            channel.QueueBind("AllSales", "CS1225_TopicEx", "CRM.Sales.*");
            channel.QueueDeclare(queue: "AllCRM", false, false, false, null);
            channel.QueueBind("AllCRM", "CS1225_TopicEx", "CRM.#");


            string message = "A Bike Sales Has Occurred!!!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("CS1225_TopicEx", "CRM.Sales.Bikes", null, body);
            Console.WriteLine($"TopicEx.Publisher sent messages to topic exchanges.");
        }
    }
}
