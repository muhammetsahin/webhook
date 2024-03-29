using Webhooks.Extensions;

namespace Webhooks
{
    public static class WebhookSubscriptionInfoExtensions
    {
        /// <summary>
        /// Return List of subscribed webhooks definitions <see cref="WebhookSubscriptionInfo.Webhooks"/>
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSubscribedWebhooks(this WebhookSubscriptionInfo webhookSubscription)
        {
            if (string.IsNullOrWhiteSpace(webhookSubscription.Webhooks))
            {
                return new List<string>();
            }

            return webhookSubscription.Webhooks.FromJson<List<string>>();
        }

        /// <summary>
        /// Adds webhook subscription to <see cref="WebhookSubscriptionInfo.Webhooks"/> if not exists
        /// </summary>
        /// <param name="webhookSubscription"></param>
        /// <param name="name">webhook unique name</param>
        public static void SubscribeWebhook(this WebhookSubscriptionInfo webhookSubscription, string name)
        {
            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can not be null, empty or whitespace!");
            }

            var webhookDefinitions = webhookSubscription.GetSubscribedWebhooks();
            if (webhookDefinitions.Contains(name))
            {
                return;
            }

            webhookDefinitions.Add(name);
            webhookSubscription.Webhooks = webhookDefinitions.ToJson();
        }

        /// <summary>
        ///  Removes webhook subscription from <see cref="WebhookSubscriptionInfo.Webhooks"/> if exists
        /// </summary>
        /// <param name="webhookSubscription"></param>
        /// <param name="name">webhook unique name</param>
        public static void UnsubscribeWebhook(this WebhookSubscriptionInfo webhookSubscription, string name)
        {
            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can not be null, empty or whitespace!");
            }

            var webhookDefinitions = webhookSubscription.GetSubscribedWebhooks();
            if (!webhookDefinitions.Contains(name))
            {
                return;
            }

            webhookDefinitions.Remove(name);
            webhookSubscription.Webhooks = webhookDefinitions.ToJson();
        }

        /// <summary>
        /// Clears all <see cref="WebhookSubscriptionInfo.Webhooks"/> 
        /// </summary>
        /// <param name="webhookSubscription"></param> 
        public static void RemoveAllSubscribedWebhooks(this WebhookSubscriptionInfo webhookSubscription)
        {
            webhookSubscription.Webhooks = null;
        }

        /// <summary>
        /// if subscribed to given webhook
        /// </summary>
        /// <returns></returns>
        public static bool IsSubscribed(this WebhookSubscriptionInfo webhookSubscription, string webhookName)
        {
            if (string.IsNullOrWhiteSpace(webhookSubscription.Webhooks))
            {
                return false;
            }

            return webhookSubscription.GetSubscribedWebhooks().Contains(webhookName);
        }

        /// <summary>
        /// Returns additional webhook headers <see cref="WebhookSubscriptionInfo.Headers"/>
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetWebhookHeaders(this WebhookSubscriptionInfo webhookSubscription)
        {
            if (string.IsNullOrWhiteSpace(webhookSubscription.Headers))
            {
                return new Dictionary<string, string>();
            }

            return webhookSubscription.Headers.FromJson<Dictionary<string, string>>();
        }

        /// <summary>
        /// Adds webhook subscription to <see cref="WebhookSubscriptionInfo.Webhooks"/> if not exists
        /// </summary>
        public static void AddWebhookHeader(this WebhookSubscriptionInfo webhookSubscription, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) )
            {
                throw new ArgumentNullException(nameof(key), $"{nameof(key)} can not be null, empty or whitespace!");
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), $"{nameof(value)} can not be null, empty or whitespace!");
            }

            var headers = webhookSubscription.GetWebhookHeaders();
            headers[key] = value;

            webhookSubscription.Headers = headers.ToJson();
        }

        /// <summary>
        /// Adds webhook subscription to <see cref="WebhookSubscriptionInfo.Webhooks"/> if not exists
        /// </summary>
        /// <param name="webhookSubscription"></param>
        /// <param name="header">Key of header</param>
        public static void RemoveWebhookHeader(this WebhookSubscriptionInfo webhookSubscription, string header)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                throw new ArgumentNullException(nameof(header), $"{nameof(header)} can not be null, empty or whitespace!");
            }

            var headers = webhookSubscription.GetWebhookHeaders();

            if (!headers.ContainsKey(header))
            {
                return;
            }

            headers.Remove(header);

            webhookSubscription.Headers = headers.ToJson();
        }

        /// <summary>
        /// Clears all <see cref="WebhookSubscriptionInfo.Webhooks"/> 
        /// </summary>
        /// <param name="webhookSubscription"></param> 
        public static void RemoveAllWebhookHeaders(this WebhookSubscriptionInfo webhookSubscription)
        {
            webhookSubscription.Headers = null;
        }

        public static WebhookSubscriptionInfo ToWebhookSubscriptionInfo(this WebhookSubscription webhookSubscription)
        {
            return new WebhookSubscriptionInfo
            {
                Id = webhookSubscription.Id,
                TenantId = webhookSubscription.TenantId,
                IsActive = webhookSubscription.IsActive,
                Secret = webhookSubscription.Secret,
                WebhookUri = webhookSubscription.WebhookUri,
                Webhooks = webhookSubscription.Webhooks.ToJson(),
                Headers = webhookSubscription.Headers.ToJson()
            };
        }
    }
}
