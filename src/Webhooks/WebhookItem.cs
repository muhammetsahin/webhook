
namespace Webhooks
{
    public record WebhookItem
    {
        public WebhookItem(Guid? id, 
                           string webhookUri, 
                           bool isActive, 
                           List<string> webhooks, 
                           IDictionary<string, string> headers)
        {
            Id = id;
            WebhookUri = webhookUri;
            IsActive = isActive;
            Webhooks = webhooks;
            Headers = headers;
        }

        /// <summary>
        /// Subscription webhook endpoint
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Subscription webhook endpoint
        /// </summary>
        public string WebhookUri { get; init; }

        /// <summary>
        /// Is subscription active
        /// </summary>
        public bool IsActive { get; init; }

        /// <summary>
        /// Subscribed webhook definitions unique names. <see cref="WebhookDefinition.Name"/>
        /// </summary>
        public List<string> Webhooks { get; init; }

        /// <summary>
        /// Gets a set of additional HTTP headers.That headers will be sent with the webhook.
        /// </summary>
        public IDictionary<string, string> Headers { get; init; }
    }
}
