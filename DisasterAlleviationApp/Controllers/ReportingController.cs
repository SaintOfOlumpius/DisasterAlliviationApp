using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportingController : ControllerBase
{
    private readonly IDonationService _donationService;
    private readonly IBeneficiaryService _beneficiaryService;
    private readonly IVolunteerService _volunteerService;
    private readonly IDisasterService _disasterService;

    public ReportingController(
        IDonationService donationService,
        IBeneficiaryService beneficiaryService,
        IVolunteerService volunteerService,
        IDisasterService disasterService)
    {
        _donationService = donationService;
        _beneficiaryService = beneficiaryService;
        _volunteerService = volunteerService;
        _disasterService = disasterService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetSummary()
    {
        var donations = await _donationService.GetAllAsync();
        var beneficiaries = await _beneficiaryService.GetAllAsync();
        var volunteers = await _volunteerService.GetAllAsync();
        var disasters = await _disasterService.GetAllAsync();

        var donationsByType = donations
            .GroupBy(d => d.Type)
            .ToDictionary(g => g.Key, g => new
            {
                Count = g.Count(),
                TotalAmount = g.Sum(d => d.Amount)
            });

        var summary = new
        {
            TotalDonations = donations.Count,
            TotalBeneficiaries = beneficiaries.Count,
            TotalVolunteers = volunteers.Count,
            TotalDisasters = disasters.Count,
            DonationsByType = donationsByType,
            TotalDonationAmount = donations.Sum(d => d.Amount)
        };

        return Ok(summary);
    }
}
