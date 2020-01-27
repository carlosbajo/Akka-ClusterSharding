using System;
using Shared;

namespace ClusterNode
{
    /// <summary>
    /// For this project the next nuget packages was required.
    /// * Akka
    /// * Akka.Bootstrap.Docker
    /// * Akka.Cluster
    /// * Akka.Cluster.Sharding
    /// * Akka.Persistence
    /// * Akka.Persistence.Postgresql
    ///
    /// before run this project just execute on terminal => "docker-compose up". add "-d" at the end for daemon mode.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Print.Line(ConsoleColor.Cyan);
            Print.Message("CLUSTER NODE", ConsoleColor.Cyan);
            Print.Line(ConsoleColor.Cyan);

            var actors = Actors.Build();

            Console.CancelKeyPress += async (sender, eventArgs) => { await actors.Shutdown(); };

            actors
                .StayAlive()
                .Wait();
        }
    }
}