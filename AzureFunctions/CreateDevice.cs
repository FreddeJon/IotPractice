using System;
using System.IO;
using System.Threading.Tasks;
using AzureFunctions.Helpers;
using AzureFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace AzureFunctions;

public static class CreateDevice
{
    private static readonly RegistryManager RegistryManager =
        RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

    [FunctionName("CreateDevice")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req
    )
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<DeviceRequest>(requestBody);

        if (data == null || string.IsNullOrEmpty(data.DeviceId))
        {
            return new BadRequestResult();
        }


        try
        {
            var device = await RegistryManager.AddDeviceAsync(new Device(data.DeviceId));

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