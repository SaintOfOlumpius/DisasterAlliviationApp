using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VolunteersController : ControllerBase
{
    private readonly IVolunteerService _volunteerService;

    public VolunteersController(IVolunteerService volunteerService)
    {
        _volunteerService = volunteerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Volunteer>>> GetAll()
    {
        var volunteers = await _volunteerService.GetAllAsync();
        return Ok(volunteers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Volunteer>> GetById(string id)
    {
        var volunteer = await _volunteerService.GetByIdAsync(id);
        if (volunteer == null)
        {
            return NotFound();
        }
        return Ok(volunteer);
    }

    [HttpPost]
    public async Task<ActionResult<Volunteer>> Create(Volunteer volunteer)
    {
        var createdVolunteer = await _volunteerService.CreateAsync(volunteer);
        return CreatedAtAction(nameof(GetById), new { id = createdVolunteer.Id }, createdVolunteer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Volunteer volunteer)
    {
        volunteer.Id = id;
        var result = await _volunteerService.UpdateAsync(id, volunteer);
        if (!result)
        {
            return NotFound();
        }
        return Ok(volunteer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _volunteerService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
