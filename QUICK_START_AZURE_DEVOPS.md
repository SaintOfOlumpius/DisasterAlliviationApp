# Quick Start: Azure DevOps Pipeline Setup

This guide will help you quickly set up your Azure DevOps pipeline for the Disaster Alleviation App.

## 🚀 Quick Setup (5 Minutes)

### Step 1: Create Azure DevOps Project

1. Go to [https://dev.azure.com](https://dev.azure.com)
2. Sign in or create a free account
3. Click **New project**
4. Enter project name: `DisasterAlleviationApp`
5. Choose visibility (Private recommended)
6. Click **Create**

### Step 2: Push Your Code

#### Option A: Import from Local Repository

```bash
# Navigate to your project directory
cd "/Users/saint/Desktop/Disaster_Alleviation_POE (4)"

# Initialize git if not already done
git init

# Add all files
git add .

# Commit
git commit -m "Initial commit with Azure DevOps pipeline"

# Get your Azure DevOps repository URL from Azure DevOps
# Go to Repos > Files > Clone > Copy the HTTPS URL

# Add remote and push
git remote add origin https://dev.azure.com/[YourOrganization]/[YourProject]/_git/[YourRepository]
git branch -M main
git push -u origin main
```

#### Option B: Connect GitHub Repository

1. Go to **Repos** > **Files** in Azure DevOps
2. Click **New repository** > **GitHub**
3. Authorize and select your GitHub repository
4. Azure DevOps will create a connection

### Step 3: Create Pipeline

1. Go to **Pipelines** > **Pipelines** in Azure DevOps
2. Click **New pipeline**
3. Select your repository source:
   - **Azure Repos Git** (for Azure Repos)
   - **GitHub** (for GitHub)
4. Select your repository
5. Choose **Existing Azure Pipelines YAML file**
6. Select branch: `main` (or `master`)
7. Select path: `azure-pipelines.yml`
8. Click **Continue**
9. Review the pipeline and click **Run**

### Step 4: Run the Pipeline

1. Click **Run pipeline** if not automatically started
2. Select branch: `main`
3. Click **Run**
4. Watch the pipeline execute!

## 📊 Viewing Results

### Test Results
1. Go to **Pipelines** > **Your Pipeline** > **Recent runs**
2. Click on a run
3. Go to **Tests** tab
4. See all test results, pass/fail counts, and execution time

### Code Coverage
1. In the pipeline run, go to **Code coverage** tab
2. View coverage by file, line, and branch
3. See coverage trends over time

### Build Artifacts
1. In the pipeline run, go to **Artifacts** tab
2. Download the published application package

## 🔧 Configuration Options

### Pipeline Variables

To configure pipeline variables:

1. Go to **Pipelines** > **Your Pipeline** > **Edit**
2. Click **Variables** (top right)
3. Add variables:
   - `MongoDBConnectionString` (Mark as secret)
   - `BuildConfiguration` (default: Release)
   - Any other configuration values

### Branch Policies

To protect your main branch:

1. Go to **Repos** > **Branches**
2. Right-click `main` > **Branch policies**
3. Enable:
   - ✅ **Build validation**: Require pipeline to succeed
   - ✅ **Status checks**: Require tests to pass
   - ✅ **Minimum reviewers**: 1-2 reviewers

## 📝 Pipeline Files Overview

Your project includes these pipeline files:

1. **`azure-pipelines.yml`** (Main pipeline - use this one)
   - Build and test stages
   - Code coverage reporting
   - Test result publishing

2. **`azure-pipelines-full.yml`** (Full-featured pipeline)
   - All features from main pipeline
   - Additional quality checks
   - Security scanning
   - Deployment stages (commented out)

3. **`.azuredevops/pipeline-templates/`** (Reusable templates)
   - Build and test template
   - Deployment template

## 🎯 What the Pipeline Does

### Build Stage
✅ Restores NuGet packages
✅ Builds the solution in Release mode
✅ Runs all unit tests
✅ Collects code coverage
✅ Publishes test results (TRX and JUnit formats)
✅ Publishes code coverage reports
✅ Creates build artifacts

### Quality Check Stage
✅ Code formatting checks
✅ Quality gate validations

### Security Scan Stage
✅ Dependency vulnerability scanning
✅ Security analysis

## 🐛 Troubleshooting

### Pipeline Fails on Test Step

**Issue**: Tests are failing

**Solution**:
1. Run tests locally: `dotnet test`
2. Fix failing tests
3. Commit and push changes
4. Pipeline will automatically re-run

### Code Coverage Not Showing

**Issue**: Code coverage tab is empty

**Solution**:
1. Verify `coverlet.collector` package is in test project
2. Check pipeline logs for coverage file paths
3. Ensure tests actually run (check Test tab)

### Build Fails on Restore

**Issue**: NuGet restore fails

**Solution**:
1. Check internet connectivity in pipeline
2. Verify package sources are accessible
3. Check for proxy settings if behind corporate firewall

### Pipeline Not Triggering

**Issue**: Pipeline doesn't run on push

**Solution**:
1. Check trigger configuration in `azure-pipelines.yml`
2. Verify branch name matches trigger branches
3. Check if `.azuredevops` folder is excluded (update excludes if needed)

## 📚 Additional Resources

- [Azure DevOps Documentation](https://docs.microsoft.com/azure/devops/)
- [YAML Pipeline Reference](https://docs.microsoft.com/azure/devops/pipelines/yaml-schema)
- [.NET Core Tasks](https://docs.microsoft.com/azure/devops/pipelines/tasks/build/dotnet-core-cli)

## ✅ Success Checklist

After setup, you should have:

- ✅ Pipeline runs automatically on push/PR
- ✅ Tests run and results are published
- ✅ Code coverage reports are generated
- ✅ Build artifacts are created
- ✅ Pipeline status appears in pull requests
- ✅ Email notifications on build completion (if configured)

## 🎉 You're All Set!

Your Azure DevOps pipeline is now configured and running. Every push to your main branch will:

1. Build your application
2. Run all tests
3. Generate code coverage reports
4. Publish test results
5. Create build artifacts

Happy coding! 🚀

