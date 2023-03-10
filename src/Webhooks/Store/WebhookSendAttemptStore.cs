using Microsoft.EntityFrameworkCore;
using Webhooks.Data;
using Webhooks.ResultObject;

namespace Webhooks.Store
{
    /// <summary>
    /// Implements <see cref="IWebhookSendAttemptStore"/> using repositories.
    /// </summary>
    public class WebhookSendAttemptStore : IWebhookSendAttemptStore
    {

        private readonly WebhookContext _webhookContext;
        public WebhookSendAttemptStore(WebhookContext webhookContext)
        {
            _webhookContext = webhookContext;
        }

        public virtual async Task InsertAsync(WebhookSendAttempt webhookSendAttempt)
        {
            webhookSendAttempt.CreationTime = DateTime.Now;
            await _webhookContext.WebhookSendAttempt.AddAsync(webhookSendAttempt).ConfigureAwait(false);

            await _webhookContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual void Insert(WebhookSendAttempt webhookSendAttempt)
        {
            webhookSendAttempt.CreationTime = DateTime.Now;
            _webhookContext.WebhookSendAttempt.Add(webhookSendAttempt);

            _webhookContext.SaveChanges();
        }

        public virtual async Task UpdateAsync(WebhookSendAttempt webhookSendAttempt)
        {
            webhookSendAttempt.LastModificationTime = DateTime.Now;

            var existing = _webhookContext.WebhookSendAttempt.Local.SingleOrDefault(s => s.Id == webhookSendAttempt.Id);
            if (existing == null)
                _webhookContext.Update(webhookSendAttempt); // Not tracked, so call Update to update and track
            else
                _webhookContext.Entry(existing).CurrentValues.SetValues(webhookSendAttempt); // Tracked, so copy across values.


            await _webhookContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual void Update(WebhookSendAttempt webhookSendAttempt)
        {
            webhookSendAttempt.LastModificationTime = DateTime.Now;

            var existing = _webhookContext.WebhookSendAttempt.Local.SingleOrDefault(s => s.Id == webhookSendAttempt.Id);
            if (existing == null)
                _webhookContext.Update(webhookSendAttempt); // Not tracked, so call Update to update and track
            else
                _webhookContext.Entry(existing).CurrentValues.SetValues(webhookSendAttempt); // Tracked, so copy across values.


            _webhookContext.SaveChanges();
        }

        public virtual async Task<WebhookSendAttempt> GetAsync(Guid tenantId, Guid id)
        {
            return await _webhookContext.WebhookSendAttempt.SingleAsync(s => s.TenantId == tenantId && s.Id == id).ConfigureAwait(false);
        }

        public virtual WebhookSendAttempt Get(Guid tenantId, Guid id)
        {
            return _webhookContext.WebhookSendAttempt.Single(s => s.TenantId == tenantId && s.Id == id);
        }

        public virtual async Task<int> GetSendAttemptCountAsync(Guid tenantId, Guid webhookEventId,
            Guid webhookSubscriptionId)
        {
            int sendAttemptCount;

            sendAttemptCount = await _webhookContext.WebhookSendAttempt
                .CountAsync(attempt =>
                    attempt.WebhookEventId == webhookEventId &&
                    attempt.WebhookSubscriptionId == webhookSubscriptionId
                ).ConfigureAwait(false);

            return sendAttemptCount;
        }

        public virtual int GetSendAttemptCount(Guid tenantId, Guid webhookEventId, Guid webhookSubscriptionId)
        {
            int sendAttemptCount;

            sendAttemptCount = _webhookContext.WebhookSendAttempt
                .Count(attempt =>
                    attempt.WebhookEventId == webhookEventId &&
                    attempt.WebhookSubscriptionId == webhookSubscriptionId
                );


            return sendAttemptCount;
        }

        public virtual async Task<bool> HasXConsecutiveFailAsync(Guid tenantId, Guid subscriptionId, int failCount)
        {
            bool result;

            if (await _webhookContext.WebhookSendAttempt.CountAsync(x => x.TenantId == tenantId && x.WebhookSubscriptionId == subscriptionId).ConfigureAwait(false) <
                failCount)
            {
                result = false;
            }
            else
            {
                result = !await _webhookContext.WebhookSendAttempt
                                               .OrderByDescending(attempt => attempt.CreationTime)
                                               .Take(failCount)
                                               .Where(attempt => attempt.ResponseStatusCode == System.Net.HttpStatusCode.OK)
                                               .AnyAsync().ConfigureAwait(false);
            }
            return result;
        }

        public virtual async Task<IPagedDataResult<List<WebhookSendAttempt>>> GetAllSendAttemptsBySubscriptionAsPagedListAsync(
            Guid tenantId,
            Guid subscriptionId,
            int maxResultCount,
            int skipCount)
        {
            var query = _webhookContext.WebhookSendAttempt.Include(s => s.WebhookEvent)
                .Where(attempt => attempt.TenantId == tenantId &&
                    attempt.WebhookSubscriptionId == subscriptionId
                );

            var totalCount = await query.CountAsync().ConfigureAwait(false);

            var list = await query
                .OrderByDescending(attempt => attempt.CreationTime)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync()
                .ConfigureAwait(false);

            var sendAttempts = PagedDataResult<List<WebhookSendAttempt>>.Success(list);
            sendAttempts.TotalCount = totalCount;

            return sendAttempts;
        }

        public virtual IPagedDataResult<List<WebhookSendAttempt>> GetAllSendAttemptsBySubscriptionAsPagedList(Guid tenantId,
            Guid subscriptionId, int maxResultCount, int skipCount)
        {

            var query = _webhookContext.WebhookSendAttempt.Include(s => s.WebhookEvent)
                .Where(attempt => attempt.TenantId == tenantId &&
                    attempt.WebhookSubscriptionId == subscriptionId
                );

            var totalCount = query.Count();

            var list = query
                .OrderByDescending(attempt => attempt.CreationTime)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToList();

            var sendAttempts = PagedDataResult<List<WebhookSendAttempt>>.Success(list);
            sendAttempts.TotalCount = totalCount;

            return sendAttempts;
        }

        public virtual async Task<List<WebhookSendAttempt>> GetAllSendAttemptsByWebhookEventIdAsync(Guid tenantId,
            Guid webhookEventId)
        {
            List<WebhookSendAttempt> sendAttempts;

            sendAttempts = await
                _webhookContext.WebhookSendAttempt
                    .Where(attempt => attempt.TenantId == tenantId && attempt.WebhookEventId == webhookEventId)
                    .OrderByDescending(attempt => attempt.CreationTime)
                    .ToListAsync()
                    .ConfigureAwait(false);

            return sendAttempts;
        }

        public virtual List<WebhookSendAttempt> GetAllSendAttemptsByWebhookEventId(Guid tenantId, Guid webhookEventId)
        {
            List<WebhookSendAttempt> sendAttempts;

            sendAttempts =
               _webhookContext.WebhookSendAttempt
                   .Where(attempt => attempt.TenantId == tenantId && attempt.WebhookEventId == webhookEventId)
                   .OrderByDescending(attempt => attempt.CreationTime)
                   .ToList();

            return sendAttempts;
        }

    }
}
