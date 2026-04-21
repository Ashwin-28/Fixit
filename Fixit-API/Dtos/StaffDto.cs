namespace Fixit_API.Dtos
{
    public class StaffDto
    {
        public Guid EmployeeProfileId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public int ActiveAssignedTickets { get; set; }
    }
}
