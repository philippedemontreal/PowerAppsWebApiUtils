using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PowerAppsWebApiUtils.Client;
using System.Linq;
using PawauBeta01.Data;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly WebApiContext _webApiContext;
        public Function1(WebApiContext webApiContext)
            => _webApiContext = webApiContext;

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger logger)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var firstName = (string)req.Query["FirstName"];
                var lastName = (string)req.Query["LastName"]?? string.Empty;
                
                var contact = 
                    _webApiContext
                    .CreateQuery<Contact>()
                    .Where(p => p.FirstName == firstName && p.LastName.StartsWith(lastName))
                    .Select(p => new Contact(p.Id){ FirstName = p.FirstName, LastName = p.LastName})
                    .FirstOrDefault();

                logger.LogInformation($"contact '{contact?.Id}");
                logger.LogInformation($"FirstName='{contact?.FirstName}'");
                logger.LogInformation($"LastName='{contact?.LastName}'");

                return new OkObjectResult(contact?.Id);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}