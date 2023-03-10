using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webhooks
{
    /// <summary>
    /// Store created web hooks. To see who get that webhook check with <see cref="WebhookSendAttempt.WebhookEventId"/> and you can get <see cref="WebhookSendAttempt.WebhookSubscriptionId"/>
    /// </summary>
    [Table("WebhookEvents", Schema = "Webhook")]
    public class WebhookEvent
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Webhook unique name <see cref="WebhookDefinition.Name"/>
        /// </summary>
        [Required]
        public string WebhookName { get; set; } = default!;

        /// <summary>
        /// Webhook data as JSON string.
        /// </summary>
        public string Data { get; set; } = default!;

        public DateTime CreationTime { get; set; }

        public Guid TenantId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
