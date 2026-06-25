namespace Project2030.Application.DTOs.Profile;

public class UpdateProfileRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfilePhoto { get; set; }
}
