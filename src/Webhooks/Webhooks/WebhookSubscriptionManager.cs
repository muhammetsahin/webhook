using Webhooks.Extensions;

namespace Webhooks
{
    public class WebhookSubscriptionManager : IWebhookSubscriptionManager
    {
        public readonly IWebhookSubscriptionsStore _webhookSubscriptionsStore;


        private readonly IWebhookDefinitionManager _webhookDefinitionManager;

        private const string WebhookSubscriptionSecretPrefix = "whs_";

        public WebhookSubscriptionManager(IWebhookDefinitionManager webhookDefinitionManager, 
                                          IWebhookSubscriptionsStore webhookSubscriptionsStore)
        {
            _webhookDefinitionManager = webhookDefinitionManager;
            _webhookSubscriptionsStore = webhookSubscriptionsStore;
        }

        public virtual async Task<WebhookSubscription> GetAsync(Guid id)
        {
            return (await _webhookSubscriptionsStore.GetAsync(id)).ToWebhookSubscription();
        }

        public virtual WebhookSubscription Get(Guid id)
        {
            return _webhookSubscriptionsStore.Get(id).ToWebhookSubscription();
        }

        public virtual async Task<List<WebhookSubscription>> GetAllSubscriptionsAsync(Guid tenantId)
        {
            return (await _webhookSubscriptionsStore.GetAllSubscriptionsAsync(tenantId))
                .Select(subscriptionInfo => subscriptionInfo.ToWebhookSubscription()).ToList();
        }

        public virtual List<WebhookSubscription> GetAllSubscriptions(Guid tenantId)
        {
            return _webhookSubscriptionsStore.GetAllSubscriptions(tenantId)
                .Select(subscriptionInfo => subscriptionInfo.ToWebhookSubscription()).ToList();
        }

        public virtual async Task<List<WebhookSubscription>> GetAllSubscriptionsIfFeaturesGrantedAsync(Guid tenantId, string webhookName)
        {
            if (!_webhookDefinitionManager.IsAvailable(tenantId, webhookName))
            {
                return new List<WebhookSubscription>();
            }

            return (await _webhookSubscriptionsStore.GetAllSubscriptionsAsync(tenantId, webhookName))
                .Select(subscriptionInfo => subscriptionInfo.ToWebhookSubscription()).ToList();
        }

        public virtual List<WebhookSubscription> GetAllSubscriptionsIfFeaturesGranted(Guid tenantId, string webhookName)
        {
            if (!_webhookDefinitionManager.IsAvailable(tenantId, webhookName))
            {
                return new List<WebhookSubscription>();
            }

            return _webhookSubscriptionsStore.GetAllSubscriptions(tenantId, webhookName)
                .Select(subscriptionInfo => subscriptionInfo.ToWebhookSubscription()).ToList();
        }

        public virtual async Task<List<WebhookSubscription>> GetAllSubscriptionsOfTenantsAsync(Guid[] tenantIds)
        {
            return (await _webhookSubscriptionsStore.GetAllSubscriptionsOfTenantsAsync(tenantIds))
                .Select(subscriptionInfo => subscriptionInfo.ToWebhookSubscription()).ToList();
        }

        public virtual List<WebhookSubscription> GetAllSubscriptionsOfTenants(Guid[] tenantIds)
        {
            return _webhookSubscriptionsStore.GetAllSubscriptionsOfTenants(tenantIds)
                .Select(subscriptionInfo => subscriptionInfo.ToWebhookSubscription()).ToList();
        }

        public virtual List<WebhookSubscription> GetAllSubscriptionsOfTenantsIfFeaturesGrantedAsync(Guid[] tenantIds, string webhookName)
        {
            var featureGrantedTenants = new List<Guid>();
            foreach (var tenantId in tenantIds)
            {
                if (_webhookDefinitionManager.IsAvailable(tenantId, webhookName))
                {
                    featureGrantedTenants.Add(tenantId);
                }
            }

            return (_webhookSubscriptionsStore.GetAllSubscriptionsOfTenants(featureGrantedTenants.ToArray(), webhookName))
                .Select(subscriptionInfo => subscriptionInfo.ToWebhookSubscription()).ToList();
        }

