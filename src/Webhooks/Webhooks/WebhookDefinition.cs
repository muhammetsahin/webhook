using System;

namespace Webhooks
{
    public class WebhookDefinition
    {
        /// <summary>
        /// Unique name of the webhook.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Display name of the webhook.
        /// Optional.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description for the webhook.
        /// Optional.
        /// </summary>
        public string Description { get; set; }


        public WebhookDefinition(string name, string? displayName = null, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can not be null, empty or whitespace!");
            }

            Name = name.Trim();
            DisplayName = displayName;
            Description = description;
        }
    }
}
