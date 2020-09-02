using DemoDurableTaskAnalyzer.Activities;
using DemoDurableTaskAnalyzer.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoDurableTaskAnalyzer.Orchestrators
{
    public class ChainingOrchestrator
    {
        [FunctionName(nameof(ChainingOrchestrator))]
        public async Task<OrchestratorResult> Run(
          [OrchestrationTrigger] IDurableOrchestrationContext context,
          ILogger logger)
        {
            // Since the orchestrator code is being replayed many times
            // don't depend on non-deterministic behavior or blocking calls such as:
            // - DateTime.Now (use context.CurrentUtcDateTime instead)
            // - Guid.NewGuid (use context.NewGuid instead)
            // - System.IO
            // - Thread.Sleep/Task.Delay (use context.CreateTimer instead)
            //
            // More info: https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-code-constraints

            var orchestrationResult = new OrchestratorResult();

            // DF101
            var currentDateTime = DateTime.UtcNow;
            //var currentDateTime = context.CurrentUtcDateTime;
            var activityWithDateTimeParameterResult = await context.CallActivityAsync<string>(
                nameof(ActivityWithDateTimeParameter),
                currentDateTime);
            orchestrationResult.Items.Add(activityWithDateTimeParameterResult);

            // DF102
            var newGuid = Guid.NewGuid();
            //var newGuid = context.NewGuid;
            var activityWithGuidParameterResult = await context.CallActivityAsync<string>(
                nameof(ActivityWithGuidParameter),
                context.NewGuid());
            orchestrationResult.Items.Add(activityWithGuidParameterResult);

            // DF103
            await Task.Delay(5000);
            // Perform the Delay in the activity or call an activity with RetryOptions.

            // DF104
            await Task.Run(() => { return "Nooooooooooooo"; } );
            // Perform the code inside an activity.

            // DF105
            const string uri = "https://github.com/Azure/azure-functions-durable-extension";
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync(uri);
            //var durableRequest = new DurableHttpRequest(HttpMethod.Get, new Uri(uri));
            //var durableResponse = await context.CallHttpAsync(durableRequest);

            // DF106
            var someSetting = Environment.GetEnvironmentVariable("SettingThatWillBeChangedWhenYouLeastExpectIt");

            // DF107
            var newGuidFromMethod = GetMeANewGuid();

            return orchestrationResult;
        }

        private static Guid GetMeANewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
