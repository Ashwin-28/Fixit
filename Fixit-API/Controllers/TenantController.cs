using Fixit_API.Dtos.Tenents;
using Fixit_API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fixit_API.Controllers;

[Route("api/tenant")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantController(
        ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    // Get Dashboard
    [HttpGet("dashboard/{userId}")]
    public async Task<IActionResult> GetDashboard(
        Guid userId)
    {
        var result =
            await _tenantService
            .GetTenantDashboard(userId);

        return Ok(result);
    }

    // Get Tickets
    [HttpGet("tickets/{userId}")]
    public async Task<IActionResult> GetTickets(
        Guid userId)
    {
        var result =
            await _tenantService
            .GetTenantTickets(userId);

        return Ok(result);
    }

    // Create Ticket
    [HttpPost("create-ticket/{userId}")]
    public async Task<IActionResult> CreateTicket(
        Guid userId,
        CreateTenantTicketDTO dto)
    {
        var result =
            await _tenantService
            .CreateTicket(userId, dto);

        return Ok(result);
    }
}