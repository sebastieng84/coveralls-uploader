using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using coveralls_uploader.Models.Coveralls;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace coveralls_uploader.Services
{
    public class CoverallsService
    {
        private const string CoverallsApiUrl = "https://coveralls.io/api/v1/";
        private const string JobsResource = "jobs";

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
            formData.Add(new StringContent(jobJson, Encoding.UTF8), "json_file", "coverage.json");

            var response = await client.PostAsync($"{CoverallsApiUrl}{JobsResource}", formData);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}