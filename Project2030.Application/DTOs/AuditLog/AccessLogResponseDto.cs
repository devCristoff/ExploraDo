namespace Project2030.Application.DTOs.AuditLog;

public class AccessLogResponseDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
    public DateTime AccessTime { get; set; }
    public string? IpAddress { get; set; }
    public string? Location { get; set; }
    public string Action { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? FailureReason { get; set; }
}
