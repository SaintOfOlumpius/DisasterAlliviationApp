# Azure DevOps Pipeline Setup Guide

This guide will help you set up Azure DevOps pipelines for the Disaster Alleviation App project.

## Prerequisites

1. Azure DevOps organization (https://dev.azure.com)
2. Project repository (Azure Repos or GitHub)
3. Azure DevOps project with appropriate permissions

## Step 1: Create Azure DevOps Repository

### Option A: Using Azure Repos

1. Navigate to your Azure DevOps organization
2. Create a new project or use an existing one
3. Go to **Repos** > **Files**
4. Click **Import** and import your local repository
5. Or use Git to push your code:

```bash
# Initialize git if not already done
git init
git add .
git commit -m "Initial commit"

# Add Azure DevOps remote
git remote add origin https://dev.azure.com/[Organization]/[Project]/_git/[Repository]
git push -u origin main
```

### Option B: Connect GitHub Repository

1. Go to **Project Settings** > **Repos**
2. Click **New repository** > **GitHub**
3. Authorize and select your GitHub repository
4. Follow the connection wizard

## Step 2: Create Pipeline

### Method 1: Using YAML File (Recommended)

1. Navigate to **Pipelines** > **Pipelines**
2. Click **New pipeline**
3. Select your repository source (Azure Repos, GitHub, etc.)
4. Choose your repository
5. Select **Existing Azure Pipelines YAML file**
6. Choose the branch (usually `main` or `master`)
7. Select the path: `azure-pipelines.yml`
8. Click **Continue** and then **Run**

### Method 2: Using Classic Editor

1. Navigate to **Pipelines** > **Pipelines**
2. Click **New pipeline**
3. Select **Use the classic editor**
4. Choose your repository source
5. Select **.NET Desktop** template or **Empty job**
6. Configure tasks:
   - .NET Core restore
   - .NET Core build
   - .NET Core test (with code coverage)
   - Publish test results
   - Publish code coverage

## Step 3: Configure Pipeline Variables

1. Go to your pipeline
2. Click **Edit** > **Variables**
3. Add any required variables:
   - `MongoDBConnectionString` (Mark as secret)
   - `BuildConfiguration` (default: Release)
   - Any other configuration values

## Step 4: Enable Branch Policies (Optional)

1. Go to **Repos** > **Branches**
2. Right-click on `main` branch > **Branch policies**
3. Enable:
   - **Build validation**: Require pipeline to succeed
   - **Status checks**: Require tests to pass
   - **Minimum number of reviewers**: 1

## Step 5: Configure Code Coverage

The pipeline is already configured to collect code coverage using Coverlet. Coverage reports will be published automatically to:
- Test results tab
- Code coverage tab in build summary

## Pipeline Features

✅ **Automated Build**: Builds on every push and PR
✅ **Test Execution**: Runs all unit tests automatically
✅ **Code Coverage**: Collects and publishes coverage reports
✅ **Test Results**: Publishes test results in multiple formats
✅ **Artifact Publishing**: Publishes build artifacts for deployment
✅ **Quality Gates**: Fails pipeline if tests fail

## Pipeline Stages

1. **Build**: Restores, builds, and tests the solution
2. **Quality Check**: Code quality validations
3. **Security Scan**: Dependency vulnerability scanning

## Viewing Results

- **Test Results**: Go to **Pipelines** > Your pipeline > Run > **Tests** tab
- **Code Coverage**: Go to **Pipelines** > Your pipeline > Run > **Code coverage** tab
- **Build Artifacts**: Go to **Pipelines** > Your pipeline > Run > **Artifacts**

## Troubleshooting

### Tests Not Running
- Ensure test projects are named `*Tests.csproj`
- Check that test project references the main project correctly

### Code Coverage Not Showing
- Verify `coverlet.collector` package is installed in test project
- Check that tests are using `--collect:"XPlat Code Coverage"`

### Build Failures
- Check the build logs for detailed error messages
- Ensure all NuGet packages can be restored
- Verify .NET 8.0 SDK is available in the pipeline

## Additional Configuration

### Add Deployment Stage

Add a deployment stage to your `azure-pipelines.yml`:

```yaml
- stage: Deploy
  displayName: 'Deploy to Azure'
  dependsOn: Build
  condition: succeeded()
  jobs:
  - deployment: DeployToAppService
    displayName: 'Deploy to Azure App Service'
    environment: 'Production'
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureWebApp@1
            inputs:
              azureSubscription: 'Your-Azure-Service-Connection'
              appName: 'Your-App-Service-Name'
              package: '$(Pipeline.Workspace)/drop/app/**/*.zip'
```

### Add SonarQube Integration

1. Install SonarQube extension
2. Add SonarQube tasks to pipeline
3. Configure quality gates

### Add Security Scanning

- Install WhiteSource or Snyk extension
- Add security scanning tasks
- Configure alert thresholds

## Best Practices

1. **Branch Protection**: Always protect your main branch
2. **Review Process**: Require code reviews before merging
3. **Automated Testing**: Never skip tests
4. **Code Coverage**: Maintain minimum coverage thresholds
5. **Security**: Regularly update dependencies
6. **Documentation**: Keep pipeline documentation updated

## Support

For issues or questions:
1. Check Azure DevOps documentation
2. Review pipeline logs
3. Consult with your DevOps team

---

**Last Updated**: 2024

