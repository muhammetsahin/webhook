using Microsoft.EntityFrameworkCore;
using Webhooks.Data;

namespace Webhooks
{
    /// <summary>
    /// Implements <see cref="IWebhookEventStore"/> using repositories.
    /// </summary>
    public class WebhookEventStore : IWebhookEventStore
    {
        private readonly WebhookContext _webhookContext;
        public WebhookEventStore(WebhookContext webhookContext)
        {
            _webhookContext = webhookContext;
        }

        public virtual async Task<Guid> InsertAndGetIdAsync(WebhookEvent webhookEvent)
        {
            await _webhookContext.WebhookEvent.AddAsync(webhookEvent).ConfigureAwait(false);

            await _webhookContext.SaveChangesAsync().ConfigureAwait(false);

            return _webhookContext.WebhookEvent.Single(s => s.Id == webhookEvent.Id).Id;

        }

        public virtual Guid InsertAndGetId(WebhookEvent webhookEvent)
        {
            _webhookContext.WebhookEvent.Add(webhookEvent);

            _webhookContext.SaveChanges();

            return _webhookContext.WebhookEvent.Single(s => s.Id == webhookEvent.Id).Id;
        }

        public virtual Task<WebhookEvent> GetAsync(Guid tenantId, Guid id)
        {
            return _webhookContext.WebhookEvent.SingleAsync(s => s.TenantId == tenantId && s.Id == id);
        }

        public virtual WebhookEvent Get(Guid tenantId, Guid id)
        {
            return _webhookContext.WebhookEvent.Single(s => s.TenantId == tenantId && s.Id == id);
        }
    }
}
