using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[Route("api/recurring-schedules")]
[Authorize(Roles = "ClubManager,SuperAdmin")]
public class RecurringSchedulesController : BaseApiController
{
    private readonly ISessionService _sessionService;

    public RecurringSchedulesController(ISessionService sessionService, ITenantService tenantService)
        : base(tenantService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecurringScheduleDto>>> GetAll()
    {
        var clubId = GetClubId();
        var schedules = await _sessionService.GetRecurringSchedulesAsync(clubId);
        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecurringScheduleDto>> GetById(Guid id)
    {
        var clubId = GetClubId();
        var schedule = await _sessionService.GetRecurringScheduleByIdAsync(clubId, id);
        if (schedule == null)
            return NotFound();
        return Ok(schedule);
    }

    [HttpPost]
    public async Task<ActionResult<RecurringScheduleDto>> Create([FromBody] RecurringScheduleCreateRequest request)
    {
        var clubId = GetClubId();
        var schedule = await _sessionService.CreateRecurringScheduleAsync(clubId, request);
        return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RecurringScheduleDto>> Update(Guid id, [FromBody] RecurringScheduleUpdateRequest request)
    {
        var clubId = GetClubId();
        var schedule = await _sessionService.UpdateRecurringScheduleAsync(clubId, id, request);
        if (schedule == null)
            return NotFound();
        return Ok(schedule);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var clubId = GetClubId();
        var result = await _sessionService.DeleteRecurringScheduleAsync(clubId, id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/generate")]
    public async Task<ActionResult> GenerateSessions(Guid id, [FromBody] GenerateSessionsRequest request)
    {
        var clubId = GetClubId();
        var count = await _sessionService.GenerateSessionsAsync(clubId, id, request);
        return Ok(new { GeneratedCount = count });
    }
}
