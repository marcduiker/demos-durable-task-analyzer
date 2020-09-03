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
            // https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-code-constraints

            var orchestrationResult = new OrchestratorResult(context.InstanceId);

            // DF101 DateTime calls must be deterministic inside an orchestrator function.
            var currentDateTime = DateTime.UtcNow;
            // Use CurrentUtcDateTime from the IDurableOrchestrationContext:
            //var currentDateTime = context.CurrentUtcDateTime;

            var activityWithDateTimeParameterResult = await context.CallActivityAsync<string>(
                nameof(ActivityWithDateTimeParameter),
                currentDateTime);
            orchestrationResult.Items.Add(activityWithDateTimeParameterResult);

            // DF102 Guid calls must be deterministic inside an orchestrator function.
            var newGuid = Guid.NewGuid();
            // Use NewGuid from the IDurableOrchestrationContext:
            //var newGuid = context.NewGuid;

            var activityWithGuidParameterResult = await context.CallActivityAsync<string>(
                nameof(ActivityWithGuidParameter),
                newGuid);
            orchestrationResult.Items.Add(activityWithGuidParameterResult);

            // DF103 Thread.Sleep and Task.Delay calls are not allowed inside an orchestrator function.
            await Task.Delay(5000);
            // Use a Durable Timer from the IDurableOrchestrationContext:
            //var timeToWait = TimeSpan.FromSeconds(5);
            //var timeToContinue = context.CurrentUtcDateTime.Add(timeToWait);
            //await context.CreateTimer(timeToContinue, CancellationToken.None);

            // DF104 Thread and Task calls must be deterministic inside an orchestrator function.
            var externalResult = await Task.Run(() => { return CallToApiWhichReturnsSomethingDifferentEachTime(); } );
            // Perform the code inside an activity.

            // DF105 I/O operations are not allowed inside an orchestrator function.
            const string uri = "https://github.com/Azure/azure-functions-durable-extension";
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync(uri);
            // Use the CallHttpAsync method from the IDurableOrchestrationContext:
            //var durableRequest = new DurableHttpRequest(HttpMethod.Get, new Uri(uri));
            //var durableResponse = await context.CallHttpAsync(durableRequest);

            // DF106 Environment variables must be accessed in a deterministic way inside an orchestrator function.
            var someSetting = Environment.GetEnvironmentVariable("SettingThatWillBeChangedWhenYouLeastExpectIt");

            // DF107 Methods definied in source code that are used in an orchestrator must be deterministic.
            var newGuidFromMethod = GetMeANewGuid();

            // DF108 Activity function call is using the wrong argument type.
            (Guid Guid, DateTime Date) activityInput = (context.NewGuid(), context.CurrentUtcDateTime);
            //(DateTime Date, Guid Guid) activityInput = (context.CurrentUtcDateTime, context.NewGuid());
            var activityUsingNameOfResult = await context.CallActivityAsync<string>(
                nameof(ActivityWithDateTimeAndGuidTuple),
                activityInput);
            orchestrationResult.Items.Add(activityUsingNameOfResult);

            // DF109 Activity function call references unknown Activity function.
            await context.CallActivityAsync<string>(
                "ActivityWithGuidParameters",
                context.NewGuid());
            // I suggest you always have one function per class and use nameof(ClassName)
            //await context.CallActivityAsync<string>(
            //    nameof(ActivityWithGuidParameter),
            //    context.NewGuid());


            // DF110 Activity function call return type doesn't match function definition return type.
            await context.CallActivityAsync<Guid>(
                nameof(ActivityWithGuidParameter),
                context.NewGuid());

            return orchestrationResult;
        }

        private static string CallToApiWhichReturnsSomethingDifferentEachTime()
        {
            return "or maybe not";
        }

        private static Guid GetMeANewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
