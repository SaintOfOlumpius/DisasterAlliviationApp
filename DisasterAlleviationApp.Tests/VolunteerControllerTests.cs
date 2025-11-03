using DisasterAlleviationApp.Controllers;
using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DisasterAlleviationApp.Tests;

public class VolunteerControllerTests
{
    private readonly Mock<IVolunteerService> _mockVolunteerService;
    private readonly VolunteersController _volunteersController;

    public VolunteerControllerTests()
    {
        _mockVolunteerService = new Mock<IVolunteerService>();
        _volunteersController = new VolunteersController(_mockVolunteerService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Arrange
        var volunteers = new List<Volunteer>
        {
            new Volunteer { Id = "1", Name = "Alice Johnson", Contact = "alice@example.com" },
            new Volunteer { Id = "2", Name = "Bob Williams", Contact = "bob@example.com" }
        };

        _mockVolunteerService.Setup(s => s.GetAllAsync()).ReturnsAsync(volunteers);

        // Act
        var result = await _volunteersController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedVolunteers = Assert.IsAssignableFrom<List<Volunteer>>(okResult.Value);
        Assert.Equal(2, returnedVolunteers.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        // Arrange
        var volunteer = new Volunteer { Id = "1", Name = "Alice Johnson", Contact = "alice@example.com" };
        _mockVolunteerService.Setup(s => s.GetByIdAsync("1")).ReturnsAsync(volunteer);

        // Act
        var result = await _volunteersController.GetById("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedVolunteer = Assert.IsType<Volunteer>(okResult.Value);
        Assert.Equal("1", returnedVolunteer.Id);
        Assert.Equal("Alice Johnson", returnedVolunteer.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        _mockVolunteerService.Setup(s => s.GetByIdAsync("999")).ReturnsAsync((Volunteer?)null);

        // Act
        var result = await _volunteersController.GetById("999");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        // Arrange
        var newVolunteer = new Volunteer { Name = "Test Volunteer", Contact = "test@example.com" };
        var createdVolunteer = new Volunteer { Id = "1", Name = "Test Volunteer", Contact = "test@example.com" };
        _mockVolunteerService.Setup(s => s.CreateAsync(It.IsAny<Volunteer>())).ReturnsAsync(createdVolunteer);

        // Act
        var result = await _volunteersController.Create(newVolunteer);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedVolunteer = Assert.IsType<Volunteer>(createdAtActionResult.Value);
        Assert.Equal("1", returnedVolunteer.Id);
    }

    [Fact]
    public async Task UpdateVolunteer_ShouldReturnOk()
    {
        // Arrange
        var volunteerId = "1";
        var existingVolunteer = new Volunteer { Id = volunteerId, Name = "Alice Johnson", Contact = "alice@example.com" };
        var updatedVolunteer = new Volunteer { Id = volunteerId, Name = "Alice Smith", Contact = "alice.smith@example.com" };

        // First, add the volunteer with a known ID
        _mockVolunteerService.Setup(s => s.CreateAsync(It.IsAny<Volunteer>()))
            .ReturnsAsync(existingVolunteer);
        
        // Ensure the volunteer exists
        _mockVolunteerService.Setup(s => s.GetByIdAsync(volunteerId)).ReturnsAsync(existingVolunteer);
        
        // Mock UpdateAsync to return true (indicating the update was successful)
        _mockVolunteerService.Setup(s => s.UpdateAsync(volunteerId, It.IsAny<Volunteer>())).ReturnsAsync(true);

        // Add the volunteer first
        await _volunteersController.Create(existingVolunteer);

        // Act
        var result = await _volunteersController.Update(volunteerId, updatedVolunteer);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedVolunteer = Assert.IsType<Volunteer>(okResult.Value);
        Assert.Equal(volunteerId, returnedVolunteer.Id);
        Assert.Equal("Alice Smith", returnedVolunteer.Name);
        _mockVolunteerService.Verify(s => s.UpdateAsync(volunteerId, It.Is<Volunteer>(v => v.Id == volunteerId)), Times.Once);
    }

    [Fact]
    public async Task DeleteVolunteer_ShouldReturnNoContent()
    {
        // Arrange
        var volunteerId = "1";
        var existingVolunteer = new Volunteer { Id = volunteerId, Name = "Alice Johnson", Contact = "alice@example.com" };

        // First, add the volunteer with a known ID
        _mockVolunteerService.Setup(s => s.CreateAsync(It.IsAny<Volunteer>()))
            .ReturnsAsync(existingVolunteer);
        
        // Ensure the volunteer exists
        _mockVolunteerService.Setup(s => s.GetByIdAsync(volunteerId)).ReturnsAsync(existingVolunteer);
        
        // Mock DeleteAsync to return true (indicating the deletion was successful)
        _mockVolunteerService.Setup(s => s.DeleteAsync(volunteerId)).ReturnsAsync(true);

        // Add the volunteer first
        await _volunteersController.Create(existingVolunteer);

        // Act
        var result = await _volunteersController.Delete(volunteerId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockVolunteerService.Verify(s => s.DeleteAsync(volunteerId), Times.Once);
    }
}

