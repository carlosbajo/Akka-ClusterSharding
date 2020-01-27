using System;

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
            Console.WriteLine("Hello World!");
        }
    }
}