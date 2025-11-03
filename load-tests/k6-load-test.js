// k6 Load Test for Disaster Alleviation API
// Run with: k6 run k6-load-test.js

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend, Counter } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const responseTime = new Trend('response_time');
const requestCounter = new Counter('total_requests');

// Configuration
export const options = {
    stages: [
        { duration: '30s', target: 10 },  // Ramp up to 10 users over 30 seconds
        { duration: '1m', target: 10 },   // Stay at 10 users for 1 minute
        { duration: '30s', target: 20 },   // Ramp up to 20 users over 30 seconds
        { duration: '1m', target: 20 },    // Stay at 20 users for 1 minute
        { duration: '30s', target: 0 },   // Ramp down to 0 users over 30 seconds
    ],
    thresholds: {
        http_req_duration: ['p(95)<500', 'p(99)<1000'], // 95% of requests should be below 500ms, 99% below 1000ms
        http_req_failed: ['rate<0.01'],                   // Error rate should be less than 1%
        errors: ['rate<0.01'],                            // Custom error rate threshold
    },
};

// Base URL - configure via environment variable or update here
const BASE_URL = __ENV.BASE_URL || 'https://localhost:5001';

// Helper function to generate random IDs
function generateRandomId() {
    return Math.random().toString(36).substring(2, 15);
}

// Helper function to get random item from array
function getRandomItem(array) {
    return array[Math.floor(Math.random() * array.length)];
}

// Test data generators
const donationTypes = ['Money', 'Goods', 'Services'];
const disasterTypes = ['Hurricane', 'Flood', 'Earthquake', 'Fire', 'Tornado'];

// Store created IDs for use in subsequent requests
let donationIds = [];
let volunteerIds = [];
let beneficiaryIds = [];
let disasterIds = [];

export function setup() {
    // Setup function - runs once before all VUs
    console.log('Setting up test data...');
    
    // Create some initial test data
    const setupRequests = [];
    
    // Create initial donations
    for (let i = 0; i < 5; i++) {
        const donation = {
            Type: getRandomItem(donationTypes),
            Description: `Test donation ${i}`,
            Amount: Math.random() * 10000,
            Date: new Date().toISOString()
        };
        setupRequests.push({ url: `${BASE_URL}/api/donations`, data: donation });
    }
    
    return { createdCount: 5 };
}

export default function (data) {
    // Main test function - runs for each VU iteration
    
    const testScenarios = [
        // Read operations (70% of traffic)
        testGetAllDonations,
        testGetAllVolunteers,
        testGetAllBeneficiaries,
        testGetAllDisasters,
        testGetDonationById,
        testGetVolunteerById,
        testGetBeneficiaryById,
        testGetDisasterById,
        testGetReportingSummary,
        
        // Write operations (20% of traffic)
        testCreateDonation,
        testCreateVolunteer,
        testCreateBeneficiary,
        testCreateDisaster,
        
        // Update operations (8% of traffic)
        testUpdateDonation,
        testUpdateVolunteer,
        
        // Delete operations (2% of traffic)
        testDeleteDonation,
    ];
    
    // Weighted random selection - prefer read operations
    const random = Math.random();
    let scenario;
    
    if (random < 0.70) {
        // 70% - Read operations
        scenario = testScenarios[Math.floor(Math.random() * 9)];
    } else if (random < 0.90) {
        // 20% - Create operations
        scenario = testScenarios[9 + Math.floor(Math.random() * 4)];
    } else if (random < 0.98) {
        // 8% - Update operations
        scenario = testScenarios[13 + Math.floor(Math.random() * 2)];
    } else {
        // 2% - Delete operations
        scenario = testScenarios[15];
    }
    
    scenario();
    
    // Think time between requests
    sleep(Math.random() * 2 + 1); // Random sleep between 1-3 seconds
}

// Test functions for each endpoint

function testGetAllDonations() {
    const response = http.get(`${BASE_URL}/api/donations`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetAllDonations' }
    });
    
    checkResponse(response, 'GetAllDonations');
}

function testGetAllVolunteers() {
    const response = http.get(`${BASE_URL}/api/volunteers`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetAllVolunteers' }
    });
    
    checkResponse(response, 'GetAllVolunteers');
}

function testGetAllBeneficiaries() {
    const response = http.get(`${BASE_URL}/api/beneficiaries`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetAllBeneficiaries' }
    });
    
    checkResponse(response, 'GetAllBeneficiaries');
}

function testGetAllDisasters() {
    const response = http.get(`${BASE_URL}/api/disasters`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetAllDisasters' }
    });
    
    checkResponse(response, 'GetAllDisasters');
}

function testGetDonationById() {
    if (donationIds.length === 0) {
        // If no IDs available, skip this test
        return;
    }
    
    const id = getRandomItem(donationIds);
    const response = http.get(`${BASE_URL}/api/donations/${id}`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetDonationById' }
    });
    
    checkResponse(response, 'GetDonationById');
}

function testGetVolunteerById() {
    if (volunteerIds.length === 0) {
        return;
    }
    
    const id = getRandomItem(volunteerIds);
    const response = http.get(`${BASE_URL}/api/volunteers/${id}`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetVolunteerById' }
    });
    
    checkResponse(response, 'GetVolunteerById');
}

function testGetBeneficiaryById() {
    if (beneficiaryIds.length === 0) {
        return;
    }
    
    const id = getRandomItem(beneficiaryIds);
    const response = http.get(`${BASE_URL}/api/beneficiaries/${id}`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetBeneficiaryById' }
    });
    
    checkResponse(response, 'GetBeneficiaryById');
}

