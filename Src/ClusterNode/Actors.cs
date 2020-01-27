using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Sharding;
using Akka.Configuration;
using Shared;
using Shared.Actors;
namespace ClusterNode
{
    /// <summary>
    /// The purpose of this class is to create the actor system on the program and
    /// manage the actors by the actor proxy for communicate across the cluster. :D
    /// </summary>
    public class Actors
    {
        public static Actors Instance { get; private set; }
        
        public static Actors Build()
        {
            var directory =
                AppContext.BaseDirectory.Substring(0,
                    AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
            
            var hocon = File.ReadAllText($"{directory}cluster.hocon");
            
            var config = ConfigurationFactory
                .ParseString(hocon)
                .WithFallback(ClusterSharding.DefaultConfig());

            Instance = new Actors(config);

            return Instance;
        }

        private readonly ActorSystem _system;

        private Actors(Config config)
        {
            _system = ActorSystem.Create(Constants.SystemName, config);

            var sharding = ClusterSharding.Get(_system);
            var settings = ClusterShardingSettings
                .Create(_system)
                .WithRole(Constants.ClusterNodeRoleName);

            ShardRegion = sharding.Start(
                typeName: Customer.TypeName,
                entityProps: Props.Create<Customer>(),
                settings: settings,
                messageExtractor: new MessageExtractor());
        }

        private IActorRef ShardRegion { get; }

        public Task StayAlive()
        {
            return _system.WhenTerminated;
        }

        public Task Shutdown()
        {
            return CoordinatedShutdown
                .Get(_system)
                .Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}