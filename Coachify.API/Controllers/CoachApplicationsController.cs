using Coachify.BLL.DTOs.CoachApplication;
using Coachify.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coachify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoachApplicationsController : ControllerBase
{
    private readonly ICoachApplicationService _service;

    public CoachApplicationsController(ICoachApplicationService service) 
        => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() 
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var d = await _service.GetByIdAsync(id);
        return d == null ? NotFound() : Ok(d);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCoachApplicationDto dto)
    {
        var c = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = c.ApplicationId }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCoachApplicationDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    // Метод для одобрения заявки
    [HttpPost("approve/{applicationId}")]
    public async Task<IActionResult> ApproveCoachApplication(int applicationId)
    {
        try
        {
            await _service.ApproveCoachApplicationAsync(applicationId);
            return Ok(new { message = "Application approved successfully" });
        }
        catch (Exception ex)
        {
            // Возвращаем ошибку с сообщением
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingApplications()
    {
        var pendingApplications = await _service.GetPendingApplicationsAsync();
        return Ok(pendingApplications);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) 
        => Ok(await _service.DeleteAsync(id));
}