namespace Fixit_API.Models;

public class TenantProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ApartmentNumber { get; set; } = string.Empty;
    public string UnitNumber { get; set; } = string.Empty;
    public DateTime MovedInAt { get; set; }

    public User? User { get; set; }
    public ICollection<MaintenanceTicket> Tickets { get; set; } = new List<MaintenanceTicket>();
}
