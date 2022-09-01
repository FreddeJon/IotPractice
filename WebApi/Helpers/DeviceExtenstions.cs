namespace WebApi.Helpers;
public static class DeviceExtenstions
{
    public static string GenerateConnectionString(this Device device)
    {
        return $"HostName=systemutveckling-hub.azure-devices.net;DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";
    }
}
