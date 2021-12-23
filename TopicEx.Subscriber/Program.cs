using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TopicEx.Subscriber
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

            var consumerAll = new EventingBasicConsumer(channel);
            consumerAll.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TopixEx.Subscriber received '{message}' from AllSales Queue");
            };
            channel.BasicConsume("AllSales", true, consumerAll);

            var consumerBikes = new EventingBasicConsumer(channel);
            consumerBikes.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TopixEx.Subscriber received '{message}' from BikesSales Queue");
            };
            channel.BasicConsume("BikesSales", true, consumerBikes);

            var consumerClothing = new EventingBasicConsumer(channel);
            consumerClothing.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"TopixEx.Subscriber received '{message}' from ClothingSales Queue");
            };
            channel.BasicConsume("ClothingSales", true, consumerBikes);

            Console.WriteLine("Listening for messages. Press ENTER anytime to quit...");
            Console.ReadLine();
        }
    }
}

