# DentalTrack - Claude Context Documentation

## 🎯 Project Overview

DentalTrack is a comprehensive dental practice management web application focused on teeth whitening treatments. The MVP provides dental professionals with tools to manage patients, track treatments, capture photos, and analyze progress using AI.

### 📋 Key Information
- **Project Name**: DentalTrack
- **Type**: Web Application (Responsive - no native mobile apps in MVP)
- **Target**: Dental practices specializing in teeth whitening
- **Architecture**: Clean Architecture (.NET 8 + React + Azure)
- **Timeline**: 7 sprints (5 months) - Currently in Sprint 0
- **Repository**: https://github.com/amajail/dentaltrack

## 🏗️ Technical Architecture

### Frontend
- **Framework**: React 19.1.0 + TypeScript
- **UI Library**: Material-UI (MUI) v7.2.0
- **Build Tool**: Vite 7.0.4
- **State Management**: React Query + React Context
- **HTTP Client**: Axios 1.11.0
- **Form Handling**: React Hook Form + Zod validation
- **Testing**: Vitest + React Testing Library

### Backend
- **Framework**: .NET 8 Web API
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, API)
- **ORM**: Entity Framework Core
- **Database**: Azure SQL Database
- **Authentication**: JWT + Google OAuth 2.0
- **Pattern**: CQRS with MediatR
- **Testing**: xUnit + Moq + FluentAssertions

### Cloud Infrastructure (Azure)
- **Compute**: Azure App Service (API) + Azure Static Web Apps (Frontend)
- **Database**: Azure SQL Database
- **Storage**: Azure Blob Storage (for dental photos)
- **AI**: Azure Cognitive Services (Computer Vision)
- **Security**: Azure Key Vault
- **Monitoring**: Application Insights

### DevOps & CI/CD
- **Source Control**: Git + GitHub
- **CI/CD**: GitHub Actions (4 workflows)
- **Quality Gates**: 80% code coverage, security scanning, performance monitoring
- **Deployment**: Automatic staging, manual production with approval
- **Infrastructure**: Infrastructure as Code with potential Terraform

## 🚀 Current Status (January 27, 2025)

### ✅ Completed User Stories
1. **US-001**: Configurar monorepo con estructura Clean Architecture ✅
2. **US-003**: Configurar GitHub Actions CI/CD pipeline ✅

### 🔄 In Progress / Ready
- **US-002**: Configurar GitHub repository y Projects (Ready for Development)
- **US-004**: Configurar infraestructura Azure completa (Ready for Development) 
- **US-004b**: Activar y validar CI/CD pipeline (Ready for Development)

### 📊 Sprint Progress
- **Sprint 0**: 50% completed (2/4 user stories done)
- **Next Priority**: Complete US-004b to activate CI/CD pipeline
- **Estimated Completion**: Sprint 0 by end of week

## 📁 Project Structure

```
dentaltrack/
├── 📁 backend/                 # .NET 8 Web API (Clean Architecture)
│   ├── src/
│   │   ├── DentalTrack.Api/           # Presentation Layer (Controllers, Middleware)
│   │   ├── DentalTrack.Application/   # Business Logic (Handlers, Commands, Queries)
│   │   ├── DentalTrack.Domain/        # Domain Entities & Value Objects
│   │   └── DentalTrack.Infrastructure/ # Data Access (EF Core, Repositories)
│   └── tests/                         # Unit & Integration Tests
├── 📁 frontend/                # React 19+ TypeScript App
│   ├── src/
│   │   ├── components/               # Reusable UI Components
│   │   ├── pages/                    # Application Pages/Routes
│   │   ├── api/                      # API Client & Types
│   │   ├── hooks/                    # Custom React Hooks
│   │   ├── context/                  # React Context Providers
│   │   └── theme/                    # Material-UI Theme
│   └── public/                       # Static Assets
├── 📁 docs/                    # Comprehensive Documentation
│   ├── user-stories/                # Epic-organized user stories
│   ├── sprints/                      # Sprint planning & tracking
│   ├── deployment/                   # CI/CD & deployment guides
│   └── CLAUDE.md                     # This context file
├── 📁 .github/                 # GitHub Actions & Templates
│   ├── workflows/                    # CI/CD workflows (4 workflows)
│   └── scripts/                      # Automation scripts
└── 📄 docker-compose.yml       # Local Development Stack
```

## 🔧 Development Workflow

