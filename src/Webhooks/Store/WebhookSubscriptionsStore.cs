using Microsoft.EntityFrameworkCore;
using Webhooks.Data;

namespace Webhooks.Store
{
    /// <summary>
    /// Implements <see cref="IWebhookSubscriptionsStore"/> using repositories.
    /// </summary>
    public class WebhookSubscriptionsStore : IWebhookSubscriptionsStore
    {
        private readonly WebhookContext _webhookContext;
        public WebhookSubscriptionsStore(WebhookContext webhookContext)
        {
            _webhookContext = webhookContext;
        }


        public virtual async Task<WebhookSubscriptionInfo> GetAsync(Guid id)
        {
            return await _webhookContext.WebhookSubscriptionInfo.FindAsync(id).ConfigureAwait(false);
        }

        public virtual WebhookSubscriptionInfo Get(Guid id)
        {
            return _webhookContext.WebhookSubscriptionInfo.SingleOrDefault(s => s.Id == id);
        }

        public virtual async Task InsertAsync(WebhookSubscriptionInfo webhookInfo)
        {
            await _webhookContext.WebhookSubscriptionInfo.AddAsync(webhookInfo).ConfigureAwait(false);

            await _webhookContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual void Insert(WebhookSubscriptionInfo webhookInfo)
        {
            _webhookContext.WebhookSubscriptionInfo.Add(webhookInfo);

            _webhookContext.SaveChanges();
        }

        public virtual async Task UpdateAsync(WebhookSubscriptionInfo webhookSubscription)
        {
            _webhookContext.WebhookSubscriptionInfo.Update(webhookSubscription);

            await _webhookContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual void Update(WebhookSubscriptionInfo webhookSubscription)
        {
            _webhookContext.WebhookSubscriptionInfo.Update(webhookSubscription);

            _webhookContext.SaveChanges();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = _webhookContext.WebhookSubscriptionInfo.IgnoreQueryFilters().SingleOrDefault(s => s.Id == id);

            if (entity != null)
                _webhookContext.WebhookSubscriptionInfo.Remove(entity);

            await _webhookContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual void Delete(Guid id)
        {
            var entity = _webhookContext.WebhookSubscriptionInfo.IgnoreQueryFilters().SingleOrDefault(s => s.Id == id);

            if (entity != null)
                _webhookContext.WebhookSubscriptionInfo.Remove(entity);

            _webhookContext.SaveChanges();
        }

        public virtual async Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid tenantId)
        {
            return await _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Where(s => s.TenantId == tenantId).ToListAsync().ConfigureAwait(false);
        }

        public virtual List<WebhookSubscriptionInfo> GetAllSubscriptions(Guid tenantId)
        {
            return _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Where(s => s.TenantId == tenantId).ToList();
        }

        public virtual async Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(
            Guid tenantId,
            string webhookName)
        {

            return await _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Where(subscriptionInfo =>
                subscriptionInfo.TenantId == tenantId &&
                subscriptionInfo.Webhooks.Contains("\"" + webhookName + "\"")
            ).ToListAsync().ConfigureAwait(false);

        }

        public virtual List<WebhookSubscriptionInfo> GetAllSubscriptions(Guid tenantId, string webhookName)
        {
            return _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Where(subscriptionInfo =>
               subscriptionInfo.TenantId == tenantId &&
               subscriptionInfo.Webhooks.Contains("\"" + webhookName + "\"")
            ).ToList();
        }

        public virtual async Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid[] tenantIds)
        {
            return await _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Where(subscriptionInfo =>
                tenantIds.Contains(subscriptionInfo.TenantId)).ToListAsync().ConfigureAwait(false);

        }

        public virtual List<WebhookSubscriptionInfo> GetAllSubscriptionsOfTenants(Guid[] tenantIds)
        {
            return _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Where(subscriptionInfo =>
               tenantIds.Contains(subscriptionInfo.TenantId)).ToList();
        }

        public virtual async Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(
            Guid[] tenantIds,
            string webhookName)
        {
            return await _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Where(subscriptionInfo =>
                tenantIds.Contains(subscriptionInfo.TenantId) &&
                subscriptionInfo.Webhooks.Contains("\"" + webhookName + "\"")
            ).ToListAsync().ConfigureAwait(false);
        }

        public virtual List<WebhookSubscriptionInfo> GetAllSubscriptionsOfTenants(Guid[] tenantIds, string webhookName)
        {
            return _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Where(subscriptionInfo =>
                  tenantIds.Contains(subscriptionInfo.TenantId) &&
                  subscriptionInfo.Webhooks.Contains("\"" + webhookName + "\"")
               ).ToList();
        }

        public virtual async Task<bool> IsSubscribedAsync(Guid tenantId, string webhookName)
        {
            return await _webhookContext.WebhookSubscriptionInfo.AsNoTracking().AnyAsync(subscriptionInfo =>
                    subscriptionInfo.TenantId == tenantId &&
                    subscriptionInfo.Webhooks.Contains("\"" + webhookName + "\"")).ConfigureAwait(false);
        }

        public virtual bool IsSubscribed(Guid tenantId, string webhookName)
        {
            return _webhookContext.WebhookSubscriptionInfo.AsNoTracking().Any(subscriptionInfo =>
                   subscriptionInfo.TenantId == tenantId &&
                   subscriptionInfo.Webhooks.Contains("\"" + webhookName + "\""));
        }

        public async Task DeleteAllByTenantAsync(Guid tenantId)
        {
            var entity = await _webhookContext.WebhookSubscriptionInfo
                                              .IgnoreQueryFilters()
                                              .Where(s => s.TenantId == tenantId)
                                              .ToListAsync()
                                              .ConfigureAwait(false);

            if (entity.Any())
                _webhookContext.WebhookSubscriptionInfo.RemoveRange(entity);

            await _webhookContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public void DeleteAllByTenant(Guid tenantId)
        {
            var entity = _webhookContext.WebhookSubscriptionInfo.IgnoreQueryFilters().Where(s => s.TenantId == tenantId).ToList();

            if (entity.Any())
                _webhookContext.WebhookSubscriptionInfo.RemoveRange(entity);

            _webhookContext.SaveChanges();
        }
    }
}
