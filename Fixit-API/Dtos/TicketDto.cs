using Fixit_API.Models.Enums;

namespace Fixit_API.Dtos
{
    public class TicketDto
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
    }
}
