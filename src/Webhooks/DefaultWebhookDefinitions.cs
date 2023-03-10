
namespace Webhooks
{
    public static class DefaultWebhookDefinitions
    {

        public static Dictionary<string, WebhookDefinition> Webhooks { get; set; } = new Dictionary<string, WebhookDefinition>()
        {
            {Constants.MoneyTransferCompleted,new WebhookDefinition(Constants.MoneyTransferCompleted,"Money Transfer Completed",@"{""transactionId"":""XXXX-XXX-XX-XXXX"",""amount"": 15.40,""description"":""Lorem ipsam"",""currency"":""TRY"",""dueDate"":""2022-01-17 17:09:26.452"",""nationalNoOrTaxNo"":""11111111111"",""toIban"":""TR330006100519786457841326"",""toName"":""Donald Duck"",""state"":""Completed""}") },
            {Constants.MoneyTransferFailed,new WebhookDefinition(Constants.MoneyTransferFailed,"Money Transfer Failed",@"{""transactionId"":""XXXX-XXX-XX-XXXX"",""amount"": 10.20,""description"":""Lorem ipsam"",""currency"":""TRY"",""dueDate"":""2022-01-17 17:09:26.452"",""nationalNoOrTaxNo"":""11111111111"",""toIban"":""TR330006100519786457841326"",""toName"":""Donald Duck"",""state"":""Failed""}") },
            {Constants.MoneyTransferCancelled,new WebhookDefinition(Constants.MoneyTransferCancelled,"Money Transfer Cancelled",@"{""transactionId"":""XXXX-XXX-XX-XXXX"",""amount"": 8.10,""description"":""Lorem ipsam"",""currency"":""TRY"",""dueDate"":""2022-01-17 17:09:26.452"",""nationalNoOrTaxNo"":""11111111111"",""toIban"":""TR330006100519786457841326"",""toName"":""Donald Duck"",""state"":""Cancelled""}") },
            {Constants.MoneyTransferRefunded,new WebhookDefinition(Constants.MoneyTransferRefunded,"Money Transfer Refunded",@"{""transactionId"":""XXXX-XXX-XX-XXXX"",""amount"": 8.10,""description"":""Lorem ipsam"",""currency"":""TRY"",""dueDate"":""2022-01-17 17:09:26.452"",""nationalNoOrTaxNo"":""11111111111"",""toIban"":""TR330006100519786457841326"",""toName"":""Donald Duck"",""state"":""Refunded""}") },
        };
    }
}