        public virtual List<WebhookSubscription> GetAllSubscriptionsOfTenantsIfFeaturesGranted(Guid[] tenantIds, string webhookName)
        {
            var featureGrantedTenants = new List<Guid>();
            foreach (var tenantId in tenantIds)
            {
                if (_webhookDefinitionManager.IsAvailable(tenantId, webhookName))
                {
                    featureGrantedTenants.Add(tenantId);
                }
            }

            return _webhookSubscriptionsStore.GetAllSubscriptionsOfTenants(featureGrantedTenants.ToArray(), webhookName)
                .Select(subscriptionInfo => subscriptionInfo.ToWebhookSubscription()).ToList();
        }

        public virtual async Task<bool> IsSubscribedAsync(Guid tenantId, string webhookName)
        {
            if (!_webhookDefinitionManager.IsAvailable(tenantId, webhookName))
            {
                return false;
            }

            return await _webhookSubscriptionsStore.IsSubscribedAsync(tenantId, webhookName);
        }

        public virtual bool IsSubscribed(Guid tenantId, string webhookName)
        {
            if (!_webhookDefinitionManager.IsAvailable(tenantId, webhookName))
            {
                return false;
            }

            return _webhookSubscriptionsStore.IsSubscribed(tenantId, webhookName);
        }

        public virtual async Task AddOrUpdateSubscriptionAsync(WebhookSubscription webhookSubscription)
        {
            CheckIfPermissionsGranted(webhookSubscription);

            if (webhookSubscription.Id == default)
            {
                webhookSubscription.Id = Guid.NewGuid();
                webhookSubscription.Secret = WebhookSubscriptionSecretPrefix + Guid.NewGuid().ToString("N");
                await _webhookSubscriptionsStore.InsertAsync(webhookSubscription.ToWebhookSubscriptionInfo());
            }
            else
            {
                var subscription = await _webhookSubscriptionsStore.GetAsync(webhookSubscription.Id);
                subscription.WebhookUri = webhookSubscription.WebhookUri;
                subscription.Webhooks = webhookSubscription.Webhooks.ToJson();
                subscription.Headers = webhookSubscription.Headers.ToJson();
                await _webhookSubscriptionsStore.UpdateAsync(subscription);
            }
        }

        public virtual void AddOrUpdateSubscription(WebhookSubscription webhookSubscription)
        {
            if (webhookSubscription.Id == default)
            {
                webhookSubscription.Id = Guid.NewGuid();
                webhookSubscription.Secret = WebhookSubscriptionSecretPrefix + Guid.NewGuid().ToString("N");
                _webhookSubscriptionsStore.Insert(webhookSubscription.ToWebhookSubscriptionInfo());
            }
            else
            {
                var subscription = _webhookSubscriptionsStore.Get(webhookSubscription.Id);
                subscription.WebhookUri = webhookSubscription.WebhookUri;
                subscription.Webhooks = webhookSubscription.Webhooks.ToJson();
                subscription.Headers = webhookSubscription.Headers.ToJson();
                _webhookSubscriptionsStore.Update(subscription);
            }
        }

        public virtual async Task ActivateWebhookSubscriptionAsync(Guid id, bool active)
        {
            var webhookSubscription = await _webhookSubscriptionsStore.GetAsync(id);
            webhookSubscription.IsActive = active;
            await _webhookSubscriptionsStore.UpdateAsync(webhookSubscription);
        }

        public virtual Task DeleteSubscriptionAsync(Guid id)
        {
            return _webhookSubscriptionsStore.DeleteAsync(id);
        }

        public virtual void DeleteSubscription(Guid id)
        {
            _webhookSubscriptionsStore.Delete(id);
        }

        public virtual void AddWebhookAsync(WebhookSubscriptionInfo subscription, string webhookName)
        {
            subscription.SubscribeWebhook(webhookName);
        }

        public virtual void AddWebhook(WebhookSubscriptionInfo subscription, string webhookName)
        {
            subscription.SubscribeWebhook(webhookName);
        }

        public async Task DeleteSubscriptionByTenantAsync(Guid tenantId)
        {
            await _webhookSubscriptionsStore.DeleteAllByTenantAsync(tenantId);
        }

        public void DeleteSubscriptionByTenant(Guid tenantId)
        {
            _webhookSubscriptionsStore.DeleteAllByTenant(tenantId);
        }

        #region PermissionCheck

        protected virtual void CheckIfPermissionsGranted(WebhookSubscription webhookSubscription)
        {
            if (webhookSubscription.Webhooks is null || webhookSubscription.Webhooks.Count == 0)
            {
                return;
            }
        }

        #endregion
    }
}
