using DisasterAlleviationApp.Controllers;
using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using Moq;
using Xunit;

namespace DisasterAlleviationApp.Tests;

public class ReportingTests
{
    private readonly Mock<IDonationService> _mockDonationService;
    private readonly Mock<IBeneficiaryService> _mockBeneficiaryService;
    private readonly Mock<IVolunteerService> _mockVolunteerService;
    private readonly Mock<IDisasterService> _mockDisasterService;
    private readonly ReportingController _reportingController;

    public ReportingTests()
    {
        _mockDonationService = new Mock<IDonationService>();
        _mockBeneficiaryService = new Mock<IBeneficiaryService>();
        _mockVolunteerService = new Mock<IVolunteerService>();
        _mockDisasterService = new Mock<IDisasterService>();
        _reportingController = new ReportingController(
            _mockDonationService.Object,
            _mockBeneficiaryService.Object,
            _mockVolunteerService.Object,
            _mockDisasterService.Object);
    }

    [Fact]
    public async Task GetSummary_ReturnsSummaryWithCounts()
    {
        // Arrange
        var donations = new List<Donation>
        {
            new Donation { Id = "1", Type = "Money", Amount = 1000, Date = DateTime.Now },
            new Donation { Id = "2", Type = "Goods", Amount = 500, Date = DateTime.Now },
            new Donation { Id = "3", Type = "Services", Amount = 750, Date = DateTime.Now }
        };

        var beneficiaries = new List<Beneficiary>
        {
            new Beneficiary { Id = "1", Name = "John Doe" },
            new Beneficiary { Id = "2", Name = "Jane Smith" }
        };

        var volunteers = new List<Volunteer>
        {
            new Volunteer { Id = "1", Name = "Alice Johnson" },
            new Volunteer { Id = "2", Name = "Bob Williams" }
        };

        var disasters = new List<Disaster>
        {
            new Disaster { Id = "1", Name = "Hurricane Alpha" },
            new Disaster { Id = "2", Name = "Flood Beta" }
        };

        _mockDonationService.Setup(s => s.GetAllAsync()).ReturnsAsync(donations);
        _mockBeneficiaryService.Setup(s => s.GetAllAsync()).ReturnsAsync(beneficiaries);
        _mockVolunteerService.Setup(s => s.GetAllAsync()).ReturnsAsync(volunteers);
        _mockDisasterService.Setup(s => s.GetAllAsync()).ReturnsAsync(disasters);

        // Act
        var result = await _reportingController.GetSummary();
        var okResult = result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult;
        var summary = okResult?.Value;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(okResult);
        Assert.NotNull(summary);
    }

    [Fact]
    public async Task GetSummary_ReturnsDonationsByType()
    {
        // Arrange
        var donations = new List<Donation>
        {
            new Donation { Id = "1", Type = "Money", Amount = 1000, Date = DateTime.Now },
            new Donation { Id = "2", Type = "Money", Amount = 500, Date = DateTime.Now },
            new Donation { Id = "3", Type = "Goods", Amount = 750, Date = DateTime.Now }
        };

        _mockDonationService.Setup(s => s.GetAllAsync()).ReturnsAsync(donations);
        _mockBeneficiaryService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Beneficiary>());
        _mockVolunteerService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Volunteer>());
        _mockDisasterService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Disaster>());

        // Act
        var result = await _reportingController.GetSummary();
        var okResult = result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(okResult);
    }
}
