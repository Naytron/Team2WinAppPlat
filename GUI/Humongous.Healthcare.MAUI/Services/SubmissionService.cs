using Humongous.Healthcare.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Humongous.Healthcare.MAUI.Services
{
    internal class SubmissionService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        string uriPrefix = "";
        public SubmissionService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public async Task<bool> AddSubmission(Submission sub)
        {
            string uri = uriPrefix;
            string json = JsonSerializer.Serialize<Submission>(sub, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(uri, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Submission>> GetAllSubmissions()
        {
            string uri = uriPrefix;
            HttpResponseMessage response = await _client.GetAsync(uri);
            var ret = JsonSerializer.Deserialize<List<Submission>>(response.Content.ToString(), _serializerOptions);
            return ret;
        }
    }
}
