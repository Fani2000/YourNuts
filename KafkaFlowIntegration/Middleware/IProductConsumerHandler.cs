using KafkaFlow;

namespace KafkaFlowIntegration
{
    public interface IProductConsumerHandler
    {
        Task Handle(IMessageContext context, string message);
    }
}