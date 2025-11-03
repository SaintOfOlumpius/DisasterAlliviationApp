# Load Tests for Disaster Alleviation API

This directory contains load tests for the Disaster Alleviation API using k6.

## Prerequisites

### Install k6

**macOS:**
```bash
brew install k6
```

**Windows:**
```powershell
choco install k6
```

**Linux:**
```bash
sudo gpg -k
sudo gpg --no-default-keyring --keyring /usr/share/keyrings/k6-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
echo "deb [signed-by=/usr/share/keyrings/k6-archive-keyring.gpg] https://dl.k6.io/deb stable main" | sudo tee /etc/apt/sources.list.d/k6.list
sudo apt-get update
sudo apt-get install k6
```

**Docker:**
```bash
docker pull grafana/k6
```

## Test Files

### 1. Smoke Test (`k6-smoke-test.js`)
- **Purpose**: Quick validation that all endpoints work
- **Load**: 1 user for 30 seconds
- **Use Case**: Run before deployments to ensure basic functionality

```bash
k6 run k6-smoke-test.js
```

### 2. Load Test (`k6-load-test.js`)
- **Purpose**: Normal expected load simulation
- **Load**: Ramps from 10 to 20 users over 4 minutes
- **Use Case**: Regular performance testing, CI/CD integration

```bash
k6 run k6-load-test.js
```

### 3. Stress Test (`k6-stress-test.js`)
- **Purpose**: Find breaking points and capacity limits
- **Load**: Ramps from 50 to 150 users over 9 minutes
- **Use Case**: Capacity planning, performance tuning

```bash
k6 run k6-stress-test.js
```

## Running Tests

### Local Development (HTTP)

```bash
# Smoke test against local API
BASE_URL=http://localhost:5000 k6 run k6-smoke-test.js

# Load test against local API
BASE_URL=http://localhost:5000 k6 run k6-load-test.js

# Stress test against local API
BASE_URL=http://localhost:5000 k6 run k6-stress-test.js
```

### Production/Staging (HTTPS)

```bash
# Smoke test against production
BASE_URL=https://your-api.com k6 run k6-smoke-test.js

# Load test against production
BASE_URL=https://your-api.com k6 run k6-load-test.js

# Stress test against production
BASE_URL=https://your-api.com k6-stress-test.js
```

### Using npm Scripts

```bash
# Install dependencies (if needed)
npm install

# Run smoke test (local)
npm run test:smoke:local

# Run load test (local)
npm run test:load:local

# Run stress test (local)
npm run test:stress:local
```

### Using Docker

```bash
# Smoke test
docker run --rm -i grafana/k6 run - <k6-smoke-test.js

# Load test
docker run --rm -i grafana/k6 run - <k6-load-test.js

# Stress test
docker run --rm -i grafana/k6 run - <k6-stress-test.js
```

## Test Scenarios

### Endpoints Tested

**Read Operations (70% of traffic):**
- `GET /api/donations` - Get all donations
- `GET /api/donations/{id}` - Get donation by ID
- `GET /api/volunteers` - Get all volunteers
- `GET /api/volunteers/{id}` - Get volunteer by ID
- `GET /api/beneficiaries` - Get all beneficiaries
- `GET /api/beneficiaries/{id}` - Get beneficiary by ID
- `GET /api/disasters` - Get all disasters
- `GET /api/disasters/{id}` - Get disaster by ID
- `GET /api/reporting/summary` - Get reporting summary

**Write Operations (20% of traffic):**
- `POST /api/donations` - Create donation
- `POST /api/volunteers` - Create volunteer
- `POST /api/beneficiaries` - Create beneficiary
- `POST /api/disasters` - Create disaster

**Update Operations (8% of traffic):**
- `PUT /api/donations/{id}` - Update donation
- `PUT /api/volunteers/{id}` - Update volunteer

**Delete Operations (2% of traffic):**
- `DELETE /api/donations/{id}` - Delete donation

## Performance Thresholds

### Load Test Thresholds
- **95th percentile response time**: < 500ms
- **99th percentile response time**: < 1000ms
- **Error rate**: < 1%

### Stress Test Thresholds
- **95th percentile response time**: < 2000ms
- **99th percentile response time**: < 3000ms
- **Error rate**: < 5%

## Understanding Results

### Key Metrics

**HTTP Request Duration:**
- `avg`: Average response time
- `min`: Minimum response time
- `med`: Median response time
- `max`: Maximum response time
- `p(90)`: 90th percentile
- `p(95)`: 95th percentile
- `p(99)`: 99th percentile

**HTTP Request Rate:**
- `http_reqs`: Total HTTP requests
- `http_reqs/s`: Requests per second

**Errors:**
- `http_req_failed`: Percentage of failed requests
- `errors`: Custom error metric

### Sample Output

```
     ✓ GetAllDonations - Status is 200-299
     ✓ GetAllDonations - Response time < 1000ms
     ✓ CreateDonation - Status is 200-299
     ✓ CreateDonation - Response time < 1000ms

     checks.........................: 100.00% ✓ 23456    ✗ 0
     data_received..................: 2.5 MB  12.5 kB/s
     data_sent......................: 1.8 MB  9.0 kB/s
     http_req_duration..............: avg=245ms   min=120ms  med=230ms  max=890ms  p(90)=420ms  p(95)=510ms
     http_req_failed.................: 0.00%  ✓ 0        ✗ 23456
     http_reqs......................: 23456   117.28/s
     iteration_duration.............: avg=3.2s    min=2.1s   med=3.0s   max=5.8s
     iterations.....................: 23456   117.28/s
```

## Integration with Azure DevOps

Load tests can be integrated into Azure DevOps pipelines. See `azure-pipelines-load-test.yml` for an example.

### Running in Pipeline

```yaml
- task: Bash@3
  displayName: 'Install k6'
  inputs:
    targetType: 'inline'
    script: |
      # Install k6 based on OS
      # (add appropriate install commands)

- task: Bash@3
  displayName: 'Run Load Tests'
  inputs:
    targetType: 'inline'
    script: |
      cd load-tests
      k6 run k6-load-test.js --env BASE_URL=$(ApiBaseUrl)
```

## Troubleshooting

### Common Issues

**Connection Refused:**
- Ensure the API is running
- Check the BASE_URL is correct
- Verify the port is accessible

**SSL Certificate Errors:**
- Use `--insecure-skip-tls-verify` flag for self-signed certificates:
  ```bash
  k6 run --insecure-skip-tls-verify k6-load-test.js
  ```

**High Error Rate:**
- Check API logs for errors
- Verify database connectivity
- Check resource limits (CPU, memory)

**Slow Response Times:**
- Check database query performance
- Verify API scaling
- Monitor resource usage

## Best Practices

1. **Start with Smoke Tests**: Always run smoke tests before load tests
2. **Gradual Load Increase**: Use ramp-up stages to gradually increase load
3. **Monitor Resources**: Monitor API server resources during tests
4. **Test Realistic Scenarios**: Use realistic data and user behavior
5. **Regular Testing**: Run load tests regularly as part of CI/CD
6. **Baseline Metrics**: Establish performance baselines and track changes

## Additional Resources

- [k6 Documentation](https://k6.io/docs/)
- [k6 Examples](https://github.com/k6io/example-scripts)
- [Load Testing Best Practices](https://k6.io/docs/test-authoring/create-tests-from-recordings/)

## License

This project is provided as-is for educational purposes.

