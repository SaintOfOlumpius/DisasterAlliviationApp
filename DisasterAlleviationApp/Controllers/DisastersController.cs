using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DisastersController : ControllerBase
{
    private readonly IDisasterService _disasterService;

    public DisastersController(IDisasterService disasterService)
    {
        _disasterService = disasterService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Disaster>>> GetAll()
    {
        var disasters = await _disasterService.GetAllAsync();
        return Ok(disasters);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Disaster>> GetById(string id)
    {
        var disaster = await _disasterService.GetByIdAsync(id);
        if (disaster == null)
        {
            return NotFound();
        }
        return Ok(disaster);
    }

    [HttpPost]
    public async Task<ActionResult<Disaster>> Create(Disaster disaster)
    {
        var createdDisaster = await _disasterService.CreateAsync(disaster);
        return CreatedAtAction(nameof(GetById), new { id = createdDisaster.Id }, createdDisaster);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Disaster disaster)
    {
        disaster.Id = id;
        var result = await _disasterService.UpdateAsync(id, disaster);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _disasterService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
