# US-TD03: Docker Build Pipeline Implementation

## User Story
**As a** DevOps engineer  
**I want** Docker images to be built and pushed automatically in the CI pipeline  
**So that** we can deploy containerized applications consistently across environments

## Background
Docker build functionality was temporarily removed from the CI pipeline (`ab33248`) to resolve immediate startup failures. The infrastructure exists (Dockerfiles, docker-compose files) but needs to be re-integrated into the automated pipeline.

### What was removed
- Docker build job in `.github/workflows/ci-cd.yml` (lines 146-152, now commented out)
- Docker Hub login and multi-platform image builds
- Dependency on Docker build from security-scan job

## Acceptance Criteria

### Prerequisites
- [ ] Set up Docker Hub account for the project
- [ ] Configure GitHub repository secrets:
  - `DOCKERHUB_USERNAME` - Docker Hub username
  - `DOCKERHUB_TOKEN` - Docker Hub access token (not password)

### Implementation Requirements
- [ ] **Backend Docker Image**: Multi-stage build that runs tests and creates production image
  - Uses `backend/Dockerfile.ci` 
  - .NET 8 Web API with health checks
  - Runs tests during build process
- [ ] **Frontend Docker Image**: Multi-stage build with linting, type-checking, and static file serving
  - Uses `frontend/Dockerfile.ci`
  - React + TypeScript + Vite build
  - Nginx serving static files with `nginx.ci.conf`
- [ ] **Multi-platform Support**: Build for linux/amd64 and linux/arm64 architectures
- [ ] **Image Tagging Strategy**: Use git commit SHA, branch name, and 'latest' tags
- [ ] **Security Scanning**: Include Docker images in vulnerability scans
- [ ] **CI Integration**: Docker build job runs after successful tests and security scans

### Quality Gates
- [ ] Docker builds complete successfully without errors
- [ ] Images are properly tagged and pushed to registry
- [ ] Build artifacts include image digest and vulnerability scan results
- [ ] Images pass health check validations
- [ ] Documentation includes Docker deployment instructions

## Technical Details

### Files Involved
- `.github/workflows/ci-cd.yml` (lines 146-152 currently commented out)
- `backend/Dockerfile.ci` - Backend multi-stage build configuration
- `frontend/Dockerfile.ci` - Frontend build with nginx configuration
- `frontend/nginx.ci.conf` - Nginx configuration for serving React app
- `docker-compose.ci.yml` - Local testing configuration with all services

### Current State
- ✅ Docker build job is commented out in CI pipeline
- ✅ Dockerfile configurations exist and are functional
- ✅ Local docker-compose setup available for testing
- ✅ CI pipeline works without Docker dependency

### Docker Images to Build
1. **dentaltrack-api** (Backend)
   - Base: `mcr.microsoft.com/dotnet/aspnet:8.0`
   - Multi-stage build with SDK for compilation
   - Includes health check endpoint
   - Runs tests during build

2. **dentaltrack-web** (Frontend)
   - Base: `node:20-alpine` for build, `nginx:alpine` for runtime
   - Includes linting and type checking
   - Optimized production build
   - Health check via nginx

### Implementation Plan
1. **Phase 1**: Set up Docker Hub repository and secrets
2. **Phase 2**: Re-enable docker-build job in CI pipeline
3. **Phase 3**: Configure proper tagging and multi-platform builds
4. **Phase 4**: Add Docker image security scanning
5. **Phase 5**: Update deployment workflows to use Docker images

## Definition of Done
- [ ] CI pipeline builds and pushes Docker images on main branch
- [ ] Images are available in Docker Hub repository with proper tags
- [ ] Security scans include Docker vulnerability assessment
- [ ] Team can deploy using Docker images locally and in staging
- [ ] Documentation updated with container deployment steps
- [ ] No regression in CI pipeline performance or reliability

## Story Points
**5 points** - Medium complexity involving CI/CD configuration, external service integration, and testing

## Priority
**Medium** - Technical debt that should be addressed before production deployment but doesn't block current development

## Dependencies
- Requires Docker Hub account setup
- May need infrastructure team coordination for registry access
- Should be implemented after current CI stability is confirmed

## Related Issues
- Original issue: CI pipeline startup failures resolved in commit `ab33248`
- Future enhancement: Container deployment to Azure Container Instances
- Security consideration: Docker image vulnerability scanning integration

## Implementation Notes
- The Docker infrastructure is already built and tested locally
- Only requires secrets configuration and pipeline re-enablement
- Should maintain current CI pipeline stability and performance
- Consider implementing gradual rollout (main branch only initially)

## Testing Strategy
- Test Docker builds locally using `docker-compose.ci.yml`
- Verify multi-platform builds work correctly
- Ensure security scanning covers Docker images
- Validate image deployment in staging environment
- Confirm no impact on existing CI pipeline performance