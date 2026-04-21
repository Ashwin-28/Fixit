namespace Fixit_API.Models;

public class TicketAttachment
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }

    public MaintenanceTicket? Ticket { get; set; }
}
