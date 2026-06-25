using System.Net.Http.Json;
using Project2030.Application.Interfaces;

namespace Project2030.Infrastructure.Services;

public class GeolocationService : IGeolocationService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GeolocationService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> ResolveLocationAsync(string ipAddress)
    {
        if (IsPrivateOrLocalIp(ipAddress))
            return "Local";

        try
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(3);

            IpApiResponse? response = await client.GetFromJsonAsync<IpApiResponse>(
                $"http://ip-api.com/json/{ipAddress}?fields=status,city,country");

            if (response?.Status == "success" && !string.IsNullOrEmpty(response.City))
                return $"{response.City}, {response.Country}";
        }
        catch
        {
            // Silently fall through to default
        }

        return "Desconocida";
    }

    private static bool IsPrivateOrLocalIp(string ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "Unknown")
            return true;

        if (ipAddress is "::1" or "127.0.0.1" or "localhost")
            return true;

        if (ipAddress.StartsWith("192.168.") || ipAddress.StartsWith("10.") || ipAddress.StartsWith("172."))
            return true;

        return false;
    }

    private sealed class IpApiResponse
    {
        public string Status { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
