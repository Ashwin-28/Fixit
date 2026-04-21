using Fixit_API.Models.Enums;

namespace Fixit_API.Models;

public class TicketStatusHistory
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public TicketStatus OldStatus { get; set; }
    public TicketStatus NewStatus { get; set; }
    public Guid ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string Notes { get; set; } = string.Empty;

    public MaintenanceTicket? Ticket { get; set; }
    public User? ChangedByUser { get; set; }
}
