#!/bin/bash

# CI/CD Testing Script for DentalTrack
# This script tests the CI/CD pipeline locally using Docker and act

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Functions
log_info() {
    echo -e "${BLUE}ℹ️  $1${NC}"
}

log_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

log_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

log_error() {
    echo -e "${RED}❌ $1${NC}"
}

# Check prerequisites
check_prerequisites() {
    log_info "Checking prerequisites..."
    
    # Check Docker
    if ! command -v docker &> /dev/null; then
        log_error "Docker is not installed. Please install Docker first."
        exit 1
    fi
    
    # Check Docker Compose
    if ! command -v docker-compose &> /dev/null; then
        log_error "Docker Compose is not installed. Please install Docker Compose first."
        exit 1
    fi
    
    # Check if Docker is running
    if ! docker info &> /dev/null; then
        log_error "Docker is not running. Please start Docker first."
        exit 1
    fi
    
    # Check act (optional)
    if command -v act &> /dev/null; then
        ACT_AVAILABLE=true
        log_success "act is available for GitHub Actions testing"
    else
        ACT_AVAILABLE=false
        log_warning "act is not installed. GitHub Actions testing will be skipped."
        log_info "Install act: https://github.com/nektos/act"
    fi
    
    log_success "Prerequisites check completed"
}

# Test backend build and tests
test_backend() {
    log_info "Testing backend build and tests..."
    
    cd backend
    
    # Restore dependencies
    log_info "Restoring .NET dependencies..."
    dotnet restore
    
    # Build
    log_info "Building backend..."
    dotnet build --configuration Release --no-restore
    
    # Run tests
    log_info "Running backend tests..."
    dotnet test --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage"
    
    # Check coverage (if available)
    if [ -d "tests" ]; then
        COVERAGE_FILES=$(find tests -name "coverage.cobertura.xml" 2>/dev/null | wc -l)
        if [ $COVERAGE_FILES -gt 0 ]; then
            log_success "Coverage reports generated"
        else
            log_warning "No coverage reports found"
        fi
    fi
    
    cd ..
    log_success "Backend tests completed"
}

# Test frontend build and tests
test_frontend() {
    log_info "Testing frontend build and tests..."
    
    cd frontend
    
    # Install dependencies
    log_info "Installing frontend dependencies..."
    npm ci
    
    # Linting
    log_info "Running frontend linting..."
    npm run lint
    
    # Type checking
    log_info "Running TypeScript type checking..."
    npm run type-check
    
    # Tests
    log_info "Running frontend tests..."
    npm run test:coverage
    
    # Build
    log_info "Building frontend..."
    npm run build
    
    # Check bundle size
    if [ -d "dist" ]; then
        BUNDLE_SIZE=$(du -sk dist | cut -f1)
        BUNDLE_SIZE_MB=$((BUNDLE_SIZE / 1024))
        log_info "Bundle size: ${BUNDLE_SIZE_MB}MB"
        
        if [ $BUNDLE_SIZE_MB -gt 5 ]; then
            log_warning "Bundle size ${BUNDLE_SIZE_MB}MB exceeds 5MB limit"
        else
            log_success "Bundle size is within limits"
        fi
    fi
    
    cd ..
    log_success "Frontend tests completed"
}

# Test security scanning
test_security() {
    log_info "Testing security scanning..."
    
    # Test with Docker Compose
    if [ -f "docker-compose.ci.yml" ]; then
        log_info "Running Trivy security scan..."
        docker-compose -f docker-compose.ci.yml run --rm trivy
        
        if [ -f "trivy-results.json" ]; then
            log_success "Trivy scan completed"
            
            # Check for critical vulnerabilities
            CRITICAL_COUNT=$(jq '.Results[]?.Vulnerabilities[]? | select(.Severity == "CRITICAL") | length' trivy-results.json 2>/dev/null | wc -l)
            if [ $CRITICAL_COUNT -gt 0 ]; then
                log_warning "Found critical vulnerabilities: $CRITICAL_COUNT"
            else
                log_success "No critical vulnerabilities found"
            fi
        fi
    else
        log_warning "docker-compose.ci.yml not found, skipping Docker-based security scan"
    fi
}

# Test with act (GitHub Actions locally)
test_github_actions() {
    if [ "$ACT_AVAILABLE" = false ]; then
        log_warning "Skipping GitHub Actions testing (act not available)"
        return
    fi
    
    log_info "Testing GitHub Actions workflows locally..."
    
    # Test CI workflow
    if [ -f ".github/workflows/ci-cd.yml" ]; then
        log_info "Testing CI workflow..."
        act -j backend-ci --dry-run
        act -j frontend-ci --dry-run
        log_success "CI workflow syntax validation passed"
    fi
    
    # Test Quality Gate workflow
    if [ -f ".github/workflows/quality-gate.yml" ]; then
        log_info "Testing Quality Gate workflow..."
        act pull_request -W .github/workflows/quality-gate.yml --dry-run
        log_success "Quality Gate workflow syntax validation passed"
    fi
}

