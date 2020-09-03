using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;

namespace DemoDurableTaskAnalyzer.Activities
{
    public class ActivityWithDateTimeAndGuidTuple
    {
        [FunctionName(nameof(ActivityWithDateTimeAndGuidTuple))]
        public string Run(
          [ActivityTrigger] (DateTime DateTime, Guid Guid) input)
        {
            return $"{input.DateTime} & {input.Guid}";
        }
    }
}
