using KafkaFlow;
using KafkaFlow.Configuration;

namespace YourNuts.KafkaFlowIntegration.Options;

public class KafkaConsumerConfiguration
{
    public int? BufferSize { get; set; }
    public int? WorkerCount { get; set; }
}

public class KafkaProducerConfiguration
{
    // Add producer-specific configurations here
}

public class KafkaOptions
{
    public const string SectionName = "Kafka";

    public bool Enabled { get; set; } = true;


    public List<string> Brokers { get; set; } = [];

    private Dictionary<string, KafkaConsumerConfiguration>? Consumer { get; set; } = new();
    private Dictionary<string, KafkaProducerConfiguration>? Producer { get; set; } = new();


    public KafkaConsumerConfiguration? GetConsumer(string topic)
    {
        return Consumer?.GetValueOrDefault(topic, null);
    }

    public void Register(IKafkaConfigurationBuilder kafka,
        Action<IClusterConfigurationBuilder>? configureCluster = null,
        List<string>? topics = null)
    {
        kafka
            .AddCluster(cluster =>
            {
                cluster.WithBrokers(Brokers);

                // Invoke the additional configuration action
                configureCluster?.Invoke(cluster);

                // if in development environment
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development" ||
                    topics == null) return;

                foreach (var topic in topics)
                {
                    cluster.CreateTopicIfNotExists(topic, 1, 1);
                }
            });
    }
}