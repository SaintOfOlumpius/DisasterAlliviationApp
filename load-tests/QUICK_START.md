# Quick Start: Load Testing

This guide will help you quickly get started with load testing the Disaster Alleviation API.

## 🚀 Quick Setup (2 Minutes)

### Step 1: Install k6

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

**Verify Installation:**
```bash
k6 version
```

### Step 2: Start Your API

Make sure your Disaster Alleviation API is running:

```bash
cd DisasterAlleviationApp
dotnet run
```

The API should be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Step 3: Run Your First Load Test

**Smoke Test (Quick Validation):**
```bash
cd load-tests
k6 run --env BASE_URL=http://localhost:5000 k6-smoke-test.js
```

**Load Test (Normal Load):**
```bash
cd load-tests
k6 run --env BASE_URL=http://localhost:5000 k6-load-test.js
```

**Stress Test (High Load):**
```bash
cd load-tests
k6 run --env BASE_URL=http://localhost:5000 k6-stress-test.js
```

## 📊 Understanding Results

### Key Metrics to Watch

**HTTP Request Duration:**
- `p(95)<500ms` - 95% of requests should be under 500ms
- `p(99)<1000ms` - 99% of requests should be under 1000ms

**Error Rate:**
- `http_req_failed<1%` - Error rate should be less than 1%

**Request Rate:**
- `http_reqs/s` - Requests per second

### Sample Success Output

```
✓ GetAllDonations - Status is 200-299
✓ GetAllDonations - Response time < 1000ms

checks.........................: 100.00% ✓ 500    ✗ 0
data_received..................: 125 kB  2.5 kB/s
data_sent......................: 75 kB   1.5 kB/s
http_req_duration..............: avg=245ms   min=120ms  med=230ms  max=450ms  p(95)=420ms  p(99)=480ms
http_req_failed.................: 0.00%  ✓ 0        ✗ 500
http_reqs......................: 500     10/s
```

## 🎯 Test Types

### Smoke Test
- **Duration**: 30 seconds
- **Users**: 1
- **Purpose**: Quick validation
- **When to use**: Before deployments, quick health checks

### Load Test
- **Duration**: ~4 minutes
- **Users**: 10-20 (ramped)
- **Purpose**: Normal expected load
- **When to use**: Regular performance testing, CI/CD

### Stress Test
- **Duration**: ~9 minutes
- **Users**: 50-150 (ramped)
- **Purpose**: Find breaking points
- **When to use**: Capacity planning, performance tuning

## 🔧 Common Commands

```bash
# Run smoke test against local API
k6 run --env BASE_URL=http://localhost:5000 k6-smoke-test.js

# Run load test against staging
k6 run --env BASE_URL=https://staging-api.com k6-load-test.js

# Run with custom thresholds
k6 run --env BASE_URL=http://localhost:5000 --threshold http_req_duration=p(95)<300 k6-load-test.js

# Run with output to file
k6 run --env BASE_URL=http://localhost:5000 --out json=results.json k6-load-test.js

# Run in quiet mode (less output)
k6 run --quiet --env BASE_URL=http://localhost:5000 k6-smoke-test.js
```

## 📝 Next Steps

1. **Read Full Documentation**: See [README.md](README.md) for detailed information
2. **Customize Tests**: Edit test files to match your scenarios
3. **Integrate with CI/CD**: Add to Azure DevOps pipelines
4. **Monitor Results**: Track performance trends over time

## 🐛 Troubleshooting

### Connection Refused
- Ensure API is running
- Check BASE_URL is correct
- Verify port is accessible

### SSL Certificate Errors
```bash
# For self-signed certificates, use:
k6 run --insecure-skip-tls-verify --env BASE_URL=https://localhost:5001 k6-smoke-test.js
```

### High Error Rate
- Check API logs
- Verify database connectivity
- Monitor server resources

## 📚 Resources

- [k6 Documentation](https://k6.io/docs/)
- [k6 Examples](https://github.com/k6io/example-scripts)
- [Load Testing Best Practices](https://k6.io/docs/test-authoring/create-tests-from-recordings/)

---

**Happy Load Testing! 🚀**

