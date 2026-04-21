using Fixit_API.Models.Enums;

namespace Fixit_API.Dtos.Staffs
{
    public class StaffTicketResponseDto
    {
        public Guid TicketId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RoomLocation { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public TicketCategory Category { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }

        public string TenantName { get; set; } = string.Empty;
        public string ApartmentNumber { get; set; } = string.Empty;
    }
}
