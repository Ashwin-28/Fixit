using Fixit_API.Data;
using Fixit_API.Dtos;
using Fixit_API.Models;
using Fixit_API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fixit_API.Services
{
    public class MangerServices
    {
        private readonly AppDbContext _context;

        public MangerServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TicketDto>> GetallTickets()
        {
            return await _context.MaintenanceTickets
                .Select(ticket => new TicketDto
                {
                    Id = ticket.Id,
                    TenantId = ticket.TenantId,
                    AssignedEmployeeId = ticket.AssignedEmployeeId,
                    Category = ticket.Category,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    RoomLocation = ticket.RoomLocation,
                    ImageUrl = ticket.ImageUrl,
                    Priority = ticket.Priority,
                    Status = ticket.Status,
                    CreatedAt = ticket.CreatedAt,
                    UpdatedAt = ticket.UpdatedAt,
                    ResolvedAt = ticket.ResolvedAt
                })
                .ToListAsync();
        }

        public async Task<TicketDto?> GetTicketById(Guid id)
        {
            var ticket = await _context.MaintenanceTickets
                .Where(t => t.Id == id)
                .Select(ticket => new TicketDto
                {
                    Id = ticket.Id,
                    TenantId = ticket.TenantId,
                    AssignedEmployeeId = ticket.AssignedEmployeeId,
                    Category = ticket.Category,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    RoomLocation = ticket.RoomLocation,
                    ImageUrl = ticket.ImageUrl,
                    Priority = ticket.Priority,
                    Status = ticket.Status,
                    CreatedAt = ticket.CreatedAt,
                    UpdatedAt = ticket.UpdatedAt,
                    ResolvedAt = ticket.ResolvedAt
                })
                .FirstOrDefaultAsync();

            return ticket;
        }

        public async Task<bool> UpdateTicketStatus(Guid ticketId, TicketStatus newStatus, Guid changedByUserId, string notes = "")
        {
            var ticket = await _context.MaintenanceTickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return false;
            }

            var oldStatus = ticket.Status;
            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            if (newStatus == TicketStatus.Resolved)
            {
                ticket.ResolvedAt = DateTime.UtcNow;
            }

            _context.TicketStatusHistories.Add(new TicketStatusHistory
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ChangedByUserId = changedByUserId,
                ChangedAt = DateTime.UtcNow,
                Notes = notes
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignTicket(Guid ticketId, Guid employeeProfileId)
        {
            var ticket = await _context.MaintenanceTickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return false;
            }

            var employeeExists = await _context.EmployeeProfiles.AnyAsync(e => e.Id == employeeProfileId);
            if (!employeeExists)
            {
                return false;
            }

            ticket.AssignedEmployeeId = employeeProfileId;
            ticket.Status = TicketStatus.Assigned;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TicketDto>> FilterTickets(
            TicketStatus? status = null,
            TicketPriority? priority = null,
            TicketCategory? category = null,
            Guid? assignedEmployeeId = null)
        {
            var query = _context.MaintenanceTickets.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            if (category.HasValue)
            {
                query = query.Where(t => t.Category == category.Value);
            }

            if (assignedEmployeeId.HasValue)
            {
                query = query.Where(t => t.AssignedEmployeeId == assignedEmployeeId.Value);
            }

            return await query
                .Select(ticket => new TicketDto
                {
                    Id = ticket.Id,
                    TenantId = ticket.TenantId,
                    AssignedEmployeeId = ticket.AssignedEmployeeId,
                    Category = ticket.Category,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    RoomLocation = ticket.RoomLocation,
                    ImageUrl = ticket.ImageUrl,
                    Priority = ticket.Priority,
                    Status = ticket.Status,
                    CreatedAt = ticket.CreatedAt,
                    UpdatedAt = ticket.UpdatedAt,
                    ResolvedAt = ticket.ResolvedAt
                })
                .ToListAsync();
        }

        public async Task<List<MonthlyResolutionDto>> GetMonthlyResolutionReport(int? year = null)
        {
            var reportYear = year ?? DateTime.UtcNow.Year;

            return await _context.MaintenanceTickets
                .Where(t => t.ResolvedAt.HasValue && t.ResolvedAt.Value.Year == reportYear)
                .GroupBy(t => t.ResolvedAt!.Value.Month)
                .Select(group => new MonthlyResolutionDto
                {
                    Year = reportYear,
                    Month = group.Key,
                    ResolvedCount = group.Count()
                })
                .OrderBy(item => item.Month)
                .ToListAsync();
        }

        public async Task<List<StaffDto>> GetStaff()
        {
            return await _context.EmployeeProfiles
                .Select(employee => new StaffDto
                {
                    EmployeeProfileId = employee.Id,
                    UserId = employee.UserId,
                    FullName = employee.User != null ? employee.User.FullName : string.Empty,
                    Email = employee.User != null ? employee.User.Email ?? string.Empty : string.Empty,
                    JobTitle = employee.JobTitle,
                    IsAvailable = employee.IsAvailable,
                    ActiveAssignedTickets = employee.AssignedTickets.Count(t => t.Status != TicketStatus.Resolved && t.Status != TicketStatus.Closed)
                })
                .ToListAsync();
        }
    }
}