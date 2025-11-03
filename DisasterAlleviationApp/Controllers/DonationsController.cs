using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonationsController : ControllerBase
{
    private readonly IDonationService _donationService;

    public DonationsController(IDonationService donationService)
    {
        _donationService = donationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Donation>>> GetAll()
    {
        var donations = await _donationService.GetAllAsync();
        return Ok(donations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Donation>> GetById(string id)
    {
        var donation = await _donationService.GetByIdAsync(id);
        if (donation == null)
        {
            return NotFound();
        }
        return Ok(donation);
    }

    [HttpPost]
    public async Task<ActionResult<Donation>> Create(Donation donation)
    {
        var createdDonation = await _donationService.CreateAsync(donation);
        return CreatedAtAction(nameof(GetById), new { id = createdDonation.Id }, createdDonation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Donation donation)
    {
        donation.Id = id;
        var result = await _donationService.UpdateAsync(id, donation);
        if (!result)
        {
            return NotFound();
        }
        return Ok(donation);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _donationService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
