using System.Text;
using coveralls_uploader.Models.Coveralls;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace coveralls_uploader.Services;

public class CoverallsService
{
    private const string CoverallsApiUrl = "https://coveralls.io/api/v1/";
    private const string JobsResource = "jobs";

    private readonly ILogger _logger;

    public CoverallsService(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task UploadAsync(Job job)
    {
        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        var jobJson= JsonConvert.SerializeObject(job, new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            NullValueHandling = NullValueHandling.Ignore
        });

        using var client = new HttpClient();
        using var formData = new MultipartFormDataContent();
        formData.Add(
            new StringContent(jobJson, Encoding.UTF8), 
            "json_file", 
            "coverage.json");
        
        _logger.LogDebug(
            "Sending the following JSON to Coveralls: {jobJson}", 
            jobJson);
        
        var response = await client.PostAsync($"{CoverallsApiUrl}{JobsResource}", formData);
        
        _logger.LogDebug(
            "Coveralls returned to following response: {statusCode} {content}", 
            response.StatusCode,
            await response.Content.ReadAsStringAsync());
    }
}