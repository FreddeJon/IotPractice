using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;



namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet("{deviceId}")]
    public async Task<IActionResult> GetDevice(string deviceId)
    {
        var response = await _deviceService.GetDeviceAsync(deviceId);

        if (!string.IsNullOrEmpty(response))
        {
            return new OkObjectResult(response);
        }

        return new BadRequestResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDevice(DeviceRequest req)
    {
        var response = await _deviceService.CreateDeviceAsync(req.DeviceId);
        if (!string.IsNullOrEmpty(response))
        {
            return new OkObjectResult(response);
        }

        return new BadRequestResult();
    }
}