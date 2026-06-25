namespace Project2030.Application.DTOs.Permissions;

public class AssignPermissionRequestDto
{
    public string RoleId { get; set; } = string.Empty;
    public int ModuleId { get; set; }
    public bool CanCreate { get; set; }
    public bool CanRead { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
}
