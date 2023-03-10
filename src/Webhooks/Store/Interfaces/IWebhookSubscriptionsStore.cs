
namespace Webhooks
{
    /// <summary>
    /// This interface should be implemented by vendors to make webhooks working.
    /// </summary>
    public interface IWebhookSubscriptionsStore
    {
        /// <summary>
        /// returns subscription
        /// </summary>
        /// <param name="id">webhook subscription id</param>
        /// <returns></returns>
        Task<WebhookSubscriptionInfo> GetAsync(Guid id);

        /// <summary>
        /// returns subscription
        /// </summary>
        /// <param name="id">webhook subscription id</param>
        /// <returns></returns>
        WebhookSubscriptionInfo Get(Guid id);

        /// <summary>
        /// Saves webhook subscription to a persistent store.
        /// </summary>
        /// <param name="webhookSubscription">webhook subscription information</param>
        Task InsertAsync(WebhookSubscriptionInfo webhookSubscription);

        /// <summary>
        /// Saves webhook subscription to a persistent store.
        /// </summary>
        /// <param name="webhookSubscription">webhook subscription information</param>
        void Insert(WebhookSubscriptionInfo webhookSubscription);

        /// <summary>
        /// Updates webhook subscription to a persistent store.
        /// </summary>
        /// <param name="webhookSubscription">webhook subscription information</param>
        Task UpdateAsync(WebhookSubscriptionInfo webhookSubscription);

        /// <summary>
        /// Updates webhook subscription to a persistent store.
        /// </summary>
        /// <param name="webhookSubscription">webhook subscription information</param>
        void Update(WebhookSubscriptionInfo webhookSubscription);

        /// <summary>
        /// Deletes subscription if exists
        /// </summary>
        /// <param name="id"><see cref="WebhookSubscriptionInfo"/> primary key</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Deletes subscription if exists 
        /// </summary>
        /// <param name="id"><see cref="WebhookSubscriptionInfo"/> primary key</param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Returns all subscriptions of given tenant including deactivated 
        /// </summary>
        /// <param name="tenantId">
        /// Target tenant id.
        /// </param>
        Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid tenantId);

        /// <summary>
        /// Returns all subscriptions of given tenant including deactivated 
        /// </summary>
        /// <param name="tenantId">
        /// Target tenant id.
        /// </param>
        List<WebhookSubscriptionInfo> GetAllSubscriptions(Guid tenantId);

        /// <summary>
        /// Returns webhook subscriptions which subscribe to given webhook on tenant(s)
        /// </summary>
        /// <param name="tenantId">
        /// Target tenant id.
        /// </param>
        /// <param name="webhookName"><see cref="WebhookDefinition.Name"/></param>
        /// <returns></returns>
        Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid tenantId, string webhookName);

        /// <summary>
        /// Returns webhook subscriptions which subscribe to given webhook on tenant(s)
        /// </summary>
        /// <param name="tenantId">
        /// Target tenant id.
        /// </param>
        /// <param name="webhookName"><see cref="WebhookDefinition.Name"/></param>
        /// <returns></returns>
        List<WebhookSubscriptionInfo> GetAllSubscriptions(Guid tenantId, string webhookName);
        
        /// <summary>
        /// Returns all subscriptions of given tenant including deactivated 
        /// </summary>
        /// <param name="tenantIds">
        /// Target tenant id(s).
        /// </param>
        Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid[] tenantIds);

        /// <summary>
        /// Returns all subscriptions of given tenant including deactivated 
        /// </summary>
        /// <param name="tenantIds">
        /// Target tenant id(s).
        /// </param>
        List<WebhookSubscriptionInfo> GetAllSubscriptionsOfTenants(Guid[] tenantIds);

        /// <summary>
        /// Returns webhook subscriptions which subscribe to given webhook on tenant(s)
        /// </summary>
        /// <param name="tenantIds">
        /// Target tenant id(s).
        /// </param>
        /// <param name="webhookName"><see cref="WebhookDefinition.Name"/></param>
        /// <returns></returns>
        Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid[] tenantIds, string webhookName);

        /// <summary>
        /// Returns webhook subscriptions which subscribe to given webhook on tenant(s)
        /// </summary>
        /// <param name="tenantIds">
        /// Target tenant id(s).
        /// </param>
        /// <param name="webhookName"><see cref="WebhookDefinition.Name"/></param>
        /// <returns></returns>
        List<WebhookSubscriptionInfo> GetAllSubscriptionsOfTenants(Guid[] tenantIds, string webhookName);
        

        /// <summary>
        /// Checks if tenant subscribed for a webhook
        /// </summary>
        /// <param name="tenantId">
        /// Target tenant id(s).
        /// </param>
        /// <param name="webhookName">Name of the webhook</param>
        Task<bool> IsSubscribedAsync(Guid tenantId, string webhookName);

        /// <summary>
        /// Checks if tenant subscribed for a webhook
        /// </summary>
        /// <param name="tenantId">
        /// Target tenant id(s).
        /// </param>
        /// <param name="webhookName">Name of the webhook</param>
        bool IsSubscribed(Guid tenantId, string webhookName);

        /// <summary>
        /// Deletes subscription if exists
        /// </summary>
        /// <param name="tenantId"><see cref="WebhookSubscriptionInfo"/> primary key</param>
        /// <returns></returns>
        Task DeleteAllByTenantAsync(Guid tenantId);

        /// <summary>
        /// Deletes subscription if exists 
        /// </summary>
        /// <param name="tenantId"><see cref="WebhookSubscriptionInfo"/> primary key</param>
        /// <returns></returns>
        void DeleteAllByTenant(Guid tenantId);
    }
}
