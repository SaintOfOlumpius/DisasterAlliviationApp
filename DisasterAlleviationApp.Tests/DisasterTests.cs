using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace DisasterAlleviationApp.Tests;

public class DisasterTests
{
    private readonly Mock<IMongoCollection<Disaster>> _mockCollection;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly DisasterService _disasterService;

    public DisasterTests()
    {
        _mockCollection = new Mock<IMongoCollection<Disaster>>();
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockDatabase.Setup(db => db.GetCollection<Disaster>("Disasters", It.IsAny<MongoCollectionSettings>()))
            .Returns(_mockCollection.Object);
        _disasterService = new DisasterService(_mockDatabase.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllDisasters()
    {
        // Arrange
        var disasters = new List<Disaster>
        {
            new Disaster { Id = "1", Name = "Hurricane Alpha", Type = "Hurricane", DateOccurred = DateTime.Now, Location = "Coastal Area" },
            new Disaster { Id = "2", Name = "Flood Beta", Type = "Flood", DateOccurred = DateTime.Now, Location = "River Valley" }
        };

        var asyncCursor = new Mock<IAsyncCursor<Disaster>>();
        asyncCursor.Setup(_ => _.Current).Returns(disasters);
        asyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Disaster>>(),
            It.IsAny<FindOptions<Disaster, Disaster>>(),
            It.IsAny<CancellationToken>()))
            .Returns(new Mock<IAsyncCursor<Disaster>>().Object);

        // Act
        var result = await _disasterService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_CreatesNewDisaster()
    {
        // Arrange
        var disaster = new Disaster
        {
            Name = "Test Disaster",
            Type = "Earthquake",
            DateOccurred = DateTime.Now,
            Location = "Test Location"
        };

        _mockCollection.Setup(c => c.InsertOneAsync(
            disaster,
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()));

        // Act
        var result = await _disasterService.CreateAsync(disaster);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(disaster.Name, result.Name);
        _mockCollection.Verify(c => c.InsertOneAsync(
            disaster,
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDisaster_WhenExists()
    {
        // Arrange
        var disaster = new Disaster { Id = "1", Name = "Hurricane Alpha", Type = "Hurricane", DateOccurred = DateTime.Now, Location = "Coastal Area" };

        var asyncCursor = new Mock<IAsyncCursor<Disaster>>();
        asyncCursor.Setup(_ => _.Current).Returns(new[] { disaster });
        asyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Disaster>>(),
            It.IsAny<FindOptions<Disaster, Disaster>>(),
            It.IsAny<CancellationToken>()))
            .Returns(asyncCursor.Object);

        // Act
        var result = await _disasterService.GetByIdAsync("1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
    }
}
