using IMDbApiLib.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace DemoDurableTaskAnalyzer.Activities
{
    public class StoreResultActivity
    {
        [FunctionName(nameof(StoreResultActivity))]
        public async Task Run(
          [ActivityTrigger] SearchResult searchResult,
          IBinder binder,
          ILogger logger)
        {
            var dynamicBlobBinding = new BlobAttribute("search-results/{SearchResult.Title}.json");
            using (var writer = await binder.BindAsync<TextWriter>(dynamicBlobBinding))
            {
                var serializedSearchResult = JsonConvert.SerializeObject(searchResult, Newtonsoft.Json.Formatting.Indented);
                await writer.WriteAsync(serializedSearchResult);
            }
        }
    }
}
