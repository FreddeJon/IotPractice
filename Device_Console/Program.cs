using System.Net.Http.Json;
using System.Text;
using Microsoft.Azure.Devices.Client;

namespace Device_Console;

public class Program
{
    private const string DeviceId = "testar15";
    private static DeviceClient _deviceClient;

    public static async Task Main()
    {
        _deviceClient = await InitializeAsync(_deviceClient, DeviceId);


        while (true)
        {
            if (_deviceClient is not null)
            {
                await SendMessageAsync(_deviceClient,
                    $"Message from: {DeviceId} at {DateTimeOffset.Now.ToUnixTimeSeconds()}");
            }
            else
            {
                Console.WriteLine("Could not connect");
                break;
            }
        }

        Console.ReadKey();
    }

    private static async Task<DeviceClient> InitializeAsync(DeviceClient deviceClient, string deviceId)
    {
        await Task.Delay(10000);
        try
        {
            using var client = new HttpClient();
            var httpResponse = await client.GetAsync($"http://localhost:7071/api/GetDevice?deviceId={deviceId}");
            var connection = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                httpResponse = await client.PostAsJsonAsync("http://localhost:7071/api/CreateDevice", new {deviceId});
                connection = await httpResponse.Content.ReadAsStringAsync();
            }

            return DeviceClient.CreateFromConnectionString(connection);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error \n" + e);
        }

        return null!;
    }

    private static async Task SendMessageAsync(DeviceClient deviceClient, string message)
    {
        try
        {
            await deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(message)))!;
            Console.WriteLine(message);
        }
        catch
        {
            // ignored
        }
        await Task.Delay(10000);
    }
}