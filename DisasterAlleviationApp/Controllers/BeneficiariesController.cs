using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeneficiariesController : ControllerBase
{
    private readonly IBeneficiaryService _beneficiaryService;

    public BeneficiariesController(IBeneficiaryService beneficiaryService)
    {
        _beneficiaryService = beneficiaryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Beneficiary>>> GetAll()
    {
        var beneficiaries = await _beneficiaryService.GetAllAsync();
        return Ok(beneficiaries);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Beneficiary>> GetById(string id)
    {
        var beneficiary = await _beneficiaryService.GetByIdAsync(id);
        if (beneficiary == null)
        {
            return NotFound();
        }
        return Ok(beneficiary);
    }

    [HttpPost]
    public async Task<ActionResult<Beneficiary>> Create(Beneficiary beneficiary)
    {
        var createdBeneficiary = await _beneficiaryService.CreateAsync(beneficiary);
        return CreatedAtAction(nameof(GetById), new { id = createdBeneficiary.Id }, createdBeneficiary);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Beneficiary beneficiary)
    {
        beneficiary.Id = id;
        var result = await _beneficiaryService.UpdateAsync(id, beneficiary);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _beneficiaryService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
