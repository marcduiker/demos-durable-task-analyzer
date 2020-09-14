using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DemoDurableTaskAnalyzer.Orchestrators
{
    public class OrchestratorWithDF2xxWarnings
    {
        [FunctionName(nameof(OrchestratorWithDF2xxWarnings))]
        public void Run(
          // DF201 OrchestrationTrigger must be used with a DurableOrchestrationContext or DurableOrchestrationContextBase.
          [OrchestrationTrigger] IDurableClient context,
          //[OrchestrationTrigger] IDurableOrchestrationContext context,
          ILogger logger)
        {
            // Nothing to see here.
        }
    }
}
