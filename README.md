# Disaster Alleviation App

A .NET 8 Web API application for managing disaster relief operations with MongoDB integration.

## Project Structure

```
DisasterAlleviationApp/
├── DisasterAlleviationApp.sln
├── DisasterAlleviationApp/
│   ├── Controllers/
│   │   ├── DonationsController.cs
│   │   ├── BeneficiariesController.cs
│   │   ├── VolunteersController.cs
│   │   ├── DisastersController.cs
│   │   └── ReportingController.cs
│   ├── Models/
│   │   ├── Donation.cs
│   │   ├── Beneficiary.cs
│   │   ├── Volunteer.cs
│   │   └── Disaster.cs
│   ├── Services/
│   │   ├── DonationService.cs
│   │   ├── BeneficiaryService.cs
│   │   ├── VolunteerService.cs
│   │   └── DisasterService.cs
│   ├── DatabaseSettings.cs
│   ├── Program.cs
│   └── appsettings.json
└── DisasterAlleviationApp.Tests/
    ├── DonationTests.cs
    ├── BeneficiaryTests.cs
    ├── VolunteerTests.cs
    ├── DisasterTests.cs
    └── ReportingTests.cs
```

## Prerequisites

- .NET 8 SDK
- MongoDB (running on localhost:27017)
- Visual Studio 2022, VS Code, or Rider

## Setup Instructions

### 1. Install MongoDB

**On macOS (using Homebrew):**
```bash
brew tap mongodb/brew
brew install mongodb-community
brew services start mongodb-community
```

