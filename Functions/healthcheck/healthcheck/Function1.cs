using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.Common;
using System.Text.Json.Serialization;
using System.Configuration;

namespace healthcheck
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            //[CosmosDB(
            //    databaseName: "Healthcheckdb",
            //    containerName: "HealthCheck",
            //    ConnectionStringSettings = "")] IAsyncCollector<HealthCheck> healthCheck,
            [CosmosDB(
                databaseName: "Healthcheckdb",
                containerName: "HealthCheck",
            Connection = "CosmosDBConnStr")] IAsyncCollector<HealthCheck> healthCheck,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            string responseMessage = "submitted";
           
            //pull this from the request object
            HealthCheck hc = new HealthCheck();
            hc.id = "1";
            hc.PatientID = 1;
            hc.Date = DateTime.Now;
            hc.HealthStatus = "Good";
            hc.Symptoms = new string[] { "Cough", "Fever" };

            await healthCheck.AddAsync(hc);

            return new OkObjectResult(responseMessage);
        }

        public class HealthCheck
        {
            [JsonPropertyName("id")]
            public string id { get; set; } //This property has to be lowercase "id" because of a bug in CosmosDB
            [JsonPropertyName("patientid")]
            public int PatientID { get; set; }
            [JsonPropertyName("date")]
            public DateTime Date { get; set; }
            [JsonPropertyName("healthstatus")]
            public string HealthStatus { get; set; }
            [JsonPropertyName("symptoms")]
            public string[] Symptoms { get; set; }

        }
    }
}
