using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class SubscriberService : BackgroundService
{
    private IConnection Connection { get; }
    private IModel Channel { get; }

    public SubscriberService(ILoggerFactory loggerFactory)
    {
        ConnectionFactory factory = new()
        {
            UserName = "guest",
            Password = "guest",
            HostName = "localhost"
        };

       Connection = factory.CreateConnection();
       Channel = Connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(Channel);
        consumer.Received += (ch, ea) =>
        {
            var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine($"DirectEx.WorkerSubscriber received {content}"); ;
        };

        Channel.BasicConsume("Green", true, consumer);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        Channel.Close();
        Connection.Close();
        base.Dispose();
    }
}