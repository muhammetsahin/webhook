using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Webhooks.Data;

namespace Webhooks.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host =  CreateHostBuilder(args).Build();
           
           using (var serviceScope = host.Services.CreateScope())
           {
               var context = serviceScope.ServiceProvider.GetRequiredService<WebhookContext>();
              // context.Database.EnsureCreated();
               context.Database.Migrate();
           }
           
           host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
