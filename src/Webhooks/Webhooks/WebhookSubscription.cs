using Newtonsoft.Json;

namespace Webhooks
{
    public class WebhookSubscription 
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Tenant id of the subscribed.
        /// </summary>
        /// 
        public Guid TenantId { get; set; }

        /// <summary>
        /// Subscription webhook endpoint
        /// </summary>
        public string WebhookUri { get; set; }

        /// <summary>
        /// Webhook secret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Is subscription active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Subscribed webhook definitions unique names. <see cref="WebhookDefinition.Name"/>
        /// </summary>
        public List<string> Webhooks { get; set; }

        /// <summary>
        /// Gets a set of additional HTTP headers.That headers will be sent with the webhook.
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        public WebhookSubscription()
        {
            IsActive = true;
            Headers = new Dictionary<string, string>();
            Webhooks = new List<string>();
        }
    }
}
