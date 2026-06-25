namespace Project2030.Application.DTOs.AuditLog;

public class AccessLogFilterDto
{
    public string? UserId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Action { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
