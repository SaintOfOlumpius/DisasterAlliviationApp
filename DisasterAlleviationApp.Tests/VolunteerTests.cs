using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace DisasterAlleviationApp.Tests;

public class VolunteerTests
{
    private readonly Mock<IMongoCollection<Volunteer>> _mockCollection;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly VolunteerService _volunteerService;

    public VolunteerTests()
    {
        _mockCollection = new Mock<IMongoCollection<Volunteer>>();
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockDatabase.Setup(db => db.GetCollection<Volunteer>("Volunteers", It.IsAny<MongoCollectionSettings>()))
            .Returns(_mockCollection.Object);
        _volunteerService = new VolunteerService(_mockDatabase.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllVolunteers()
    {
        // Arrange
        var volunteers = new List<Volunteer>
        {
            new Volunteer { Id = "1", Name = "Alice Johnson", Contact = "alice@example.com" },
            new Volunteer { Id = "2", Name = "Bob Williams", Contact = "bob@example.com" }
        };

        var asyncCursor = new Mock<IAsyncCursor<Volunteer>>();
        asyncCursor.Setup(_ => _.Current).Returns(volunteers);
        asyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Volunteer>>(),
            It.IsAny<FindOptions<Volunteer, Volunteer>>(),
            It.IsAny<CancellationToken>()))
            .Returns(new Mock<IAsyncCursor<Volunteer>>().Object);

        // Act
        var result = await _volunteerService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_CreatesNewVolunteer()
    {
        // Arrange
        var volunteer = new Volunteer
        {
            Name = "Test Volunteer",
            Contact = "volunteer@example.com"
        };

        _mockCollection.Setup(c => c.InsertOneAsync(
            volunteer,
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()));

        // Act
        var result = await _volunteerService.CreateAsync(volunteer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(volunteer.Name, result.Name);
        _mockCollection.Verify(c => c.InsertOneAsync(
            volunteer,
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsVolunteer_WhenExists()
    {
        // Arrange
        var volunteer = new Volunteer { Id = "1", Name = "Alice Johnson", Contact = "alice@example.com" };

        var asyncCursor = new Mock<IAsyncCursor<Volunteer>>();
        asyncCursor.Setup(_ => _.Current).Returns(new[] { volunteer });
        asyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Volunteer>>(),
            It.IsAny<FindOptions<Volunteer, Volunteer>>(),
            It.IsAny<CancellationToken>()))
            .Returns(asyncCursor.Object);

        // Act
        var result = await _volunteerService.GetByIdAsync("1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
    }
}
