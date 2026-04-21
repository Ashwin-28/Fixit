using Fixit_API.Models.Enums;

namespace Fixit_API.Models;

public class MaintenanceTicket
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? AssignedEmployeeId { get; set; }
    public TicketCategory Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RoomLocation { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public TenantProfile? Tenant { get; set; }
    public EmployeeProfile? AssignedEmployee { get; set; }
    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
    public ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
    public ICollection<TicketStatusHistory> StatusHistory { get; set; } = new List<TicketStatusHistory>();
}