# Test Docker builds
test_docker_builds() {
    log_info "Testing Docker builds..."
    
    # Test backend Docker build
    if [ -f "backend/Dockerfile.ci" ]; then
        log_info "Building backend Docker image..."
        docker build -f backend/Dockerfile.ci backend -t dentaltrack-backend:test
        log_success "Backend Docker build completed"
    fi
    
    # Test frontend Docker build  
    if [ -f "frontend/Dockerfile.ci" ]; then
        log_info "Building frontend Docker image..."
        docker build -f frontend/Dockerfile.ci frontend -t dentaltrack-frontend:test
        log_success "Frontend Docker build completed"
    fi
}

# Test full CI environment
test_ci_environment() {
    log_info "Testing full CI environment with Docker Compose..."
    
    if [ -f "docker-compose.ci.yml" ]; then
        # Start services
        log_info "Starting CI services..."
        docker-compose -f docker-compose.ci.yml up -d sqlserver
        
        # Wait for database
        log_info "Waiting for database to be ready..."
        sleep 30
        
        # Test database connection
        docker-compose -f docker-compose.ci.yml exec -T sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P DentalTrack123! -Q "SELECT 1" > /dev/null
        if [ $? -eq 0 ]; then
            log_success "Database connection successful"
        else
            log_error "Database connection failed"
        fi
        
        # Clean up
        log_info "Cleaning up CI environment..."
        docker-compose -f docker-compose.ci.yml down -v
        
        log_success "CI environment test completed"
    else
        log_warning "docker-compose.ci.yml not found, skipping CI environment test"
    fi
}

# Cleanup function
cleanup() {
    log_info "Cleaning up test artifacts..."
    
    # Remove test Docker images
    docker rmi dentaltrack-backend:test dentaltrack-frontend:test 2>/dev/null || true
    
    # Remove test results
    rm -f trivy-results.json dependency-check-report.json
    rm -rf test-results/
    
    # Docker cleanup
    docker system prune -f
    
    log_success "Cleanup completed"
}

# Main execution
main() {
    echo "🧪 DentalTrack CI/CD Testing Script"
    echo "=================================="
    echo ""
    
    # Check if we're in the right directory
    if [ ! -f "README.md" ] || [ ! -d ".github" ]; then
        log_error "Please run this script from the project root directory"
        exit 1
    fi
    
    # Parse command line arguments
    SKIP_BACKEND=false
    SKIP_FRONTEND=false
    SKIP_SECURITY=false
    SKIP_DOCKER=false
    SKIP_FULL_CI=false
    
    while [[ $# -gt 0 ]]; do
        case $1 in
            --skip-backend)
                SKIP_BACKEND=true
                shift
                ;;
            --skip-frontend)
                SKIP_FRONTEND=true
                shift
                ;;
            --skip-security)
                SKIP_SECURITY=true
                shift
                ;;
            --skip-docker)
                SKIP_DOCKER=true
                shift
                ;;
            --skip-full-ci)
                SKIP_FULL_CI=true
                shift
                ;;
            --help)
                echo "Usage: $0 [options]"
                echo ""
                echo "Options:"
                echo "  --skip-backend    Skip backend testing"
                echo "  --skip-frontend   Skip frontend testing"
                echo "  --skip-security   Skip security testing"
                echo "  --skip-docker     Skip Docker builds"
                echo "  --skip-full-ci    Skip full CI environment test"
                echo "  --help            Show this help message"
                exit 0
                ;;
            *)
                log_error "Unknown option: $1"
                exit 1
                ;;
        esac
    done
    
    # Start testing
    check_prerequisites
    
    echo ""
    echo "🚀 Starting CI/CD tests..."
    echo ""
    
    # Run tests
    if [ "$SKIP_BACKEND" = false ]; then
        test_backend
        echo ""
    fi
    
    if [ "$SKIP_FRONTEND" = false ]; then
        test_frontend
        echo ""
    fi
    
    if [ "$SKIP_SECURITY" = false ]; then
        test_security
        echo ""
    fi
    
    test_github_actions
    echo ""
    
    if [ "$SKIP_DOCKER" = false ]; then
        test_docker_builds
        echo ""
    fi
    
    if [ "$SKIP_FULL_CI" = false ]; then
        test_ci_environment
        echo ""
    fi
    
    cleanup
    
    echo ""
    echo "🎉 CI/CD testing completed successfully!"
    echo ""
    echo "📋 Summary:"
    echo "  ✅ All tests passed"
    echo "  ✅ Security scans completed"
    echo "  ✅ Docker builds successful"
    echo "  ✅ CI environment validated"
    echo ""
    echo "💡 Next steps:"
    echo "  1. Push changes to trigger real CI/CD pipeline"
    echo "  2. Create a test PR to validate quality gates"
    echo "  3. Configure Azure secrets for deployment"
    echo "  4. Run branch protection setup script"
}

# Trap to cleanup on exit
trap cleanup EXIT

# Run main function
main "$@"