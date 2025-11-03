// k6 Smoke Test - Light load test for quick validation
// Run with: k6 run k6-smoke-test.js

import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 1,              // 1 user
    duration: '30s',     // Run for 30 seconds
    thresholds: {
        http_req_duration: ['p(95)<500'],
        http_req_failed: ['rate<0.01'],
    },
};

const BASE_URL = __ENV.BASE_URL || 'https://localhost:5001';

export default function () {
    // Test key endpoints with light load
    
    // GET all donations
    let response = http.get(`${BASE_URL}/api/donations`);
    check(response, {
        'GET /api/donations - Status is 200': (r) => r.status === 200,
    });
    
    sleep(1);
    
    // GET all volunteers
    response = http.get(`${BASE_URL}/api/volunteers`);
    check(response, {
        'GET /api/volunteers - Status is 200': (r) => r.status === 200,
    });
    
    sleep(1);
    
    // GET all beneficiaries
    response = http.get(`${BASE_URL}/api/beneficiaries`);
    check(response, {
        'GET /api/beneficiaries - Status is 200': (r) => r.status === 200,
    });
    
    sleep(1);
    
    // GET all disasters
    response = http.get(`${BASE_URL}/api/disasters`);
    check(response, {
        'GET /api/disasters - Status is 200': (r) => r.status === 200,
    });
    
    sleep(1);
    
    // GET reporting summary
    response = http.get(`${BASE_URL}/api/reporting/summary`);
    check(response, {
        'GET /api/reporting/summary - Status is 200': (r) => r.status === 200,
    });
    
    sleep(1);
    
    // POST create donation
    const donation = {
        Type: 'Money',
        Description: 'Smoke test donation',
        Amount: 100.00,
        Date: new Date().toISOString()
    };
    
    response = http.post(`${BASE_URL}/api/donations`, JSON.stringify(donation), {
        headers: { 'Content-Type': 'application/json' },
    });
    
    check(response, {
        'POST /api/donations - Status is 201': (r) => r.status === 201,
    });
    
    sleep(1);
}

