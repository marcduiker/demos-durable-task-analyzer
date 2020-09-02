using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;

namespace DemoDurableTaskAnalyzer.Activities
{
    public class ActivityWithDateTimeParameter
    {
        [FunctionName(nameof(ActivityWithDateTimeParameter))]
        public string Run(
          [ActivityTrigger] DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
