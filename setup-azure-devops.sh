#!/bin/bash

# Azure DevOps Repository Setup Script
# This script helps initialize and push your code to Azure DevOps

set -e

echo "🚀 Azure DevOps Repository Setup Script"
echo "========================================"
echo ""

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Check if git is installed
if ! command -v git &> /dev/null; then
    echo -e "${RED}❌ Git is not installed. Please install Git first.${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Git is installed${NC}"
echo ""

# Check if we're in a git repository
if [ -d .git ]; then
    echo -e "${YELLOW}⚠️  Git repository already initialized${NC}"
    echo ""
    read -p "Do you want to add a new remote? (y/n) " -n 1 -r
    echo ""
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        echo "Please enter your Azure DevOps repository URL:"
        echo "Example: https://dev.azure.com/YourOrganization/YourProject/_git/YourRepository"
        read -p "Repository URL: " repo_url
        
        if [ -z "$repo_url" ]; then
            echo -e "${RED}❌ Repository URL cannot be empty${NC}"
            exit 1
        fi
        
        read -p "Remote name (default: azure-devops): " remote_name
        remote_name=${remote_name:-azure-devops}
        
        git remote add $remote_name $repo_url
        echo -e "${GREEN}✅ Remote '$remote_name' added${NC}"
    fi
else
    echo "Initializing Git repository..."
    git init
    
    echo ""
    echo "Creating .gitignore..."
    # .gitignore should already exist, but ensure it's there
    if [ ! -f .gitignore ]; then
        echo "Warning: .gitignore not found"
    fi
    
    echo ""
    echo "Adding all files to Git..."
    git add .
    
    echo ""
    echo "Please enter your commit message:"
    read -p "Commit message (default: Initial commit with Azure DevOps pipeline): " commit_msg
    commit_msg=${commit_msg:-Initial commit with Azure DevOps pipeline}
    
    git commit -m "$commit_msg"
    echo -e "${GREEN}✅ Initial commit created${NC}"
    
    echo ""
    echo "Please enter your Azure DevOps repository URL:"
    echo "Example: https://dev.azure.com/YourOrganization/YourProject/_git/YourRepository"
    read -p "Repository URL: " repo_url
    
    if [ -z "$repo_url" ]; then
        echo -e "${RED}❌ Repository URL cannot be empty${NC}"
        exit 1
    fi
    
    git remote add origin $repo_url
    
    echo ""
    read -p "Branch name (default: main): " branch_name
    branch_name=${branch_name:-main}
    
    git branch -M $branch_name
    
    echo ""
    read -p "Do you want to push to Azure DevOps now? (y/n) " -n 1 -r
    echo ""
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        echo "Pushing to Azure DevOps..."
        git push -u origin $branch_name
        echo -e "${GREEN}✅ Code pushed to Azure DevOps${NC}"
    else
        echo "To push later, run:"
        echo "  git push -u origin $branch_name"
    fi
fi

echo ""
echo "========================================"
echo -e "${GREEN}✅ Setup complete!${NC}"
echo ""
echo "Next steps:"
echo "1. Go to Azure DevOps (https://dev.azure.com)"
echo "2. Navigate to your project"
echo "3. Go to Pipelines > Pipelines"
echo "4. Click 'New pipeline'"
echo "5. Select 'Existing Azure Pipelines YAML file'"
echo "6. Choose branch: $branch_name (or main/master)"
echo "7. Select path: azure-pipelines.yml"
echo "8. Click 'Run'"
echo ""
echo "For more details, see: QUICK_START_AZURE_DEVOPS.md"

