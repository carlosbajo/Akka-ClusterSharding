using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Sharding;
using Akka.Configuration;
using Microsoft.VisualBasic;
using Shared;
using Shared.Actors;
using Constants = Shared.Constants;

namespace Api
{
    /// <summary>
    /// The purpose of this class is to create the actor system on the program and
    /// manage the actors by the actor proxy for communicate across the cluster. :D
    /// </summary>
    public class Actors
    {
        public static Actors Instance { get; private set; }
        private readonly ActorSystem _system;
        
        public IActorRef CustomerProxy { get; }

        private Actors(Config config)
        {
            _system = ActorSystem.Create(Constants.SystemName, config);

            var sharding = ClusterSharding.Get(_system);
            
            CustomerProxy = sharding.StartProxy(
                typeName: Customer.TypeName,
                role: Constants.ClusterNodeRoleName,
                messageExtractor: new MessageExtractor());
        }

        public static Actors Build()
        {
            var directory =
                AppContext.BaseDirectory.Substring(0,
                    AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
            
            var hocon = File.ReadAllText($"{directory}/api.hocon");

            var config = ConfigurationFactory
                .ParseString(hocon)
                .WithFallback(ClusterSharding.DefaultConfig());
            
            Instance = new Actors(config);
            return Instance;
        }

        public Task StayAlive()
        {
            return _system.WhenTerminated;
        }

        public Task ShutDown()
        {
            return CoordinatedShutdown
                .Get(_system)
                .Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}