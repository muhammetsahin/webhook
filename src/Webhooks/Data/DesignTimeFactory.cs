using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Webhooks.Data
{
  public sealed class DesignTimeFactory : IDesignTimeDbContextFactory<WebhookContext>
  {
    public WebhookContext CreateDbContext(string[] args)
    {
      var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
      var connectionString = configuration.GetConnectionString("DefaultConnection");

      IServiceCollection services = new ServiceCollection();
      services.AddDbContext<WebhookContext>(
        options => options.UseNpgsql(
          connectionString,
          sqlOptions => sqlOptions
            .MigrationsAssembly(typeof(WebhookContext).Assembly.FullName)));

      var context = services.BuildServiceProvider().GetService<WebhookContext>();
      return context;

    }
  }
}
