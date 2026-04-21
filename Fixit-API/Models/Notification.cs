namespace Fixit_API.Models;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TicketId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
    public MaintenanceTicket? Ticket { get; set; }
}
