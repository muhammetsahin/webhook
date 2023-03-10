using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Webhooks
{
    /// <summary>
    /// Table for store webhook work items. Each item stores web hook send attempt of <see cref="WebhookEvent"/> to subscribed tenants
    /// </summary>
    [Table("WebhookSendAttempts", Schema = "Webhook")]
    public class WebhookSendAttempt 
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// <see cref="WebhookEvent"/> foreign id 
        /// </summary>
        [Required]
        public Guid WebhookEventId { get; set; }

        /// <summary>
        /// <see cref="WebhookSubscription"/> foreign id 
        /// </summary>
        [Required]
        public Guid WebhookSubscriptionId { get; set; }

        /// <summary>
        /// Webhook response content that webhook endpoint send back
        /// </summary>
        public string? Response { get; set; }

        /// <summary>
        /// Webhook response status code that webhook endpoint send back
        /// </summary>
        public HttpStatusCode? ResponseStatusCode { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid TenantId { get; set; }

        /// <summary>
        /// WebhookEvent of this send attempt.
        /// </summary>
        [ForeignKey("WebhookEventId")]
        public virtual WebhookEvent WebhookEvent { get; set; }
    }
}
