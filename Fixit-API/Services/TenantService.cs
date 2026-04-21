using Fixit_API.Data;
using Fixit_API.Dtos.Tenants;
using Fixit_API.Dtos.Tenents;
using Fixit_API.Interfaces;
using Fixit_API.Models;
using Fixit_API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fixit_API.Services;

public class TenantService : ITenantService
{
    private readonly AppDbContext _context;

    public TenantService(AppDbContext context)
    {
        _context = context;
    }

    // Get Dashboard Info
    public async Task<TenantDashboardDTO> GetTenantDashboard(Guid tenantUserId)
    {
        var tenant = await _context.TenantProfiles
            .Include(t => t.Tickets)
            .FirstOrDefaultAsync(t => t.UserId == tenantUserId);

        if (tenant == null)
            throw new Exception("Tenant not found");

        return new TenantDashboardDTO
        {
            ApartmentNumber = tenant.ApartmentNumber,
            UnitNumber = tenant.UnitNumber,
            TotalTickets = tenant.Tickets.Count,
            OpenTickets = tenant.Tickets
                .Count(t => t.Status != TicketStatus.Resolved),
            ResolvedTickets = tenant.Tickets
                .Count(t => t.Status == TicketStatus.Resolved)
        };
    }

    // Get All Tenant Tickets
    public async Task<List<TenantTicketDTO>> GetTenantTickets(Guid tenantUserId)
    {
        var tickets = await _context.MaintenanceTickets
            .Where(t => t.Tenant!.UserId == tenantUserId)
            .Select(t => new TenantTicketDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                RoomLocation = t.RoomLocation,
                ImageUrl = t.ImageUrl,
                Category = t.Category,
                Priority = t.Priority,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

        return tickets;
    }

    // Create Ticket
    public async Task<TenantTicketDTO> CreateTicket(
        Guid tenantUserId,
        CreateTenantTicketDTO dto)
    {
        var tenant = await _context.TenantProfiles
            .FirstOrDefaultAsync(t => t.UserId == tenantUserId);

        if (tenant == null)
            throw new Exception("Tenant not found");

        var ticket = new MaintenanceTicket
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Title = dto.Title,
            Description = dto.Description,
            RoomLocation = dto.RoomLocation,
            ImageUrl = dto.ImageUrl,
            Category = dto.Category,
            Priority = dto.Priority,
            Status = TicketStatus.Submitted,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.MaintenanceTickets.Add(ticket);

        await _context.SaveChangesAsync();

        return new TenantTicketDTO
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            RoomLocation = ticket.RoomLocation,
            ImageUrl = ticket.ImageUrl,
            Category = ticket.Category,
            Priority = ticket.Priority,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt
        };
    }
}