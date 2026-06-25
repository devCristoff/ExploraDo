namespace Project2030.Application.Interfaces;

public interface IGeolocationService
{
    Task<string> ResolveLocationAsync(string ipAddress);
}
