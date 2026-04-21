using Fixit_API.Models.Enums;

namespace Fixit_API.Dtos.Tenents
{
    
    public class CreateTenantTicketDTO
    {
        public TicketCategory Category { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string RoomLocation { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public TicketPriority Priority { get; set; }
    }
}

