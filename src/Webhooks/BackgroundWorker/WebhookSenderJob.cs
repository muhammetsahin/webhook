using Hangfire;
using Serilog;

namespace Webhooks.BackgroundWorker
{
    public interface IWebhookSenderJob
    {
        Task ExecuteAsync(WebhookSenderArgs args);
    }

    [AutomaticRetry(Attempts = 1, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public class WebhookSenderJob : IWebhookSenderJob
    {
        private readonly IWebhooksConfiguration _webhooksConfiguration;
        private readonly IWebhookSubscriptionManager _webhookSubscriptionManager;
        private readonly IWebhookSendAttemptStore _webhookSendAttemptStore;
        private readonly IWebhookSender _webhookSender;

        public WebhookSenderJob(
            IWebhooksConfiguration webhooksConfiguration,
            IWebhookSubscriptionManager webhookSubscriptionManager,
            IWebhookSendAttemptStore webhookSendAttemptStore,
            IWebhookSender webhookSender)
        {
            _webhooksConfiguration = webhooksConfiguration;
            _webhookSubscriptionManager = webhookSubscriptionManager;
            _webhookSendAttemptStore = webhookSendAttemptStore;
            _webhookSender = webhookSender;
        }

        public async Task ExecuteAsync(WebhookSenderArgs args)
        {
            if (args.TryOnce)
            {
                try
                {
                    await SendWebhook(args);
                }
                catch (Exception e)
                {
                    // Log.Error("An error occured while sending webhook with try once.", e);
                    // ignored
                }
            }
            else
            {
                await SendWebhook(args);
            }
        }

        private async Task SendWebhook(WebhookSenderArgs args)
        {
            if (args.WebhookEventId == default) return;

            if (args.WebhookSubscriptionId == default) return;

            if (!args.TryOnce)
            {
                var sendAttemptCount = await _webhookSendAttemptStore.GetSendAttemptCountAsync(
                    args.TenantId,
                    args.WebhookEventId,
                    args.WebhookSubscriptionId
                ).ConfigureAwait(false);

                if (sendAttemptCount > _webhooksConfiguration.MaxSendAttemptCount) return;
            }

            try
            {
                await _webhookSender.SendWebhookAsync(args).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while sending webhook with try once.", ex);

                // no need to retry to send webhook since subscription disabled
                if (!await TryDeactivateSubscriptionIfReachedMaxConsecutiveFailCountAsync(
                        args.TenantId,
                        args.WebhookSubscriptionId).ConfigureAwait(false))
                {
                    throw; //Throw exception to re-try sending webhook
                }
            }
        }

        private async Task<bool> TryDeactivateSubscriptionIfReachedMaxConsecutiveFailCountAsync(Guid tenantId, Guid subscriptionId)
        {
            if (!_webhooksConfiguration.IsAutomaticSubscriptionDeactivationEnabled)
            {
                return false;
            }

            var hasXConsecutiveFail = await _webhookSendAttemptStore
                .HasXConsecutiveFailAsync(
                    tenantId,
                    subscriptionId,
                    _webhooksConfiguration.MaxConsecutiveFailCountBeforeDeactivateSubscription
                ).ConfigureAwait(false);

            if (!hasXConsecutiveFail)
            {
                return false;
            }

            await _webhookSubscriptionManager.ActivateWebhookSubscriptionAsync(subscriptionId, false).ConfigureAwait(false);
            return true;
        }
    }
}
