namespace Fixit_API.Models;

public class EmployeeProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }

    public User? User { get; set; }
    public ICollection<MaintenanceTicket> AssignedTickets { get; set; } = new List<MaintenanceTicket>();
}
