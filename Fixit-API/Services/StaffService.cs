using Fixit_API.Data;
using Fixit_API.Dtos.Staffs;

using Fixit_API.Models;
using Fixit_API.Models.Enums;
using Fixit_API.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Fixit_API.Services;

public class StaffService : IStaffService
{
    private readonly AppDbContext _context;

    public StaffService(AppDbContext context)
    {
        _context = context;
    }

    // ✅ GET MY ASSIGNED TICKETS
    public async Task<List<StaffTicketResponseDto>> GetMyAssignedTickets(Guid userId)
    {
        var employee = await _context.EmployeeProfiles
            .FirstOrDefaultAsync(e => e.UserId == userId);

        if (employee == null)
            return new List<StaffTicketResponseDto>();

        var tickets = await _context.MaintenanceTickets
            .Where(t => t.AssignedEmployeeId == employee.Id)
            .Include(t => t.Tenant)
                .ThenInclude(t => t!.User)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tickets.Select(t => new StaffTicketResponseDto
        {
            TicketId = t.Id,
            Title = t.Title,
            Description = t.Description,
            RoomLocation = t.RoomLocation,
            ImageUrl = t.ImageUrl,
            Category = t.Category,
            Priority = t.Priority,
            Status = t.Status,
            CreatedAt = t.CreatedAt,
            ResolvedAt = t.ResolvedAt,
            TenantName = t.Tenant!.User!.FullName,
            ApartmentNumber = t.Tenant.ApartmentNumber
        }).ToList();
    }

    // ✅ UPDATE STATUS
    public async Task<bool> UpdateTicketStatus(Guid userId, UpdateTicketStatusDto dto)
    {
        var ticket = await _context.MaintenanceTickets
            .FirstOrDefaultAsync(t => t.Id == dto.TicketId);

        if (ticket == null)
            return false;

        var oldStatus = ticket.Status;

        if (!IsValidTransition(oldStatus, dto.NewStatus))
            return false;

        ticket.Status = dto.NewStatus;
        ticket.UpdatedAt = DateTime.UtcNow;

        if (dto.NewStatus == TicketStatus.Resolved)
            ticket.ResolvedAt = DateTime.UtcNow;

        // 🔥 HISTORY
        var history = new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            OldStatus = oldStatus,
            NewStatus = dto.NewStatus,
            ChangedByUserId = userId,
            ChangedAt = DateTime.UtcNow,
            Notes = dto.Notes
        };

        await _context.TicketStatusHistories.AddAsync(history);

        // 🔔 NOTIFICATION
        var tenant = await _context.TenantProfiles
            .FirstOrDefaultAsync(t => t.Id == ticket.TenantId);

        if (tenant != null)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = tenant.UserId,
                TicketId = ticket.Id,
                Message = $"Your ticket '{ticket.Title}' updated to {dto.NewStatus}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    // ✅ ADD COMMENT
    public async Task<bool> AddComment(Guid userId, AddCommentDto dto)
    {
        var exists = await _context.MaintenanceTickets
            .AnyAsync(t => t.Id == dto.TicketId);

        if (!exists)
            return false;

        var comment = new TicketComment
        {
            Id = Guid.NewGuid(),
            TicketId = dto.TicketId,
            AuthorUserId = userId,
            Message = dto.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _context.TicketComments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return true;
    }

    // 🔥 STATUS RULES
    private bool IsValidTransition(TicketStatus oldStatus, TicketStatus newStatus)
    {
        return oldStatus switch
        {
            TicketStatus.Assigned => newStatus == TicketStatus.InProgress,
            TicketStatus.InProgress => newStatus == TicketStatus.Resolved,
            TicketStatus.Resolved => newStatus == TicketStatus.Closed,
            _ => false
        };
    }
}
