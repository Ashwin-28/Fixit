using Fixit_API.Dtos.Staffs;

namespace Fixit_API.Services.Interface
{
    public interface IStaffService
    {
        Task<List<StaffTicketResponseDto>> GetMyAssignedTickets(Guid userId);
        Task<bool> UpdateTicketStatus(Guid userId, UpdateTicketStatusDto dto);
        Task<bool> AddComment(Guid userId, AddCommentDto dto);
    }
}

