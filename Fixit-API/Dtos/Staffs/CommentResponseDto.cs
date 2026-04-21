namespace Fixit_API.Dtos.Staffs
{
    public class CommentResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
