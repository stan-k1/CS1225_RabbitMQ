using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DirectEx.WorkerConsumer
{
    public class SubscriberService : BackgroundService
    {
        private IConnection Connection { get; }
        private IModel Channel { get; }

        public SubscriberService()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" SubscriberService Received {0}", message);
                Channel.BasicAck(ea.DeliveryTag, false);
            };
            Channel.BasicConsume("Green", false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            Channel.Close();
            Connection.Close();
            base.Dispose();
        }
    }
}
