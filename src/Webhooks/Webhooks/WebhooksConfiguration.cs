using Newtonsoft.Json;

namespace Webhooks
{
    internal class WebhooksConfiguration : IWebhooksConfiguration
    {
        public TimeSpan TimeoutDuration { get; set; } = TimeSpan.FromSeconds(60);

        public int MaxSendAttemptCount { get; set; } = 5;

        public JsonSerializerSettings? JsonSerializerSettings { get; set; } = null;

        public bool IsAutomaticSubscriptionDeactivationEnabled { get; set; } = false;

        public int MaxConsecutiveFailCountBeforeDeactivateSubscription { get; set; }

        public WebhooksConfiguration()
        {
            MaxConsecutiveFailCountBeforeDeactivateSubscription = MaxSendAttemptCount * 3; //deactivates subscription if 3 webhook can not be sent.
        }
    }
}
