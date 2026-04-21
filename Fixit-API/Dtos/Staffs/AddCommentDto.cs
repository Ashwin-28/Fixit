namespace Fixit_API.Dtos.Staffs
{
    public class AddCommentDto
    {
        public Guid TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
