# Load Test Runner Script (PowerShell)
# This script makes it easy to run different types of load tests

param(
    [Parameter(Position=0)]
    [ValidateSet("smoke", "load", "stress")]
    [string]$TestType = "smoke",
    
    [Parameter()]
    [string]$Url = "http://localhost:5000",
    
    [switch]$Help,
    [switch]$Version
)

# Function to display usage
function Show-Usage {
    Write-Host "Usage: .\run-tests.ps1 [TEST_TYPE] [OPTIONS]" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "TEST_TYPE:" -ForegroundColor Yellow
    Write-Host "  smoke     - Quick validation test (default)"
    Write-Host "  load      - Normal load test"
    Write-Host "  stress    - High load stress test"
    Write-Host ""
    Write-Host "OPTIONS:" -ForegroundColor Yellow
    Write-Host "  -Url URL       Base URL of the API (default: http://localhost:5000)"
    Write-Host "  -Help          Show this help message"
    Write-Host "  -Version       Show k6 version"
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Green
    Write-Host "  .\run-tests.ps1 smoke"
    Write-Host "  .\run-tests.ps1 load -Url https://staging-api.com"
    Write-Host "  .\run-tests.ps1 stress"
    exit 0
}

# Function to check if k6 is installed
function Test-K6Installed {
    try {
        $null = Get-Command k6 -ErrorAction Stop
        return $true
    } catch {
        Write-Host "❌ k6 is not installed" -ForegroundColor Red
        Write-Host ""
        Write-Host "Install k6:"
        Write-Host "  choco install k6"
        Write-Host "  Or see: https://k6.io/docs/getting-started/installation/"
        exit 1
    }
}

# Show help
if ($Help) {
    Show-Usage
}

# Show version
if ($Version) {
    try {
        k6 version
    } catch {
        Write-Host "k6 is not installed" -ForegroundColor Red
        exit 1
    }
    exit 0
}

# Check if k6 is installed
Test-K6Installed

# Determine test file based on type
$testFile = switch ($TestType) {
    "smoke" { "k6-smoke-test.js" }
    "load" { "k6-load-test.js" }
    "stress" { "k6-stress-test.js" }
    default {
        Write-Host "Invalid test type: $TestType" -ForegroundColor Red
        Show-Usage
    }
}

# Check if test file exists
if (-not (Test-Path $testFile)) {
    Write-Host "❌ Test file not found: $testFile" -ForegroundColor Red
    exit 1
}

# Display test information
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
Write-Host "🚀 Disaster Alleviation API Load Testing" -ForegroundColor Green
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
Write-Host ""
Write-Host "Test Type: $TestType" -ForegroundColor Yellow
Write-Host "Base URL:  $Url" -ForegroundColor Yellow
try {
    $k6Version = k6 version 2>&1 | Select-Object -First 1
    Write-Host "k6 Version: $k6Version" -ForegroundColor Yellow
} catch {
    Write-Host "k6 Version: Unknown" -ForegroundColor Yellow
}
Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
Write-Host ""

# Run the test
Write-Host "▶ Starting test..." -ForegroundColor Green
Write-Host ""

$env:BASE_URL = $Url
k6 run --env BASE_URL="$Url" $testFile

$exitCode = $LASTEXITCODE

Write-Host ""
if ($exitCode -eq 0) {
    Write-Host "✅ Test completed successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Test failed with exit code: $exitCode" -ForegroundColor Red
}

exit $exitCode

