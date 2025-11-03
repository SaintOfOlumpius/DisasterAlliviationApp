using DisasterAlleviationApp.Controllers;
using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DisasterAlleviationApp.Tests;

public class DonationControllerTests
{
    private readonly Mock<IDonationService> _mockDonationService;
    private readonly DonationsController _donationsController;

    public DonationControllerTests()
    {
        _mockDonationService = new Mock<IDonationService>();
        _donationsController = new DonationsController(_mockDonationService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Arrange
        var donations = new List<Donation>
        {
            new Donation { Id = "1", Type = "Money", Description = "Cash donation", Amount = 1000, Date = DateTime.Now },
            new Donation { Id = "2", Type = "Goods", Description = "Food supplies", Amount = 500, Date = DateTime.Now }
        };

        _mockDonationService.Setup(s => s.GetAllAsync()).ReturnsAsync(donations);

        // Act
        var result = await _donationsController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDonations = Assert.IsAssignableFrom<List<Donation>>(okResult.Value);
        Assert.Equal(2, returnedDonations.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        // Arrange
        var donation = new Donation { Id = "1", Type = "Money", Description = "Cash donation", Amount = 1000, Date = DateTime.Now };
        _mockDonationService.Setup(s => s.GetByIdAsync("1")).ReturnsAsync(donation);

        // Act
        var result = await _donationsController.GetById("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDonation = Assert.IsType<Donation>(okResult.Value);
        Assert.Equal("1", returnedDonation.Id);
        Assert.Equal("Money", returnedDonation.Type);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        _mockDonationService.Setup(s => s.GetByIdAsync("999")).ReturnsAsync((Donation?)null);

        // Act
        var result = await _donationsController.GetById("999");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        // Arrange
        var newDonation = new Donation { Type = "Money", Description = "Test donation", Amount = 100, Date = DateTime.Now };
        var createdDonation = new Donation { Id = "1", Type = "Money", Description = "Test donation", Amount = 100, Date = DateTime.Now };
        _mockDonationService.Setup(s => s.CreateAsync(It.IsAny<Donation>())).ReturnsAsync(createdDonation);

        // Act
        var result = await _donationsController.Create(newDonation);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedDonation = Assert.IsType<Donation>(createdAtActionResult.Value);
        Assert.Equal("1", returnedDonation.Id);
    }

    [Fact]
    public async Task UpdateDonation_ShouldReturnOk()
    {
        // Arrange
        var donationId = "1";
        var existingDonation = new Donation 
        { 
            Id = donationId, 
            Type = "Money", 
            Description = "Cash donation", 
            Amount = 1000, 
            Date = DateTime.Now 
        };
        var updatedDonation = new Donation 
        { 
            Id = donationId, 
            Type = "Money", 
            Description = "Updated cash donation", 
            Amount = 2000, 
            Date = DateTime.Now 
        };

        // First, add the donation with a known ID
        _mockDonationService.Setup(s => s.CreateAsync(It.IsAny<Donation>()))
            .ReturnsAsync(existingDonation);
        
        // Ensure the donation exists
        _mockDonationService.Setup(s => s.GetByIdAsync(donationId)).ReturnsAsync(existingDonation);
        
        // Mock UpdateAsync to return true (indicating the update was successful)
        _mockDonationService.Setup(s => s.UpdateAsync(donationId, It.IsAny<Donation>())).ReturnsAsync(true);

        // Add the donation first
        await _donationsController.Create(existingDonation);

        // Act
        var result = await _donationsController.Update(donationId, updatedDonation);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDonation = Assert.IsType<Donation>(okResult.Value);
        Assert.Equal(donationId, returnedDonation.Id);
        Assert.Equal("Updated cash donation", returnedDonation.Description);
        Assert.Equal(2000, returnedDonation.Amount);
        _mockDonationService.Verify(s => s.UpdateAsync(donationId, It.Is<Donation>(d => d.Id == donationId)), Times.Once);
    }

    [Fact]
    public async Task DeleteDonation_ShouldReturnNoContent()
    {
        // Arrange
        var donationId = "1";
        var existingDonation = new Donation 
        { 
            Id = donationId, 
            Type = "Money", 
            Description = "Cash donation", 
            Amount = 1000, 
            Date = DateTime.Now 
        };

        // First, add the donation with a known ID
        _mockDonationService.Setup(s => s.CreateAsync(It.IsAny<Donation>()))
            .ReturnsAsync(existingDonation);
        
        // Ensure the donation exists
        _mockDonationService.Setup(s => s.GetByIdAsync(donationId)).ReturnsAsync(existingDonation);
        
        // Mock DeleteAsync to return true (indicating the deletion was successful)
        _mockDonationService.Setup(s => s.DeleteAsync(donationId)).ReturnsAsync(true);

        // Add the donation first
        await _donationsController.Create(existingDonation);

        // Act
        var result = await _donationsController.Delete(donationId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockDonationService.Verify(s => s.DeleteAsync(donationId), Times.Once);
    }
}