### Daily Development
1. Create feature branch: `git checkout -b feature/US-xxx-description`
2. Implement with tests (maintain 80% coverage)
3. Create PR to main (Quality Gates run automatically)
4. Get 1 approval, merge (Staging deploys automatically)

### Quality Gates (All Required)
- ✅ **backend-ci**: .NET build, tests, 80% coverage
- ✅ **frontend-ci**: React build, lint, tests, bundle size <5MB
- ✅ **security-scan**: Trivy vulnerability scanning (0 critical)
- ✅ **code-quality**: Linting and formatting checks
- ✅ **security-analysis**: OWASP dependency check
- ✅ **performance-tests**: Bundle size and performance monitoring
- ✅ **coverage-analysis**: 80% coverage threshold validation

### Production Deployment
1. Create release tag: `git tag -a v1.0.0 -m "Release description"`
2. GitHub Actions > "Deploy to Production" > Manual approval required
3. Automated rollback on failure

## 🎯 Business Domain

### Core Entities
- **Patient**: Personal info, medical history, contact details
- **Treatment**: Whitening treatment plans, duration, progress tracking
- **Photo**: Before/during/after photos with metadata and quality scoring
- **Analysis**: AI-powered progress analysis and recommendations
- **User**: Dentist, dental assistant, admin roles

### Key Business Rules
- Focus on **teeth whitening treatments only** (MVP scope)
- **HIPAA compliance** considerations for patient data
- **Progress tracking** through photo comparison and AI analysis
- **Professional reporting** for dentist-patient communication

### User Roles
- **👨‍⚕️ Dentist**: Primary user, manages all aspects
- **👩‍💼 Dental Assistant**: Limited access, assists with data entry
- **👩‍💻 Admin**: System administration, user management

## 🛠️ Development Commands

### Backend (.NET 8)
```bash
cd backend
dotnet restore                    # Restore dependencies
dotnet build                     # Build solution
dotnet test                      # Run all tests
dotnet run --project src/DentalTrack.Api  # Start API
```

### Frontend (React + TypeScript)
```bash
cd frontend
npm install                      # Install dependencies
npm run dev                      # Start development server
npm run build                    # Production build
npm run lint                     # ESLint checking
npm run type-check               # TypeScript validation
npm run test                     # Run Vitest tests
npm run test:coverage            # Coverage report
```

### Full Stack Development
```bash
docker-compose up -d             # Start SQL Server database
# Run backend and frontend in separate terminals
```

### CI/CD Testing
```bash
./.github/scripts/test-ci-cd.sh  # Test CI/CD pipeline locally
./.github/scripts/setup-branch-protection.sh  # Configure branch protection
```

## 📊 Quality Standards

### Code Quality
- **Backend**: 80%+ test coverage, SOLID principles, Clean Architecture
- **Frontend**: 80%+ test coverage, TypeScript strict mode, ESLint compliance
- **Security**: Zero critical vulnerabilities, dependency scanning
- **Performance**: <2s page load, <200ms API response, <5MB bundle size

### Documentation Standards
- **Code**: TSDoc for TypeScript, XML docs for C#
- **Architecture**: Diagrams and decision records
- **API**: OpenAPI/Swagger documentation
- **User Stories**: Acceptance criteria with Claude prompts

## 🔒 Security Considerations

### Authentication & Authorization
- **Google OAuth 2.0** for user authentication
- **JWT tokens** for API authorization
- **Role-based access control** (RBAC)
- **Session management** with secure tokens

### Data Protection
- **HIPAA compliance** preparations for patient data
- **Azure Key Vault** for secrets management
- **Encrypted databases** and secure connections
- **Input validation** and sanitization

### Infrastructure Security
- **Azure Security Center** monitoring
- **Network Security Groups** for access control
- **SSL/TLS encryption** for all communications
- **Regular security scanning** in CI/CD pipeline

## 📈 Performance Requirements

### Frontend Performance
- **First Contentful Paint**: <1.5s
- **Largest Contentful Paint**: <2.5s
- **Time to Interactive**: <3s
- **Bundle Size**: <5MB total, <300KB JavaScript

### Backend Performance
- **API Response Time**: <200ms (95th percentile)
- **Database Query Time**: <100ms (95th percentile)
- **Memory Usage**: <512MB under normal load
- **Concurrent Users**: Support 100+ concurrent users

## 🧪 Testing Strategy

