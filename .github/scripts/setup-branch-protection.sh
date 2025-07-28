#!/bin/bash

# Setup Branch Protection Rules for DentalTrack
# This script configures branch protection rules for the main branch
# Requires GitHub CLI (gh) to be installed and authenticated

set -e

REPO_OWNER="amajail"
REPO_NAME="dentaltrack"
BRANCH="main"

echo "🔒 Setting up branch protection rules for ${REPO_OWNER}/${REPO_NAME}:${BRANCH}"

# Check if gh CLI is installed and authenticated
if ! command -v gh &> /dev/null; then
    echo "❌ GitHub CLI (gh) is not installed. Please install it first:"
    echo "   https://cli.github.com/"
    exit 1
fi

# Check authentication
if ! gh auth status &> /dev/null; then
    echo "❌ GitHub CLI is not authenticated. Please run 'gh auth login'"
    exit 1
fi

echo "✅ GitHub CLI is authenticated and ready"

# Configure branch protection rules
echo "📋 Configuring branch protection rules..."

gh api repos/${REPO_OWNER}/${REPO_NAME}/branches/${BRANCH}/protection \
  --method PUT \
  --field required_status_checks='{
    "strict": true,
    "contexts": [
      "backend-ci",
      "frontend-ci", 
      "security-scan",
      "code-quality",
      "security-analysis",
      "performance-tests",
      "coverage-analysis"
    ]
  }' \
  --field enforce_admins=true \
  --field required_pull_request_reviews='{
    "required_approving_review_count": 1,
    "dismiss_stale_reviews": true,
    "require_code_owner_reviews": false
  }' \
  --field restrictions=null \
  --field required_linear_history=false \
  --field allow_force_pushes=false \
  --field allow_deletions=false \
  --field required_conversation_resolution=true

echo "✅ Branch protection rules configured successfully!"

echo ""
echo "📋 Summary of protection rules for ${BRANCH} branch:"
echo "   ✅ Require status checks to pass before merging"
echo "   ✅ Require branches to be up to date before merging"
echo "   ✅ Required status checks:"
echo "      - backend-ci (from CI pipeline)"
echo "      - frontend-ci (from CI pipeline)"
echo "      - security-scan (from CI pipeline)"
echo "      - code-quality (from Quality Gate)"
echo "      - security-analysis (from Quality Gate)"
echo "      - performance-tests (from Quality Gate)"
echo "      - coverage-analysis (from Quality Gate)"
echo "   ✅ Require pull request reviews (minimum 1 approval)"
echo "   ✅ Dismiss stale reviews when new commits are pushed"
echo "   ✅ Require conversation resolution before merging"
echo "   ✅ Enforce restrictions for administrators"
echo "   ✅ Prohibit force pushes"
echo "   ✅ Prohibit branch deletion"

echo ""
echo "🎯 Next steps:"
echo "   1. Create test PR to verify protection rules"
echo "   2. Update team about new workflow requirements"
echo "   3. Document CI/CD process for the team"

echo ""
echo "⚠️  Important notes:"
echo "   - All PRs now require approval before merging"
echo "   - All status checks must pass before merging"
echo "   - Force pushes are disabled for security"
echo "   - Administrators are also subject to these rules"