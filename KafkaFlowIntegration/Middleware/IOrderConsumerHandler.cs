using KafkaFlow;

namespace KafkaFlowIntegration
{
    public interface IOrderConsumerHandler
    {
        Task Handle(IMessageContext context, string message);
    }
}