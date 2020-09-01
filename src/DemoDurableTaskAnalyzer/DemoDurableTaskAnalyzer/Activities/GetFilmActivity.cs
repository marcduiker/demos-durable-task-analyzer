using IMDbApiLib.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DemoDurableTaskAnalyzer.Activities
{
    public class GetFilmActivity
    {
        private readonly HttpClient _httpClient;

        public GetFilmActivity(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(GetFilmActivity))]
        public async Task<SearchResult> Run(
          [ActivityTrigger] string filmName,
          ILogger logger)
        {
            var htmlEncodedFilmName = HttpUtility.HtmlEncode(filmName);
            SearchResult result = new SearchResult();
            
            var imdbApiEndpoint = Environment.GetEnvironmentVariable("ImdbApi.Endpoint");
            var imdbApiKey = Environment.GetEnvironmentVariable("ImdbApi.Key");
            var searchEndpoint = new Uri($"{imdbApiEndpoint}SearchMovie/{imdbApiKey}/{htmlEncodedFilmName}");
            var responseMessage = await _httpClient.GetAsync(searchEndpoint);
            if (responseMessage.IsSuccessStatusCode)
            {
                var searchData = await responseMessage.Content.ReadAsAsync<SearchData>();
                if (searchData.Results.Any())
                {
                    result = searchData.Results.First();
                }
            }

            return result;
        }
    }
}
