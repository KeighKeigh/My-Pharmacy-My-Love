using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ELIXIR.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
           
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(6000); // HTTP
                    serverOptions.ListenAnyIP(6001, listenOptions =>
                    
                        listenOptions.UseHttps()); // HTTPS
                    
                });
                    webBuilder.UseStartup<Startup>();

                });



    }
}

//ORIGINAL