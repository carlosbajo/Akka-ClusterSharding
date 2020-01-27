using Akka.Cluster.Sharding;

namespace Shared
{
    /// <summary>
    ///  Message extractor for communication between nodes
    ///  It's  necessary :D
    /// </summary>
    public class MessageExtractor : HashCodeMessageExtractor
    {
        public MessageExtractor() : base(Constants.MaximumNumberOfShards)
        {
        }

        public override string EntityId(object message) => (message as ShardEnvelope)?.EntityId;
        public override object EntityMessage(object message) => (message as ShardEnvelope)?.Message;
    }
}