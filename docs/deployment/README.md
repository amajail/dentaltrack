# DentalTrack Deployment Guide

This document describes the CI/CD pipeline and deployment process for DentalTrack.

## 🔄 CI/CD Overview

DentalTrack uses GitHub Actions for automated CI/CD with three main workflows:

1. **CI Pipeline** (`ci-cd.yml`) - Continuous Integration
2. **Staging Deployment** (`cd-staging.yml`) - Auto-deployment to staging
3. **Production Deployment** (`cd-production.yml`) - Manual deployment to production
4. **Quality Gate** (`quality-gate.yml`) - PR validation

## 📋 Workflow Details

### CI Pipeline (ci-cd.yml)

**Triggers:** Push to `main`/`develop`, Pull Requests to `main`

**Jobs:**
- `backend-ci`: .NET build, test, coverage analysis (80% threshold)
- `frontend-ci`: TypeScript build, lint, test, bundle size check (5MB limit)
- `security-scan`: Trivy vulnerability scanning, OWASP dependency check
- `docker-build`: Docker image build and push (when configured)

**Quality Gates:**
- ✅ 80% code coverage required
- ✅ No critical security vulnerabilities
- ✅ Bundle size under 5MB
- ✅ All tests pass
- ✅ Linting and type checking pass

### Staging Deployment (cd-staging.yml)

**Triggers:** Push to `main` branch (automatic)

**Jobs:**
- `deploy-backend`: Deploy API to Azure Web App (staging)
- `deploy-frontend`: Deploy web app to Azure Static Web Apps (staging)
- `smoke-tests`: Basic functionality validation
- `rollback-on-failure`: Automatic rollback on deployment failure

**Environment:** `staging`

### Production Deployment (cd-production.yml)

**Triggers:** Manual workflow dispatch only

**Features:**
- ✅ Manual approval required (production environment protection)
- ✅ Version selection (git tag or commit SHA)
- ✅ Pre-deployment validation
- ✅ Comprehensive smoke tests
- ✅ Automatic rollback on failure
- ✅ Incident management integration

**Environment:** `production`

### Quality Gate (quality-gate.yml)

**Triggers:** Pull Requests to `main`

**Checks:**
- Code quality (linting, formatting)
- Security analysis (vulnerability scanning)
- Performance tests (bundle size, metrics)
- Coverage analysis (80% threshold)
- Automated PR status reporting

## 🚀 Deployment Process

### Development Workflow

1. **Feature Development**
   ```bash
   git checkout -b feature/new-feature
   # Make changes
   git commit -m "feat: add new feature"
   git push origin feature/new-feature
   ```

2. **Create Pull Request**
   - Quality Gate workflow runs automatically
   - All checks must pass before merging
   - Requires 1 approval from team member

3. **Merge to Main**
   - Triggers CI Pipeline
   - Automatic deployment to staging
   - Smoke tests validate deployment

### Production Deployment

1. **Prepare for Production**
   - Ensure staging is stable
   - Create git tag for release
   ```bash
   git tag -a v1.0.0 -m "Release v1.0.0"
   git push origin v1.0.0
   ```

2. **Trigger Production Deployment**
   - Go to GitHub Actions → "Deploy to Production"
   - Click "Run workflow"
   - Enter version (tag or commit SHA)
   - Manual approval required before deployment

3. **Monitor Deployment**
   - Check deployment logs
   - Verify smoke tests pass
   - Monitor application health

## 🔒 Branch Protection Rules

The `main` branch is protected with the following rules:

- ✅ Require status checks to pass before merging
- ✅ Require branches to be up to date before merging
- ✅ Require pull request reviews (minimum 1 approval)
- ✅ Dismiss stale reviews when new commits are pushed
- ✅ Require conversation resolution before merging
- ✅ Enforce restrictions for administrators
- ✅ Prohibit force pushes and branch deletion

**Required Status Checks:**
- `backend-ci`
- `frontend-ci`
- `security-scan`
- `code-quality`
- `security-analysis`
- `performance-tests`
- `coverage-analysis`

## 🛠️ Setup Instructions

### Initial Setup

1. **Configure Secrets**
   
   Add these secrets in GitHub repository settings:
   ```
   AZURE_WEBAPP_PUBLISH_PROFILE_STAGING
   AZURE_WEBAPP_PUBLISH_PROFILE_PROD
   AZURE_STATIC_WEB_APPS_API_TOKEN_STAGING
   AZURE_STATIC_WEB_APPS_API_TOKEN_PROD
   CODECOV_TOKEN (optional)
   DOCKERHUB_USERNAME (optional)
   DOCKERHUB_TOKEN (optional)
   ```

2. **Configure Branch Protection**
   ```bash
   # Run the setup script
   ./.github/scripts/setup-branch-protection.sh
   ```

3. **Configure Environments**
   
   Create environments in GitHub repository settings:
   - `staging` - No protection rules
   - `production` - Require reviewers, deployment branches: main only

### Environment Variables

**CI Pipeline:**
- `DOTNET_VERSION`: .NET version (8.0.x)
- `NODE_VERSION`: Node.js version (20.x)
- `COVERAGE_THRESHOLD`: Required coverage percentage (80)

**Staging Deployment:**
- `AZURE_WEBAPP_NAME`: Staging web app name
- `AZURE_STATICWEBAPP_NAME`: Staging static web app name

**Production Deployment:**
- `AZURE_WEBAPP_NAME`: Production web app name  
- `AZURE_STATICWEBAPP_NAME`: Production static web app name

## 📊 Monitoring and Notifications

### Status Badges

Add these badges to README.md:

```markdown
![CI](https://github.com/amajail/dentaltrack/workflows/CI%20Pipeline/badge.svg)
![Coverage](https://codecov.io/gh/amajail/dentaltrack/branch/main/graph/badge.svg)
![Security](https://github.com/amajail/dentaltrack/workflows/Security%20Scan/badge.svg)
```

### Deployment Notifications

- ✅ PR comments with quality gate results
- ✅ Automatic GitHub issues for deployment failures
- ✅ Deployment status in workflow logs

### Health Monitoring

**Staging:**
- API health endpoint: `https://dentaltrack-api-staging.azurewebsites.net/health`
- Basic functionality tests

**Production:**
- API health endpoint: `https://dentaltrack-api-prod.azurewebsites.net/health`
- Extended smoke tests
- Critical endpoint validation

## 🚨 Troubleshooting

### Common Issues

1. **Coverage Below Threshold**
   - Add more unit tests
   - Check test configuration
   - Review coverage report

2. **Security Vulnerabilities**
   - Update dependencies
   - Review security scan results
   - Apply security patches

3. **Bundle Size Too Large**
   - Optimize imports
   - Use dynamic imports
   - Analyze bundle composition

4. **Deployment Failures**
   - Check deployment logs
   - Verify Azure credentials
   - Test deployment manually

### Rollback Procedures

**Staging:**
- Automatic rollback on failure
- Manual rollback via Azure portal

**Production:**
- Automatic rollback on smoke test failure
- Manual rollback via Azure deployment slots
- Database rollback (if migrations applied)

## 📚 Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Azure Deployment Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [DentalTrack Architecture Guide](../architecture/README.md)
- [Contributing Guidelines](../CONTRIBUTING.md)