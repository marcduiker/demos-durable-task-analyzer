using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;

namespace DemoDurableTaskAnalyzer.Activities
{
    public class ActivityWithGuidParameter
    {

        [FunctionName(nameof(ActivityWithGuidParameter))]
        public string Run(
          [ActivityTrigger] Guid guid)
        {
            return guid.ToString();
        }
    }
}
