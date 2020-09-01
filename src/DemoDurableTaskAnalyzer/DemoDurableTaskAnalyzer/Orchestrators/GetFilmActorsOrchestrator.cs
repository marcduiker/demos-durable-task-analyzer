using DemoDurableTaskAnalyzer.Activities;
using IMDbApiLib.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DemoDurableTaskAnalyzer.Orchestrators
{
    public class GetFilmActorsOrchestrator
    {
        [FunctionName(nameof(GetFilmActorsOrchestrator))]
        public async Task Run(
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

            var filmName = context.GetInput<string>() ?? "Back to the Future";
            var filmResult = await context.CallActivityAsync<SearchResult>(
                nameof(GetFilmActivity),
                filmName);

            await context.CallActivityAsync(
                nameof(StoreResultActivity),
                filmResult);
        }
    }
}
