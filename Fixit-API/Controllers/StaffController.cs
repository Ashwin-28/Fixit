using Fixit_API.Dtos.Staffs;
using Fixit_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fixit_API.Controllers;

[Route("api/staff")]
[ApiController]
[Authorize(Roles = "Staff")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    // ✅ GET MY TICKETS
    [HttpGet("my-tickets")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = GetUserId();

        var result = await _staffService.GetMyAssignedTickets(userId);

        return Ok(result);
    }

    // ✅ UPDATE STATUS
    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateTicketStatusDto dto)
    {
        var userId = GetUserId();

        var result = await _staffService.UpdateTicketStatus(userId, dto);

        if (!result)
            return BadRequest("Invalid update");

        return Ok("Status updated successfully");
    }

    // ✅ ADD COMMENT
    [HttpPost("add-comment")]
    public async Task<IActionResult> AddComment([FromBody] AddCommentDto dto)
    {
        var userId = GetUserId();

        var result = await _staffService.AddComment(userId, dto);

        if (!result)
            return BadRequest("Ticket not found");

        return Ok("Comment added");
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}