
namespace Webhooks
{
    public class WebhookPayload
    {
        public string Id { get; set; }

        public string WebhookEvent { get; set; }

        public int Attempt { get; set; }

        public dynamic Data { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public WebhookPayload(string id, string webhookEvent, int attempt)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(webhookEvent))
            {
                throw new ArgumentNullException(nameof(webhookEvent));
            }

            Id = id;
            WebhookEvent = webhookEvent;
            Attempt = attempt;
            CreationTimeUtc = DateTime.UtcNow;
        }
    }
}
