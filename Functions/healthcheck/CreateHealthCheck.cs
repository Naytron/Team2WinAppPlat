using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;


namespace Humongous.Healthcare
{
    public class CreateHealthCheck
    {
        private readonly ILogger<CreateHealthCheck> _logger;

        public CreateHealthCheck(ILogger<CreateHealthCheck> log)
        {
            _logger = log;
        }

        [FunctionName("CreateHealthCheck")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "CreateHealthCheck" })]
        [OpenApiParameter(name: "patientid", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The ID of the patient")]
        [OpenApiParameter(name: "healthstatus", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "How are you feeling?")]
        [OpenApiParameter(name: "symptoms", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Describe your symptoms")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "Healthcheckdb", containerName: "HealthCheck", Connection = "CosmosDbConnectionString" 
    )]IAsyncCollector<dynamic> documentsOut)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string patientid = req.Query["patientid"];
            string healthstatus = req.Query["healthstatus"];
            string symptoms = req.Query["symptoms"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            patientid = patientid ?? data?.name;
            healthstatus = healthstatus ?? data?.healthstatus;
            symptoms = symptoms ?? data?.symptoms;

            if (!string.IsNullOrEmpty(patientid) && !string.IsNullOrEmpty(healthstatus) && !string.IsNullOrEmpty(symptoms))
    {
        // Add a JSON document to the output container.
        await documentsOut.AddAsync(new
        {
            // create a random ID
            id = System.Guid.NewGuid().ToString(),
            patientid = patientid,
            date = System.DateTime.Now,
            healthstatus = healthstatus,
            symptoms = symptoms
        });
    }
            

            return new OkObjectResult("Success");
        }
    }
}

