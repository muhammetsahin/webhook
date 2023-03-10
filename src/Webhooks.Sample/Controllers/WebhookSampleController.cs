using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Webhooks.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookSampleController : ControllerBase
    {

        private readonly ILogger<WebhookSampleController> _logger;
        private readonly IWebhookPublisher _webhookPublisher;
        public WebhookSampleController(ILogger<WebhookSampleController> logger, 
            IWebhookPublisher webhookPublisher)
        {
            _logger = logger;
            _webhookPublisher = webhookPublisher;
        }

        [HttpPost]
        public async Task MyWebHookEndpoint()
        {
            using (StreamReader reader = new(HttpContext.Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();

                if (!IsSignatureCompatible("whs_3b53fc531bcb4676b95548d3e01d5052", body))
                {
                    throw new Exception("Unexpected Signature");
                }
                //It is certain that Webhook has not been modified.
            }
        }



        [HttpGet]
        public async Task SendWebHook() => await _webhookPublisher.PublishAsync(Constants.MoneyTransferCompleted, new
        {
            transactionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
            bankTransactionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
            amount = 10M,
            description = "",
            currency = "TRY",
            dueDate = DateTime.Now,
            nationalNoOrTaxNo = "",
            toIban = "",
            toName = "",
            state = "Completed",
            transferType = "Fast",
            fromIban = "",
            fromAccountNo = "",
            regexData = ""
        }, new Guid("6A4E79EA-B636-42D5-9148-90F666970987"), true).ConfigureAwait(false);


        private bool IsSignatureCompatible(string secret, string body)
        {
            if (!HttpContext.Request.Headers.ContainsKey("finflow-webhook-signature"))
            {
                return false;
            }

            var receivedSignature = HttpContext.Request.Headers["finflow-webhook-signature"].ToString().Split("=");//will be something like "sha256=whs_XXXXXXXXXXXXXX"
                                                                                                                   //It starts with hash method name (currently "sha256") then continue with signature. You can also check if your hash method is true.

            string computedSignature;
            switch (receivedSignature[0])
            {
                case "sha256":
                    var secretBytes = Encoding.UTF8.GetBytes(secret);
                    using (var hasher = new HMACSHA256(secretBytes))
                    {
                        var data = Encoding.UTF8.GetBytes(body);
                        computedSignature = BitConverter.ToString(hasher.ComputeHash(data));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return computedSignature == receivedSignature[1];
        }
    }
}
