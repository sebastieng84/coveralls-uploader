using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using coveralls_uploader.Models.Coveralls;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace coveralls_uploader.Services
{
    public class CoverallsService
    {
        private const string CoverallsApiUrl = "https://coveralls.io/api/v1/";
        private const string JobsResource = "jobs";
    
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
    
        public CoverallsService(HttpClient httpClient,  ILogger logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        
        public async Task UploadAsync(Job job)
        {
            var jobJson = ConvertJobToJson(job);
            
            using var formData = new MultipartFormDataContent();
            formData.Add(
                new StringContent(jobJson, Encoding.UTF8), 
                "json_file", 
                "coverage.json");
            
            _logger.LogDebug(
                "Sending the following JSON to Coveralls: {jobJson}", 
                jobJson);
            
            var response = await _httpClient.PostAsync($"{CoverallsApiUrl}{JobsResource}", formData);
            
            // TODO: change LogLevel to Info and more meaningful message
            _logger.LogDebug(
                "Coveralls returned to following response: {statusCode} {content}", 
                response.StatusCode,
                await response.Content.ReadAsStringAsync());
        }
    
        private static string ConvertJobToJson(Job job)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            
            return JsonConvert.SerializeObject(job, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}