using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace DisasterAlleviationApp.Tests;

public class DonationTests
{
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly Mock<IMongoCollection<Donation>> _mockCollection;
    private readonly DonationService _donationService;

    public DonationTests()
    {
        _mockCollection = new Mock<IMongoCollection<Donation>>();
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockDatabase.Setup(db => db.GetCollection<Donation>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
            .Returns(_mockCollection.Object);
        _donationService = new DonationService(_mockDatabase.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllDonations()
    {
        // Arrange
        var donations = new List<Donation>
        {
            new Donation { Id = "1", Type = "Money", Description = "Cash donation", Amount = 1000, Date = DateTime.Now },
            new Donation { Id = "2", Type = "Goods", Description = "Food supplies", Amount = 500, Date = DateTime.Now }
        };

        var asyncCursor = new Mock<IAsyncCursor<Donation>>();
        asyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        asyncCursor.Setup(_ => _.Current).Returns(donations);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Donation>>(),
            It.IsAny<FindOptions<Donation, Donation>>(),
            It.IsAny<CancellationToken>()))
            .Returns(asyncCursor.Object);

        // Act
        var result = await _donationService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task CreateAsync_CreatesNewDonation()
    {
        // Arrange
        var donation = new Donation
        {
            Type = "Money",
            Description = "Test donation",
            Amount = 100,
            Date = DateTime.Now
        };

        _mockCollection.Setup(c => c.InsertOneAsync(
            donation,
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()));

        // Act
        var result = await _donationService.CreateAsync(donation);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(donation.Type, result.Type);
        _mockCollection.Verify(c => c.InsertOneAsync(
            donation,
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDonation_WhenExists()
    {
        // Arrange
        var donation = new Donation { Id = "1", Type = "Money", Amount = 100, Date = DateTime.Now };

        var asyncCursor = new Mock<IAsyncCursor<Donation>>();
        asyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        asyncCursor.Setup(_ => _.Current).Returns(new[] { donation });

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Donation>>(),
            It.IsAny<FindOptions<Donation, Donation>>(),
            It.IsAny<CancellationToken>()))
            .Returns(asyncCursor.Object);

        // Act
        var result = await _donationService.GetByIdAsync("1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesDonation_WhenExists()
    {
        // Arrange
        var donation = new Donation { Id = "1", Type = "Money", Amount = 200, Date = DateTime.Now };
        var replaceResult = new Mock<ReplaceOneResult>();
        replaceResult.Setup(r => r.ModifiedCount).Returns(1);

        _mockCollection.Setup(c => c.ReplaceOneAsync(
            It.IsAny<FilterDefinition<Donation>>(),
            donation,
            It.IsAny<ReplaceOptions>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(replaceResult.Object);

        // Act
        var result = await _donationService.UpdateAsync("1", donation);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_DeletesDonation_WhenExists()
    {
        // Arrange
        var deleteResult = new Mock<DeleteResult>();
        deleteResult.Setup(r => r.DeletedCount).Returns(1);

        _mockCollection.Setup(c => c.DeleteOneAsync(
            It.IsAny<FilterDefinition<Donation>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(deleteResult.Object);

        // Act
        var result = await _donationService.DeleteAsync("1");

        // Assert
        Assert.True(result);
    }
}