**On Windows:**
Download and install from [MongoDB Download Center](https://www.mongodb.com/try/download/community)

**On Linux:**
```bash
# Follow MongoDB installation guide for your distribution
# https://docs.mongodb.com/manual/installation/
```

Verify MongoDB is running:
```bash
mongosh
# or
mongo
```

### 2. Clone and Restore Dependencies

```bash
cd DisasterAlleviationApp
dotnet restore
```

### 3. Configure Database

The application is configured to use MongoDB at `mongodb://localhost:27017` with database name `DisasterAlleviationDB`. This is set in `appsettings.json`:

```json
{
  "DisasterAlleviationDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "DisasterAlleviationDB"
  }
}
```

### 4. Import Sample Data (Optional)

To import sample data into MongoDB:

```bash
# Using mongosh
mongosh DisasterAlleviationDB

# Then run the following commands in mongosh:
use DisasterAlleviationDB

db.Donations.insertMany([
  {
    "Type": "Money",
    "Description": "Cash donation for disaster relief",
    "Amount": 5000.00,
    "Date": ISODate("2024-01-15T10:00:00Z")
  },
  {
    "Type": "Goods",
    "Description": "Food supplies and clothing",
    "Amount": 2500.00,
    "Date": ISODate("2024-01-16T14:30:00Z")
  },
  {
    "Type": "Services",
    "Description": "Medical services and counseling",
    "Amount": 3000.00,
    "Date": ISODate("2024-01-17T09:15:00Z")
  }
])

db.Beneficiaries.insertMany([
  {
    "Name": "John Doe",
    "Contact": "john.doe@example.com",
    "Address": "123 Main Street, City, State 12345",
    "ReceivedDonations": []
  },
  {
    "Name": "Jane Smith",
    "Contact": "jane.smith@example.com",
    "Address": "456 Oak Avenue, City, State 67890",
    "ReceivedDonations": []
  }
])

db.Volunteers.insertMany([
  {
    "Name": "Alice Johnson",
    "Contact": "alice.johnson@example.com",
    "AssignedDisasters": []
  },
  {
    "Name": "Bob Williams",
    "Contact": "bob.williams@example.com",
    "AssignedDisasters": []
  }
])

db.Disasters.insertMany([
  {
    "Name": "Hurricane Alpha",
    "Type": "Hurricane",
    "DateOccurred": ISODate("2024-01-10T08:00:00Z"),
    "Location": "Coastal Region, State"
  },
  {
    "Name": "Flood Beta",
    "Type": "Flood",
    "DateOccurred": ISODate("2024-01-12T12:00:00Z"),
    "Location": "River Valley, State"
  }
])
```

Alternatively, you can use a script to import the sample data. See `sample-data.json` for the data format.

## Running the Application

### Using Visual Studio
1. Open `DisasterAlleviationApp.sln`
2. Set `DisasterAlleviationApp` as the startup project
3. Press F5 or click Run

### Using VS Code
1. Open the project folder
2. Press F5 or run `dotnet run` from the terminal

### Using Command Line
```bash
cd DisasterAlleviationApp
dotnet run
```

The application will start on:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

## Access Swagger UI

Once the application is running, navigate to:
- **Swagger UI**: `https://localhost:5001/swagger` (or `http://localhost:5000/swagger`)

Swagger UI provides an interactive interface to test all API endpoints.

## API Endpoints

### Donations
- `GET /api/donations` - Get all donations
- `GET /api/donations/{id}` - Get donation by ID
- `POST /api/donations` - Create a new donation
- `PUT /api/donations/{id}` - Update a donation
- `DELETE /api/donations/{id}` - Delete a donation

### Beneficiaries
- `GET /api/beneficiaries` - Get all beneficiaries
- `GET /api/beneficiaries/{id}` - Get beneficiary by ID
- `POST /api/beneficiaries` - Create a new beneficiary
- `PUT /api/beneficiaries/{id}` - Update a beneficiary
- `DELETE /api/beneficiaries/{id}` - Delete a beneficiary

### Volunteers
- `GET /api/volunteers` - Get all volunteers
- `GET /api/volunteers/{id}` - Get volunteer by ID
- `POST /api/volunteers` - Create a new volunteer
- `PUT /api/volunteers/{id}` - Update a volunteer
- `DELETE /api/volunteers/{id}` - Delete a volunteer

### Disasters
- `GET /api/disasters` - Get all disasters
- `GET /api/disasters/{id}` - Get disaster by ID
- `POST /api/disasters` - Create a new disaster
- `PUT /api/disasters/{id}` - Update a disaster
- `DELETE /api/disasters/{id}` - Delete a disaster

### Reporting
- `GET /api/reporting/summary` - Get summary statistics including:
  - Total counts for donations, beneficiaries, volunteers, and disasters
  - Donations grouped by type with counts and total amounts
  - Total donation amount

## Running xUnit Tests

### Using Visual Studio
1. Open Test Explorer (Test > Test Explorer)
2. Run All Tests

### Using Command Line
```bash
cd DisasterAlleviationApp.Tests
dotnet test
```

### Using VS Code
Run the test command from the terminal:
```bash
dotnet test
```

## Test Coverage

The test suite includes:
- **DonationTests**: CRUD operations for donations
- **BeneficiaryTests**: CRUD operations for beneficiaries
- **VolunteerTests**: CRUD operations for volunteers
- **DisasterTests**: CRUD operations for disasters
- **ReportingTests**: Summary endpoint testing with counts and grouping

## Models

### Donation
- `Id` (string) - MongoDB ObjectId
- `Type` (string) - Type of donation (Money, Goods, Services)
- `Description` (string) - Description of the donation
- `Amount` (decimal) - Donation amount
- `Date` (DateTime) - Date of donation

### Beneficiary
- `Id` (string) - MongoDB ObjectId
- `Name` (string) - Beneficiary name
- `Contact` (string) - Contact information
- `Address` (string) - Physical address
- `ReceivedDonations` (List<string>) - List of donation IDs

### Volunteer
- `Id` (string) - MongoDB ObjectId
- `Name` (string) - Volunteer name
- `Contact` (string) - Contact information
- `AssignedDisasters` (List<string>) - List of disaster IDs

### Disaster
- `Id` (string) - MongoDB ObjectId
- `Name` (string) - Disaster name
- `Type` (string) - Type of disaster
- `DateOccurred` (DateTime) - When the disaster occurred
- `Location` (string) - Location of the disaster

## Technologies Used

- **.NET 8** - Framework
- **ASP.NET Core Web API** - API framework
- **MongoDB.Driver** - MongoDB client library
- **xUnit** - Testing framework
- **Moq** - Mocking framework for unit tests
- **Swagger/OpenAPI** - API documentation

## Troubleshooting

### MongoDB Connection Issues
- Ensure MongoDB is running: `brew services list` (macOS) or check services (Windows)
- Verify connection string in `appsettings.json`
- Check MongoDB logs for errors

### Port Already in Use
- Change the port in `launchSettings.json` or use a different port
- Kill the process using the port: `lsof -ti:5000 | xargs kill -9` (macOS/Linux)

### Test Failures
- Ensure all NuGet packages are restored: `dotnet restore`
- Check that MongoDB is accessible (some tests may require MongoDB running)

## CI/CD with Azure DevOps

This project includes comprehensive Azure DevOps pipeline configuration for automated builds, testing, and reporting.

### Quick Setup

1. **Initialize Repository** (if not already done):
   ```bash
   # macOS/Linux
   ./setup-azure-devops.sh
   
   # Windows
   .\setup-azure-devops.ps1
   ```

2. **Create Pipeline in Azure DevOps**:
   - Go to Azure DevOps > Pipelines > New pipeline
   - Select "Existing Azure Pipelines YAML file"
   - Choose `azure-pipelines.yml`
   - Run!

### Pipeline Features

✅ **Automated Build**: Builds on every push and PR  
✅ **Test Execution**: Runs all unit tests automatically  
✅ **Code Coverage**: Collects and publishes coverage reports  
✅ **Test Results**: Publishes test results in multiple formats  
✅ **Build Artifacts**: Creates deployment-ready packages  

### Documentation

- **Quick Start**: See [QUICK_START_AZURE_DEVOPS.md](QUICK_START_AZURE_DEVOPS.md) for 5-minute setup
- **Comprehensive Guide**: See [AZURE_DEVOPS_SETUP.md](AZURE_DEVOPS_SETUP.md) for detailed instructions
- **Files Overview**: See [AZURE_DEVOPS_FILES_SUMMARY.md](AZURE_DEVOPS_FILES_SUMMARY.md) for file descriptions

### Pipeline Files

- **`azure-pipelines.yml`** - Main CI/CD pipeline (recommended)
- **`azure-pipelines-full.yml`** - Full-featured pipeline with advanced features
- **`.azuredevops/pipeline-templates/`** - Reusable pipeline templates

## Load Testing

This project includes comprehensive load tests using k6 to test API performance under various load conditions.

### Quick Start

1. **Install k6**:
   ```bash
   # macOS
   brew install k6
   
   # Windows
   choco install k6
   ```

2. **Start Your API**:
   ```bash
   cd DisasterAlleviationApp
   dotnet run
   ```

3. **Run Load Tests**:
   ```bash
   cd load-tests
   
   # Smoke test (quick validation)
   ./run-tests.sh smoke
   
   # Load test (normal load)
   ./run-tests.sh load
   
   # Stress test (high load)
   ./run-tests.sh stress
   ```

### Test Types

- **Smoke Test**: Quick validation with 1 user for 30 seconds
- **Load Test**: Normal expected load (10-20 users over 4 minutes)
- **Stress Test**: High load to find breaking points (50-150 users over 9 minutes)

### Test Coverage

Load tests cover all API endpoints:
- ✅ All GET endpoints (Donations, Volunteers, Beneficiaries, Disasters, Reporting)
- ✅ All POST endpoints (Create operations)
- ✅ All PUT endpoints (Update operations)
- ✅ All DELETE endpoints (Delete operations)

### Documentation

- **Quick Start**: See [load-tests/QUICK_START.md](load-tests/QUICK_START.md) for 2-minute setup
- **Full Documentation**: See [load-tests/README.md](load-tests/README.md) for detailed information
- **Azure DevOps Integration**: See `azure-pipelines-load-test.yml` for CI/CD integration

### Integration with Azure DevOps

Load tests can be integrated into Azure DevOps pipelines. See `azure-pipelines-load-test.yml` for configuration.

### imagne snippets

![image not found](images/Opera%20Snapshot_2025-11-03_214452_dev.azure.com.png)
![image not found](images/Opera%20Snapshot_2025-11-03_214508_dev.azure.com.repos.png)
![image not found](images/Opera%20Snapshot_2025-11-03_214652_dev.azure.com.fail.png)
![image not found](images/Opera%20Snapshot_2025-11-03_214719_dev.azure.com.pipe.png)


##   Unit Test report 
[Download the full report](docs/Swagger%20UI.pdf)
[Download the full report with a little more detail](docs/Swagger%20UI%20a%20little%20more%20detail.pdf)

[My GitHub Repository](https://github.com/SaintOfOlumpius/DisasterAlliviationApp)
