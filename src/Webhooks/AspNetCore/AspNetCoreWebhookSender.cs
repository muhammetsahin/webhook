using System.Net;
using Webhooks;

namespace AspNetCore.Webhook
{
    public class AspNetCoreWebhookSender : DefaultWebhookSender
    {
        public const string WebhookSenderHttpClientName = "WebhookSenderHttpClient";
        private readonly IWebhooksConfiguration _webhooksConfiguration;
        private readonly IWebhookManager _webhookManager;
        private readonly IHttpClientFactory _clientFactory;

        public AspNetCoreWebhookSender(
            IWebhooksConfiguration webhooksConfiguration,
            IWebhookManager webhookManager,
            IHttpClientFactory clientFactory)
            : base(webhooksConfiguration, webhookManager)
        {
            _webhooksConfiguration = webhooksConfiguration;
            _clientFactory = clientFactory;
            _webhookManager = webhookManager;
        }

        protected override async Task<(bool isSucceed, HttpStatusCode statusCode, string content)> SendHttpRequest(HttpRequestMessage request)
        {
            var client = _clientFactory.CreateClient(WebhookSenderHttpClientName);
            client.Timeout = _webhooksConfiguration.TimeoutDuration;

            var response = await client.SendAsync(request).ConfigureAwait(false);
            var isSucceed = response.IsSuccessStatusCode;
            var statusCode = response.StatusCode;
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return (isSucceed, statusCode, content);
        }
    }
}
