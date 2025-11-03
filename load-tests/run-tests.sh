#!/bin/bash

# Load Test Runner Script
# This script makes it easy to run different types of load tests

set -e

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Default values
BASE_URL="${BASE_URL:-http://localhost:5000}"
TEST_TYPE="smoke"

# Function to display usage
usage() {
    echo "Usage: $0 [OPTIONS] [TEST_TYPE]"
    echo ""
    echo "TEST_TYPE:"
    echo "  smoke     - Quick validation test (default)"
    echo "  load      - Normal load test"
    echo "  stress    - High load stress test"
    echo ""
    echo "OPTIONS:"
    echo "  -u, --url URL       Base URL of the API (default: http://localhost:5000)"
    echo "  -h, --help          Show this help message"
    echo "  -v, --version       Show k6 version"
    echo ""
    echo "Examples:"
    echo "  $0 smoke"
    echo "  $0 load --url https://staging-api.com"
    echo "  BASE_URL=https://api.com $0 stress"
    exit 1
}

# Function to check if k6 is installed
check_k6() {
    if ! command -v k6 &> /dev/null; then
        echo -e "${RED}❌ k6 is not installed${NC}"
        echo ""
        echo "Install k6:"
        echo "  macOS:   brew install k6"
        echo "  Windows: choco install k6"
        echo "  Linux:   See https://k6.io/docs/getting-started/installation/"
        exit 1
    fi
}

# Function to get k6 version
show_version() {
    k6 version
    exit 0
}

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        -u|--url)
            BASE_URL="$2"
            shift 2
            ;;
        -h|--help)
            usage
            ;;
        -v|--version)
            show_version
            ;;
        smoke|load|stress)
            TEST_TYPE="$1"
            shift
            ;;
        *)
            echo -e "${RED}Unknown option: $1${NC}"
            usage
            ;;
    esac
done

# Check if k6 is installed
check_k6

# Determine test file based on type
case $TEST_TYPE in
    smoke)
        TEST_FILE="k6-smoke-test.js"
        TEST_NAME="Smoke Test"
        ;;
    load)
        TEST_FILE="k6-load-test.js"
        TEST_NAME="Load Test"
        ;;
    stress)
        TEST_FILE="k6-stress-test.js"
        TEST_NAME="Stress Test"
        ;;
    *)
        echo -e "${RED}Invalid test type: $TEST_TYPE${NC}"
        usage
        ;;
esac

# Check if test file exists
if [ ! -f "$TEST_FILE" ]; then
    echo -e "${RED}❌ Test file not found: $TEST_FILE${NC}"
    exit 1
fi

# Display test information
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${GREEN}🚀 Disaster Alleviation API Load Testing${NC}"
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""
echo -e "Test Type: ${YELLOW}$TEST_NAME${NC}"
echo -e "Base URL:  ${YELLOW}$BASE_URL${NC}"
echo -e "k6 Version: ${YELLOW}$(k6 version | head -n 1)${NC}"
echo ""
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

# Run the test
echo -e "${GREEN}▶ Starting test...${NC}"
echo ""

k6 run --env BASE_URL="$BASE_URL" "$TEST_FILE"

EXIT_CODE=$?

echo ""
if [ $EXIT_CODE -eq 0 ]; then
    echo -e "${GREEN}✅ Test completed successfully${NC}"
else
    echo -e "${RED}❌ Test failed with exit code: $EXIT_CODE${NC}"
fi

exit $EXIT_CODE

