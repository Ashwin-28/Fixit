using Fixit_API.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Fixit_API.Models;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public TenantProfile? TenantProfile { get; set; }
    public EmployeeProfile? EmployeeProfile { get; set; }
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();
    public ICollection<TicketStatusHistory> TicketStatusHistoryEntries { get; set; } = new List<TicketStatusHistory>();
}
