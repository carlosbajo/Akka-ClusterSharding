using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Print.Line(ConsoleColor.Magenta);
            Print.Message("API NODE", ConsoleColor.Magenta);
            Print.Line(ConsoleColor.Magenta);

            var host = CreateHostBuilder(args).Build();

            var actors = Actors.Build();

            Console.CancelKeyPress += async (sender, eventArgs) =>
            {
                await actors.ShutDown();
                await host.StopAsync(TimeSpan.FromSeconds(10));
            };
            
            host.Run();
            actors.StayAlive().Wait();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}