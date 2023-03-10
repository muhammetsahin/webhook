using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Webhooks.Data
{
    public class WebhookContext : DbContext
    {
        public DbSet<WebhookSubscriptionInfo> WebhookSubscriptionInfo { get; set; }
        public DbSet<WebhookEvent> WebhookEvent { get; set; }
        public DbSet<WebhookSendAttempt> WebhookSendAttempt { get; set; }

        public WebhookContext(DbContextOptions<WebhookContext> options)
          : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebhookSubscriptionInfo>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<WebhookSubscriptionInfo>().HasIndex(x => x.TenantId);
            modelBuilder.Entity<WebhookSubscriptionInfo>().HasQueryFilter(p => p.IsActive);
            modelBuilder.Entity<WebhookEvent>().HasIndex(x => x.TenantId);
            modelBuilder.Entity<WebhookEvent>().Property(p => p.Id).ValueGeneratedNever();
            modelBuilder.Entity<WebhookSendAttempt>().HasIndex(x => x.TenantId);
            modelBuilder.Entity<WebhookSendAttempt>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<WebhookSendAttempt>().Property(p => p.Response).HasDefaultValue("{}");


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // Uncomment the next line to see them in the output window of visual studio
            optionsBuilder.LogTo(m => System.Diagnostics.Debug.WriteLine(m), Microsoft.Extensions.Logging.LogLevel.Warning);

            // Or uncomment the next line if you want to see them in the console
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);
        }
    }
}