function testGetDisasterById() {
    if (disasterIds.length === 0) {
        return;
    }
    
    const id = getRandomItem(disasterIds);
    const response = http.get(`${BASE_URL}/api/disasters/${id}`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetDisasterById' }
    });
    
    checkResponse(response, 'GetDisasterById');
}

function testGetReportingSummary() {
    const response = http.get(`${BASE_URL}/api/reporting/summary`, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'GetReportingSummary' }
    });
    
    checkResponse(response, 'GetReportingSummary');
}

function testCreateDonation() {
    const donation = {
        Type: getRandomItem(donationTypes),
        Description: `Load test donation ${generateRandomId()}`,
        Amount: Math.random() * 10000,
        Date: new Date().toISOString()
    };
    
    const response = http.post(`${BASE_URL}/api/donations`, JSON.stringify(donation), {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'CreateDonation' }
    });
    
    if (checkResponse(response, 'CreateDonation') && response.status === 201) {
        try {
            const body = JSON.parse(response.body);
            if (body.Id) {
                donationIds.push(body.Id);
            }
        } catch (e) {
            // Ignore parse errors
        }
    }
}

function testCreateVolunteer() {
    const volunteer = {
        Name: `Load Test Volunteer ${generateRandomId()}`,
        Contact: `loadtest${generateRandomId()}@example.com`,
        AssignedDisasters: []
    };
    
    const response = http.post(`${BASE_URL}/api/volunteers`, JSON.stringify(volunteer), {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'CreateVolunteer' }
    });
    
    if (checkResponse(response, 'CreateVolunteer') && response.status === 201) {
        try {
            const body = JSON.parse(response.body);
            if (body.Id) {
                volunteerIds.push(body.Id);
            }
        } catch (e) {
            // Ignore parse errors
        }
    }
}

function testCreateBeneficiary() {
    const beneficiary = {
        Name: `Load Test Beneficiary ${generateRandomId()}`,
        Contact: `loadtest${generateRandomId()}@example.com`,
        Address: `123 Test Street ${generateRandomId()}`,
        ReceivedDonations: []
    };
    
    const response = http.post(`${BASE_URL}/api/beneficiaries`, JSON.stringify(beneficiary), {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'CreateBeneficiary' }
    });
    
    if (checkResponse(response, 'CreateBeneficiary') && response.status === 201) {
        try {
            const body = JSON.parse(response.body);
            if (body.Id) {
                beneficiaryIds.push(body.Id);
            }
        } catch (e) {
            // Ignore parse errors
        }
    }
}

function testCreateDisaster() {
    const disaster = {
        Name: `Load Test Disaster ${generateRandomId()}`,
        Type: getRandomItem(disasterTypes),
        DateOccurred: new Date().toISOString(),
        Location: `Test Location ${generateRandomId()}`
    };
    
    const response = http.post(`${BASE_URL}/api/disasters`, JSON.stringify(disaster), {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'CreateDisaster' }
    });
    
    if (checkResponse(response, 'CreateDisaster') && response.status === 201) {
        try {
            const body = JSON.parse(response.body);
            if (body.Id) {
                disasterIds.push(body.Id);
            }
        } catch (e) {
            // Ignore parse errors
        }
    }
}

function testUpdateDonation() {
    if (donationIds.length === 0) {
        return;
    }
    
    const id = getRandomItem(donationIds);
    const donation = {
        Type: getRandomItem(donationTypes),
        Description: `Updated donation ${generateRandomId()}`,
        Amount: Math.random() * 10000,
        Date: new Date().toISOString()
    };
    
    const response = http.put(`${BASE_URL}/api/donations/${id}`, JSON.stringify(donation), {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'UpdateDonation' }
    });
    
    checkResponse(response, 'UpdateDonation');
}

function testUpdateVolunteer() {
    if (volunteerIds.length === 0) {
        return;
    }
    
    const id = getRandomItem(volunteerIds);
    const volunteer = {
        Name: `Updated Volunteer ${generateRandomId()}`,
        Contact: `updated${generateRandomId()}@example.com`,
        AssignedDisasters: []
    };
    
    const response = http.put(`${BASE_URL}/api/volunteers/${id}`, JSON.stringify(volunteer), {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'UpdateVolunteer' }
    });
    
    checkResponse(response, 'UpdateVolunteer');
}

function testDeleteDonation() {
    if (donationIds.length === 0) {
        return;
    }
    
    const id = donationIds.pop(); // Remove from array
    const response = http.del(`${BASE_URL}/api/donations/${id}`, null, {
        headers: { 'Content-Type': 'application/json' },
        tags: { name: 'DeleteDonation' }
    });
    
    checkResponse(response, 'DeleteDonation');
}

// Helper function to check response and record metrics
function checkResponse(response, operationName) {
    const isOk = check(response, {
        [`${operationName} - Status is 200-299`]: (r) => r.status >= 200 && r.status < 300,
        [`${operationName} - Response time < 1000ms`]: (r) => r.timings.duration < 1000,
    }, { tags: { name: operationName } });
    
    errorRate.add(!isOk);
    responseTime.add(response.timings.duration);
    requestCounter.add(1);
    
    if (!isOk) {
        console.error(`${operationName} failed: ${response.status} - ${response.body}`);
    }
    
    return isOk;
}

export function teardown(data) {
    // Cleanup function - runs once after all VUs finish
    console.log('Test completed. Cleaning up...');
}

