using Hangfire;
using Webhooks.BackgroundWorker;
using Webhooks.Extensions;

namespace Webhooks
{
    public class DefaultWebhookPublisher : IWebhookPublisher
    {
        public readonly IWebhookEventStore _webhookEventStore;
        private readonly IWebhookSubscriptionManager _webhookSubscriptionManager;
        private readonly IWebhooksConfiguration _webhooksConfiguration;

        public DefaultWebhookPublisher(IWebhookSubscriptionManager webhookSubscriptionManager,
                                       IWebhooksConfiguration webhooksConfiguration,
                                       IWebhookEventStore webhookEventStore)
        {
            _webhookSubscriptionManager = webhookSubscriptionManager;
            _webhooksConfiguration = webhooksConfiguration;
            _webhookEventStore = webhookEventStore;
        }

        public virtual async Task PublishAsync(string webhookName, object data, Guid tenantId, bool sendExactSameData = false)
        {
            var subscriptions = await _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGrantedAsync(tenantId, webhookName);
            await PublishAsync(webhookName, data, subscriptions, sendExactSameData);
        }

        public virtual async Task PublishAsync(Guid[] tenantIds, string webhookName, object data, bool sendExactSameData = false)
        {
            var subscriptions = _webhookSubscriptionManager.GetAllSubscriptionsOfTenantsIfFeaturesGranted(tenantIds, webhookName);
            await PublishAsync(webhookName, data, subscriptions, sendExactSameData);
        }

        private async Task PublishAsync(string webhookName, object data, List<WebhookSubscription> webhookSubscriptions, bool sendExactSameData = false)
        {
            if (webhookSubscriptions is null || webhookSubscriptions.Count == 0)
            {
                return;
            }

            var subscriptionsGroupedByTenant = webhookSubscriptions.GroupBy(x => x.TenantId);

            foreach (var subscriptionGroupedByTenant in subscriptionsGroupedByTenant)
            {
                var webhookInfo = await SaveAndGetWebhookAsync(subscriptionGroupedByTenant.Key, webhookName, data);

                foreach (var webhookSubscription in subscriptionGroupedByTenant)
                {
                    BackgroundJob.Enqueue<IWebhookSenderJob>((s) => s.ExecuteAsync(new WebhookSenderArgs
                    {
                        TenantId = webhookSubscription.TenantId,
                        WebhookEventId = webhookInfo.Id,
                        Data = webhookInfo.Data,
                        WebhookName = webhookInfo.WebhookName,
                        WebhookSubscriptionId = webhookSubscription.Id,
                        Headers = webhookSubscription.Headers,
                        Secret = webhookSubscription.Secret,
                        WebhookUri = webhookSubscription.WebhookUri,
                        SendExactSameData = sendExactSameData
                    }));
                }
            }
        }

        //public virtual void Publish(string webhookName, object data, bool sendExactSameData = false)
        //{
        //    var subscriptions = _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGranted(null, webhookName);
        //    Publish(webhookName, data, subscriptions, sendExactSameData);
        //}

        public virtual void Publish(string webhookName, object data, Guid tenantId, bool sendExactSameData = false)
        {
            var subscriptions = _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGranted(tenantId, webhookName);
            Publish(webhookName, data, subscriptions, sendExactSameData);
        }

        public virtual void Publish(Guid[] tenantIds, string webhookName, object data, bool sendExactSameData = false)
        {
            var subscriptions = _webhookSubscriptionManager.GetAllSubscriptionsOfTenantsIfFeaturesGranted(tenantIds, webhookName);
            Publish(webhookName, data, subscriptions, sendExactSameData);
        }

        private void Publish(string webhookName, object data, List<WebhookSubscription> webhookSubscriptions, bool sendExactSameData = false)
        {
            if (webhookSubscriptions is null || webhookSubscriptions.Count == 0)
            {
                return;
            }

            var subscriptionsGroupedByTenant = webhookSubscriptions.GroupBy(x => x.TenantId);

            foreach (var subscriptionGroupedByTenant in subscriptionsGroupedByTenant)
            {
                var webhookInfo = SaveAndGetWebhook(subscriptionGroupedByTenant.Key, webhookName, data);

                foreach (var webhookSubscription in subscriptionGroupedByTenant)
                {

                    BackgroundJob.Enqueue<IWebhookSenderJob>((s) => s.ExecuteAsync(new WebhookSenderArgs
                    {
                        TenantId = webhookSubscription.TenantId,
                        WebhookEventId = webhookInfo.Id,
                        Data = webhookInfo.Data,
                        WebhookName = webhookInfo.WebhookName,
                        WebhookSubscriptionId = webhookSubscription.Id,
                        Headers = webhookSubscription.Headers,
                        Secret = webhookSubscription.Secret,
                        WebhookUri = webhookSubscription.WebhookUri,
                        SendExactSameData = sendExactSameData
                    }));
                }
            }
        }

        protected virtual async Task<WebhookEvent> SaveAndGetWebhookAsync(Guid tenantId, string webhookName, object data)
        {
            var webhookInfo = new WebhookEvent
            {
                Id = Guid.NewGuid(),
                WebhookName = webhookName,
                Data = _webhooksConfiguration.JsonSerializerSettings != null
                    ? data.ToJson(settings: _webhooksConfiguration.JsonSerializerSettings)
                    : data.ToJson(),
                TenantId = tenantId,
                CreationTime = DateTime.Now,
            };

            await _webhookEventStore.InsertAndGetIdAsync(webhookInfo);
            return webhookInfo;
        }

        protected virtual WebhookEvent SaveAndGetWebhook(Guid tenantId, string webhookName, object data)
        {
            var webhookInfo = new WebhookEvent
            {
                Id = Guid.NewGuid(),
                WebhookName = webhookName,
                Data = _webhooksConfiguration.JsonSerializerSettings != null
                    ? data.ToJson(settings: _webhooksConfiguration.JsonSerializerSettings)
                    : data.ToJson(),
                TenantId = tenantId
            };

            _webhookEventStore.InsertAndGetId(webhookInfo);
            return webhookInfo;
        }
    }
}