using DisasterAlleviationApp.Models;
using DisasterAlleviationApp.Services;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace DisasterAlleviationApp.Tests;

public class BeneficiaryTests
{
    private readonly Mock<IMongoCollection<Beneficiary>> _mockCollection;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly BeneficiaryService _beneficiaryService;

    public BeneficiaryTests()
    {
        _mockCollection = new Mock<IMongoCollection<Beneficiary>>();
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockDatabase.Setup(db => db.GetCollection<Beneficiary>("Beneficiaries", It.IsAny<MongoCollectionSettings>()))
            .Returns(_mockCollection.Object);
        _beneficiaryService = new BeneficiaryService(_mockDatabase.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBeneficiaries()
    {
        // Arrange
        var beneficiaries = new List<Beneficiary>
        {
            new Beneficiary { Id = "1", Name = "John Doe", Contact = "john@example.com", Address = "123 Main St" },
            new Beneficiary { Id = "2", Name = "Jane Smith", Contact = "jane@example.com", Address = "456 Oak Ave" }
        };

        var asyncCursor = new Mock<IAsyncCursor<Beneficiary>>();
        asyncCursor.Setup(_ => _.Current).Returns(beneficiaries);
        asyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Beneficiary>>(),
            It.IsAny<FindOptions<Beneficiary, Beneficiary>>(),
            It.IsAny<CancellationToken>()))
            .Returns(new Mock<IAsyncCursor<Beneficiary>>().Object);

        // Act
        var result = await _beneficiaryService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_CreatesNewBeneficiary()
    {
        // Arrange
        var beneficiary = new Beneficiary
        {
            Name = "Test Beneficiary",
            Contact = "test@example.com",
            Address = "Test Address"
        };

        _mockCollection.Setup(c => c.InsertOneAsync(
            beneficiary,
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()));

        // Act
        var result = await _beneficiaryService.CreateAsync(beneficiary);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(beneficiary.Name, result.Name);
        _mockCollection.Verify(c => c.InsertOneAsync(
            beneficiary,
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBeneficiary_WhenExists()
    {
        // Arrange
        var beneficiary = new Beneficiary { Id = "1", Name = "John Doe", Contact = "john@example.com" };

        var asyncCursor = new Mock<IAsyncCursor<Beneficiary>>();
        asyncCursor.Setup(_ => _.Current).Returns(new[] { beneficiary });
        asyncCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Beneficiary>>(),
            It.IsAny<FindOptions<Beneficiary, Beneficiary>>(),
            It.IsAny<CancellationToken>()))
            .Returns(asyncCursor.Object);

        // Act
        var result = await _beneficiaryService.GetByIdAsync("1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
    }
}
