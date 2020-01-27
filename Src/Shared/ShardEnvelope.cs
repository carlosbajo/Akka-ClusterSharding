namespace Shared
{
    public class ShardEnvelope
    {
        public string EntityId { get; }
        public object Message { get; }

        public ShardEnvelope(string entityId, object message)
        {
            EntityId = entityId;
            Message = message;
        }
    }
}