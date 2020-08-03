using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FTPFA5601
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            //try
            //{
            //    System.Net.WebClient client = new System.Net.WebClient();
            //    client.Credentials = new System.Net.NetworkCredential(@"AzureUser", @"Passw0rd5689");
            //    client.UploadFile(@"ftp://104.46.7.78/file.txt", @"C:\users\macavall\Desktop\file.txt");
            //}
            //catch (Exception exc)
            //{
            //    log.LogInformation(exc.Message);
            //}

            //    Environment.GetEnvironmentVariable("ftp://104.46.7.78"),
            //    Environment.GetEnvironmentVariable("AzureUser"),
            //    Environment.GetEnvironmentVariable("Passw0rd5689"));

            try
            {

                var client = new FluentFTP.FtpClient("104.46.7.78", @"ftpuser2", "password");

                var items = await client.GetListingAsync("/home/AzureUser");

                foreach (var i in items)
                {
                    log.LogInformation($"{i.Name} {i.Size}");
                }
            }
            catch (Exception exc)
            {
                log.LogInformation(exc.Message);
            }


            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
