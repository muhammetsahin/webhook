using System.Collections.Immutable;

namespace Webhooks
{
    internal class WebhookDefinitionManager : IWebhookDefinitionManager
    {
        private readonly IWebhooksConfiguration _webhooksConfiguration;
        private readonly Dictionary<string, WebhookDefinition> _webhookDefinitions;

        public WebhookDefinitionManager(IWebhooksConfiguration webhooksConfiguration)
        {
            _webhooksConfiguration = webhooksConfiguration;
            _webhookDefinitions = new Dictionary<string, WebhookDefinition>();
            Initialize();
        }

        private void Initialize()
        {
            foreach (var item in DefaultWebhookDefinitions.Webhooks)
            {
                _webhookDefinitions.Add(item.Key,item.Value);
            }
        }

        public void Add(WebhookDefinition webhookDefinition)
        {
            if (_webhookDefinitions.ContainsKey(webhookDefinition.Name))
            {
                throw new Exception("There is already a webhook definition with given name: " + webhookDefinition.Name + ". Webhook names must be unique!");
            }

            _webhookDefinitions.Add(webhookDefinition.Name, webhookDefinition);
        }

        public WebhookDefinition GetOrNull(string name)
        {
            if (!_webhookDefinitions.ContainsKey(name))
            {
                return null;
            }

            return _webhookDefinitions[name];
        }

        public WebhookDefinition Get(string name)
        {
            if (!_webhookDefinitions.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Webhook definitions does not contain a definition with the key \"{name}\".");
            }

            return _webhookDefinitions[name];
        }

        public IReadOnlyList<WebhookDefinition> GetAll()
        {
            return _webhookDefinitions.Values.ToImmutableList();
        }

        public bool Remove(string name)
        {
            return _webhookDefinitions.Remove(name);
        }

        public bool Contains(string name)
        {
            return _webhookDefinitions.ContainsKey(name);
        }

        public bool IsAvailable(Guid tenantId, string name)
        {
            if (tenantId == default) // host allowed to subscribe all webhooks
            {
                return true;
            }

            var webhookDefinition = GetOrNull(name);

            if (webhookDefinition == null)
            {
                return false;
            }

            return true;
        }
    }
}
