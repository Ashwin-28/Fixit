using Fixit_API.Models.Enums;

namespace Fixit_API.Dtos.Staffs
{
    public class UpdateTicketStatusDto
    {
        public Guid TicketId { get; set; }
        public TicketStatus NewStatus { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
