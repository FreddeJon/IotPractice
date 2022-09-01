using WebApi.Helpers;

namespace WebApi.Services;

public interface IDeviceService
{
    Task<string> GetDeviceAsync(string deviceId);
    Task<string> CreateDeviceAsync(string deviceId);
}
public class DeviceService : IDeviceService
{
    private readonly RegistryManager _registryClient;

    public DeviceService(IConfiguration configuration)
    {
        _registryClient = RegistryManager.CreateFromConnectionString(configuration["IotHub"]);
    }
    public async Task<string> GetDeviceAsync(string deviceId)
    {
        try
        {
             var device = await _registryClient.GetDeviceAsync(deviceId);
             if (device is not null)
                 return device.GenerateConnectionString();
        }
        catch
        {
            // ignored
        }
        return null!;
    }

    public async Task<string> CreateDeviceAsync(string deviceId)
    {
        try
        {
            var device = await _registryClient.AddDeviceAsync(new Device(deviceId));
            if (device is not null)
                return device.GenerateConnectionString();
        }
        catch
        {
            // ignored
        }

        return null!;
    }
}