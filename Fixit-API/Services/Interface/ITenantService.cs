using Fixit_API.Dtos.Tenants;
using Fixit_API.Dtos.Tenents;

namespace Fixit_API.Interfaces;

public interface ITenantService
{
    Task<TenantDashboardDTO> GetTenantDashboard(Guid tenantUserId);

    Task<List<TenantTicketDTO>> GetTenantTickets(Guid tenantUserId);

    Task<TenantTicketDTO> CreateTicket(
        Guid tenantUserId,
        CreateTenantTicketDTO dto);
}