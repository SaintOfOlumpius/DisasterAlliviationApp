// k6 Stress Test - High load test to find breaking points
// Run with: k6 run k6-stress-test.js

import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '1m', target: 50 },   // Ramp up to 50 users
        { duration: '2m', target: 50 },    // Stay at 50 users
        { duration: '1m', target: 100 },   // Ramp up to 100 users
        { duration: '2m', target: 100 },   // Stay at 100 users
        { duration: '1m', target: 150 },   // Ramp up to 150 users
        { duration: '2m', target: 150 },   // Stay at 150 users
        { duration: '1m', target: 0 },     // Ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<2000', 'p(99)<3000'], // More lenient thresholds for stress test
        http_req_failed: ['rate<0.05'],                   // Allow up to 5% error rate
    },
};

const BASE_URL = __ENV.BASE_URL || 'https://localhost:5001';

function generateRandomId() {
    return Math.random().toString(36).substring(2, 15);
}

function getRandomItem(array) {
    return array[Math.floor(Math.random() * array.length)];
}

const donationTypes = ['Money', 'Goods', 'Services'];
const disasterTypes = ['Hurricane', 'Flood', 'Earthquake', 'Fire', 'Tornado'];

export default function () {
    const random = Math.random();
    
    // 80% read operations, 15% create, 5% update
    if (random < 0.80) {
        // Read operations
        const readOperations = [
            () => http.get(`${BASE_URL}/api/donations`),
            () => http.get(`${BASE_URL}/api/volunteers`),
            () => http.get(`${BASE_URL}/api/beneficiaries`),
            () => http.get(`${BASE_URL}/api/disasters`),
            () => http.get(`${BASE_URL}/api/reporting/summary`),
        ];
        
        const operation = getRandomItem(readOperations);
        const response = operation();
        check(response, {
            'Read operation - Status is 200': (r) => r.status === 200,
        });
    } else if (random < 0.95) {
        // Create operations
        const donation = {
            Type: getRandomItem(donationTypes),
            Description: `Stress test donation ${generateRandomId()}`,
            Amount: Math.random() * 10000,
            Date: new Date().toISOString()
        };
        
        const response = http.post(`${BASE_URL}/api/donations`, JSON.stringify(donation), {
            headers: { 'Content-Type': 'application/json' },
        });
        
        check(response, {
            'Create donation - Status is 201': (r) => r.status === 201,
        });
    } else {
        // Update operations
        const donation = {
            Type: getRandomItem(donationTypes),
            Description: `Updated stress test donation ${generateRandomId()}`,
            Amount: Math.random() * 10000,
            Date: new Date().toISOString()
        };
        
        // Use a random ID (may fail if doesn't exist, which is OK for stress test)
        const randomId = generateRandomId();
        const response = http.put(`${BASE_URL}/api/donations/${randomId}`, JSON.stringify(donation), {
            headers: { 'Content-Type': 'application/json' },
        });
        
        check(response, {
            'Update donation - Status is 200 or 404': (r) => r.status === 200 || r.status === 404,
        });
    }
    
    sleep(Math.random() * 1 + 0.5); // Random sleep between 0.5-1.5 seconds
}

