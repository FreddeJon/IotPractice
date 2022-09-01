using System;
using System.Threading.Tasks;
using AzureFunctions.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureFunctions;

public static class GetDevice
{
    private static readonly RegistryManager RegistryManager =
        RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

    [FunctionName("GetDevice")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
        HttpRequest req)
    {
        string deviceId = req.Query["deviceId"];

        if (string.IsNullOrEmpty(deviceId))
        {
            return new BadRequestResult();
        }

        try
        {
            var device = await RegistryManager.GetDeviceAsync(deviceId);
            if (device != null)
            {
                return new OkObjectResult(device.GenerateConnectionString());
            }
        }
        catch
        {
            // ignored
        }

        return new BadRequestResult();
    }
}