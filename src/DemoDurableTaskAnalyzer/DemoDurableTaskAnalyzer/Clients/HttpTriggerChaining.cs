using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using DemoDurableTaskAnalyzer.Orchestrators;

namespace DemoDurableTaskAnalyzer
{
    public class HttpTriggerChaining
    {
        [FunctionName(nameof(HttpTriggerChaining))]
        public async Task<HttpResponseMessage> Run(
          [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethod.Get))] HttpRequestMessage requestMessage,
          [DurableClient] IDurableClient client,
          ILogger logger)
        {
            var instanceId = await client.StartNewAsync(
                nameof(ChainingOrchestrator));

            return client.CreateCheckStatusResponse(requestMessage, instanceId);
        }
    }
}
