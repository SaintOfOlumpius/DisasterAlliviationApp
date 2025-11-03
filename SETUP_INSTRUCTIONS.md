# Quick Setup Instructions

## Prerequisites Installation

### Install .NET 8 SDK
**macOS:**
```bash
brew install --cask dotnet-sdk
```

**Windows:**
Download from: https://dotnet.microsoft.com/download/dotnet/8.0

**Linux:**
```bash
# Ubuntu/Debian
wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 8.0.0
```

### Install MongoDB
**macOS (using Homebrew):**
```bash
brew tap mongodb/brew
brew install mongodb-community
brew services start mongodb-community
```

**Windows:**
Download and install from: https://www.mongodb.com/try/download/community

**Verify MongoDB is running:**
```bash
mongosh
# or
mongo
```

## Project Setup

### 1. Restore Dependencies
```bash
cd DisasterAlleviationApp
dotnet restore
```

### 2. Import Sample Data (Optional)
```bash
# Using mongosh
mongosh DisasterAlleviationDB < import-sample-data.js

# Or manually in mongosh:
mongosh
use DisasterAlleviationDB
# Then paste the contents from import-sample-data.js
```

### 3. Run the Application
```bash
cd DisasterAlleviationApp
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001/swagger`

### 4. Run Tests
```bash
cd DisasterAlleviationApp.Tests
dotnet test
```

## Using Visual Studio

1. Open `DisasterAlleviationApp.sln`
2. Set `DisasterAlleviationApp` as startup project
3. Press **F5** to run
4. Swagger UI will open automatically

## Using VS Code

1. Open the project folder in VS Code
2. Press **F5** or run `dotnet run` from terminal
3. Navigate to `https://localhost:5001/swagger`

## API Testing Examples

### Using Swagger UI
1. Navigate to `https://localhost:5001/swagger`
2. Click "Try it out" on any endpoint
3. Fill in the request body
4. Click "Execute"

### Using curl

**Get all donations:**
```bash
curl -X GET "http://localhost:5000/api/donations"
```

**Create a donation:**
```bash
curl -X POST "http://localhost:5000/api/donations" \
  -H "Content-Type: application/json" \
  -d '{
    "Type": "Money",
    "Description": "Test donation",
    "Amount": 1000,
    "Date": "2024-01-15T10:00:00Z"
  }'
```

**Get summary report:**
```bash
curl -X GET "http://localhost:5000/api/reporting/summary"
```

## Troubleshooting

### MongoDB Connection Failed
- Ensure MongoDB is running: `brew services list` (macOS)
- Check connection string in `appsettings.json`
- Verify MongoDB is accessible: `mongosh`

### Port Already in Use
- Change the port in `launchSettings.json` (in Properties folder)
- Or kill the process: `lsof -ti:5000 | xargs kill -9` (macOS/Linux)

### Tests Fail
- Run `dotnet restore` first
- Some tests may require MongoDB to be running for integration tests
