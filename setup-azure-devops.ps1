# Azure DevOps Repository Setup Script (PowerShell)
# This script helps initialize and push your code to Azure DevOps

Write-Host "🚀 Azure DevOps Repository Setup Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if git is installed
try {
    $gitVersion = git --version
    Write-Host "✅ Git is installed: $gitVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Git is not installed. Please install Git first." -ForegroundColor Red
    exit 1
}

Write-Host ""

# Check if we're in a git repository
if (Test-Path .git) {
    Write-Host "⚠️  Git repository already initialized" -ForegroundColor Yellow
    Write-Host ""
    $addRemote = Read-Host "Do you want to add a new remote? (y/n)"
    if ($addRemote -eq 'y' -or $addRemote -eq 'Y') {
        Write-Host "Please enter your Azure DevOps repository URL:"
        Write-Host "Example: https://dev.azure.com/YourOrganization/YourProject/_git/YourRepository"
        $repoUrl = Read-Host "Repository URL"
        
        if ([string]::IsNullOrWhiteSpace($repoUrl)) {
            Write-Host "❌ Repository URL cannot be empty" -ForegroundColor Red
            exit 1
        }
        
        $remoteName = Read-Host "Remote name (default: azure-devops)"
        if ([string]::IsNullOrWhiteSpace($remoteName)) {
            $remoteName = "azure-devops"
        }
        
        git remote add $remoteName $repoUrl
        Write-Host "✅ Remote '$remoteName' added" -ForegroundColor Green
    }
} else {
    Write-Host "Initializing Git repository..."
    git init
    
    Write-Host ""
    Write-Host "Checking .gitignore..."
    if (-not (Test-Path .gitignore)) {
        Write-Host "Warning: .gitignore not found" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "Adding all files to Git..."
    git add .
    
    Write-Host ""
    $commitMsg = Read-Host "Please enter your commit message (default: Initial commit with Azure DevOps pipeline)"
    if ([string]::IsNullOrWhiteSpace($commitMsg)) {
        $commitMsg = "Initial commit with Azure DevOps pipeline"
    }
    
    git commit -m $commitMsg
    Write-Host "✅ Initial commit created" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "Please enter your Azure DevOps repository URL:"
    Write-Host "Example: https://dev.azure.com/YourOrganization/YourProject/_git/YourRepository"
    $repoUrl = Read-Host "Repository URL"
    
    if ([string]::IsNullOrWhiteSpace($repoUrl)) {
        Write-Host "❌ Repository URL cannot be empty" -ForegroundColor Red
        exit 1
    }
    
    git remote add origin $repoUrl
    
    Write-Host ""
    $branchName = Read-Host "Branch name (default: main)"
    if ([string]::IsNullOrWhiteSpace($branchName)) {
        $branchName = "main"
    }
    
    git branch -M $branchName
    
    Write-Host ""
    $pushNow = Read-Host "Do you want to push to Azure DevOps now? (y/n)"
    if ($pushNow -eq 'y' -or $pushNow -eq 'Y') {
        Write-Host "Pushing to Azure DevOps..."
        git push -u origin $branchName
        Write-Host "✅ Code pushed to Azure DevOps" -ForegroundColor Green
    } else {
        Write-Host "To push later, run:"
        Write-Host "  git push -u origin $branchName"
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ Setup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Go to Azure DevOps (https://dev.azure.com)"
Write-Host "2. Navigate to your project"
Write-Host "3. Go to Pipelines > Pipelines"
Write-Host "4. Click 'New pipeline'"
Write-Host "5. Select 'Existing Azure Pipelines YAML file'"
Write-Host "6. Choose branch: $branchName (or main/master)"
Write-Host "7. Select path: azure-pipelines.yml"
Write-Host "8. Click 'Run'"
Write-Host ""
Write-Host "For more details, see: QUICK_START_AZURE_DEVOPS.md"

