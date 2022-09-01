using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public class ReadMessages
    {
        [FunctionName("ReadMessages")]
        public void Run([IoTHubTrigger("messages/events", Connection = "IotHubEndpoint")]EventData message, ILogger log)
        {
            log.LogInformation($"{Encoding.UTF8.GetString(message.Body)}");
        }
    }
}