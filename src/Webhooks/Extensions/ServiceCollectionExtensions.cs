using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Webhooks.BackgroundWorker;
using Webhooks.Store;

namespace Webhooks.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWebhooks(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextFactory<Data.WebhookContext>(builder =>
                                                         builder
                                                         .UseNpgsql(config.GetConnectionString("DefaultConnection")));

            services.AddTransient<IWebhookSenderJob, WebhookSenderJob>();
            services.AddTransient<IWebhookEventStore, WebhookEventStore>();
            services.AddTransient<IWebhookSendAttemptStore, WebhookSendAttemptStore>();
            services.AddTransient<IWebhookSubscriptionsStore, WebhookSubscriptionsStore>();
            services.AddSingleton<IWebhooksConfiguration, WebhooksConfiguration>();//Singleton
            services.AddTransient<IWebhookSubscriptionManager, WebhookSubscriptionManager>();
            services.AddTransient<IWebhookDefinitionManager, WebhookDefinitionManager>();
            services.AddTransient<IWebhookManager, WebhookManager>();
            services.AddTransient<IWebhookPublisher, DefaultWebhookPublisher>();
            services.AddTransient<IWebhookSender, DefaultWebhookSender>();

        }
    }
}