### Backend Testing
- **Unit Tests**: xUnit with Moq for mocking
- **Integration Tests**: TestContainers for database testing
- **API Tests**: WebApplicationFactory for controller testing
- **Coverage**: 80% minimum, focus on business logic

### Frontend Testing
- **Unit Tests**: Vitest for component and utility testing
- **Component Tests**: React Testing Library for UI testing
- **API Mocking**: MSW (Mock Service Worker) for API simulation
- **Coverage**: 80% minimum, focus on user interactions

### E2E Testing (Future)
- **Playwright** for full application testing
- **Cross-browser testing** (Chrome, Firefox, Safari, Edge)
- **Mobile responsive testing** on various screen sizes

## 🌍 Environment Configuration

### Development
- **API**: http://localhost:5000
- **Frontend**: http://localhost:3000
- **Database**: SQL Server via Docker (localhost:1433)

### Staging (Auto-deploy from main)
- **API**: https://dentaltrack-api-staging.azurewebsites.net
- **Frontend**: https://dentaltrack-web-staging.azurestaticapps.net
- **Database**: Azure SQL Database (staging)

### Production (Manual deployment)
- **API**: https://dentaltrack-api-prod.azurewebsites.net
- **Frontend**: https://www.dentaltrack.com (future)
- **Database**: Azure SQL Database (production)

## 📚 Key Documentation

### For Developers
- [📋 Project Overview](./project-overview.md)
- [🛠️ Technical Stack](./technical-stack.md) 
- [🚀 Deployment Guide](./deployment/README.md)
- [📝 User Stories](./user-stories/README.md)

### For DevOps
- [🔧 CI/CD Pipeline Documentation](./deployment/README.md)
- [☁️ Azure Infrastructure Guide](./deployment/azure-setup.md)
- [🛡️ Security Configuration](./deployment/security.md)

### For Product Management
- [🗺️ Product Roadmap](./roadmap.md)
- [📊 Sprint Planning](./sprints/)
- [📋 User Stories Backlog](./user-stories/README.md)

## 🎯 Next Steps (Priority Order)

### Immediate (This Week)
1. **Complete US-004b**: Activate CI/CD pipeline with Azure credentials
2. **Validate Pipeline**: Test full deployment flow to staging/production
3. **Team Training**: Ensure all developers understand new workflow

### Short Term (Next Sprint)
4. **Begin US-005**: Start backend API development with Clean Architecture
5. **US-008**: Parallel frontend React application setup
6. **US-015**: Authentication implementation planning

### Medium Term (Next Month)
7. **Patient Management**: Core CRUD operations (US-018, US-019)
8. **Photo Management**: Image capture and storage (US-021, US-022)
9. **Basic AI Integration**: Azure Cognitive Services setup (US-024)

## 🤖 Claude Development Notes

### When Working on DentalTrack
1. **Always maintain 80%+ test coverage** - this is enforced by CI/CD
2. **Follow Clean Architecture patterns** in backend development
3. **Use Material-UI components** for consistent responsive design
4. **Implement proper error handling** and user feedback
5. **Consider HIPAA compliance** for any patient data handling
6. **Test on mobile** - this is a mobile-first responsive application

### Code Style Preferences
- **TypeScript**: Strict mode, explicit types, no `any`
- **C#**: Modern C# features, async/await patterns, SOLID principles
- **Components**: Functional components with TypeScript interfaces
- **API**: RESTful design with clear error responses
- **Database**: EF Core with migrations, proper indexing

### Common Patterns
- **API State**: Use React Query for server state management
- **Form Handling**: React Hook Form + Zod validation
- **Error Handling**: Global error boundaries + specific error handling
- **Authentication**: JWT tokens with refresh token rotation
- **File Uploads**: Azure Blob Storage with secure URLs

## 📞 Support & Contacts

### Repository Information
- **GitHub**: https://github.com/amajail/dentaltrack
- **Issues**: Use GitHub Issues with provided templates
- **PRs**: Follow PR template and quality gates

### Development Resources
- **CI/CD Status**: GitHub Actions tab
- **Coverage Reports**: Codecov integration
- **Security Alerts**: GitHub Security tab
- **Performance**: Application Insights (when configured)

---

**📅 Last Updated**: January 27, 2025  
**📝 Maintained By**: DentalTrack Development Team  
**🔄 Version**: Sprint 0 - Foundation Phase