using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Practice.Core.Applications.WebApplication
{
    public class MainClass
    {
        static void Main(string[] args)
        {
            var webBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .ConfigureLogging((context, loggingBuilder) =>
                {                    
                    loggingBuilder.AddDebug();
                    loggingBuilder.AddConfiguration(configuration: context.Configuration.GetSection("Logging"));
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        loggingBuilder.AddConsole();
                    }
                })
                .UseStartup<Startup>()
                .Build();

            if(webBuilder != default(WebHostBuilder))
            {
                webBuilder.Run();
            }
        }
    }
}
