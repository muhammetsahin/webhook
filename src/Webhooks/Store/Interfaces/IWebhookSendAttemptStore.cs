using Webhooks.ResultObject;

namespace Webhooks
{
    public interface IWebhookSendAttemptStore
    {
        Task InsertAsync(WebhookSendAttempt webhookSendAttempt);

        void Insert(WebhookSendAttempt webhookSendAttempt);

        Task UpdateAsync(WebhookSendAttempt webhookSendAttempt);

        void Update(WebhookSendAttempt webhookSendAttempt);

        Task<WebhookSendAttempt> GetAsync(Guid tenantId, Guid id);

        WebhookSendAttempt Get(Guid tenantId, Guid id);

        /// <summary>
        /// Returns work item count by given web hook id and subscription id, (How many times publisher tried to send web hook)
        /// </summary>
        Task<int> GetSendAttemptCountAsync(Guid tenantId, Guid webhookId, Guid webhookSubscriptionId);

        /// <summary>
        /// Returns work item count by given web hook id and subscription id. (How many times publisher tried to send web hook)
        /// </summary>
        int GetSendAttemptCount(Guid tenantId, Guid webhookId, Guid webhookSubscriptionId);

        /// <summary>
        /// Checks is there any successful webhook attempt in last <paramref name="searchCount"/> items. Should return true if there are not X number items
        /// </summary>
        Task<bool> HasXConsecutiveFailAsync(Guid tenantId, Guid subscriptionId, int searchCount);

        Task<IPagedDataResult<List<WebhookSendAttempt>>> GetAllSendAttemptsBySubscriptionAsPagedListAsync(Guid tenantId, Guid subscriptionId, int maxResultCount, int skipCount);

        IPagedDataResult<List<WebhookSendAttempt>> GetAllSendAttemptsBySubscriptionAsPagedList(Guid tenantId, Guid subscriptionId, int maxResultCount, int skipCount);

        Task<List<WebhookSendAttempt>> GetAllSendAttemptsByWebhookEventIdAsync(Guid tenantId, Guid webhookEventId);

        List<WebhookSendAttempt> GetAllSendAttemptsByWebhookEventId(Guid tenantId, Guid webhookEventId);

    }
}
