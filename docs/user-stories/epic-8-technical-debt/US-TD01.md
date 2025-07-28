# US-TD01: Improve test coverage to 80% (Backend & Frontend)

## 📋 User Story
**Como** desarrollador del equipo  
**Quiero** incrementar la cobertura de tests a 80% en backend y frontend  
**Para que** tengamos una base de código más confiable y mantenible

## ✅ Criterios de Aceptación

### 🧪 Backend Coverage (Current: ~35% → Target: 80%)
- [ ] Unit tests para Domain entities y value objects
- [ ] Application layer tests (Handlers, Commands, Queries)  
- [ ] Infrastructure layer tests (Repositories, Data access)
- [ ] Integration tests para API controllers
- [ ] Validation tests para DTOs y requests
- [ ] Error handling and edge cases coverage

### ⚛️ Frontend Coverage (Current: 55.8% → Target: 80%)
- [ ] Component unit tests con React Testing Library
- [ ] Custom hooks testing con renderHook
- [ ] API client and service layer tests
- [ ] Context providers and state management tests
- [ ] Error boundary and error handling tests
- [ ] Integration tests para user workflows

### 📊 Quality Metrics
- [ ] Backend line coverage ≥ 80%
- [ ] Frontend line coverage ≥ 80%
- [ ] Branch coverage ≥ 75%
- [ ] Function coverage ≥ 80%
- [ ] Mutation testing score ≥ 70% (future enhancement)

## 🛠️ Technical Implementation

### Backend Test Strategy
```csharp
// Domain Tests
- PatientTests.cs: Entity validation, business rules
- TreatmentTests.cs: Treatment lifecycle, calculations
- PhotoMetadataTests.cs: Value object validation

// Application Tests  
- CreatePatientHandlerTests.cs: Command handling
- GetPatientsQueryHandlerTests.cs: Query execution
- PatientServiceTests.cs: Business logic

// Infrastructure Tests
- PatientRepositoryTests.cs: Data access
- DbContextTests.cs: Entity configurations
- UnitOfWorkTests.cs: Transaction handling

// Integration Tests
- PatientsControllerTests.cs: API endpoints
- AuthenticationTests.cs: Security middleware
```

### Frontend Test Strategy
```typescript
// Component Tests
- App.test.tsx ✅ (100% coverage)
- DashboardPage.test.tsx: Dashboard functionality
- AppLayout.test.tsx: Navigation and layout
- ErrorBoundary.test.tsx: Error handling

// Hook Tests
- useApi.test.ts: API interaction patterns
- useLocalStorage.test.ts: Persistence logic

// Service Tests
- apiClient.test.ts: HTTP client behavior
- QueryProvider.test.tsx: React Query setup

// Integration Tests
- userWorkflows.test.tsx: Complete user journeys
- formSubmission.test.tsx: Form validation flows
```

## 📝 Implementation Plan

### Phase 1: Foundation (1 week)
- [ ] Setup test utilities and helpers
- [ ] Configure test databases (TestContainers)
- [ ] Create component test utilities (render helpers)
- [ ] Establish testing patterns and conventions

### Phase 2: Domain & Core Logic (1 week)  
- [ ] Write comprehensive domain entity tests
- [ ] Add application service layer tests
- [ ] Test critical business logic paths
- [ ] Cover validation and error scenarios

### Phase 3: Infrastructure & Integration (1 week)
- [ ] Add repository and data access tests
- [ ] Test API endpoints and middleware
- [ ] Add authentication and authorization tests
- [ ] Test database migrations and seeding

### Phase 4: Frontend Components (1 week)
- [ ] Test all React components
- [ ] Add custom hook tests
- [ ] Test state management and context
- [ ] Cover error handling scenarios

### Phase 5: Integration & E2E (1 week)
- [ ] Add frontend integration tests
- [ ] Test API client and service layer
- [ ] Add user workflow tests
- [ ] Performance and accessibility tests

## 🎯 Current Coverage Analysis

### Backend Files Needing Tests
```
Low Coverage (<50%):
- Domain/ValueObjects/* (0% coverage)
- Application/Services/* (25% coverage) 
- Infrastructure/Repositories/* (40% coverage)
- Api/Controllers/* (45% coverage)

Medium Coverage (50-70%):
- Application/Handlers/* (60% coverage)
- Infrastructure/Data/* (65% coverage)

Good Coverage (>70%):
- Domain/Entities/* (85% coverage)
```

### Frontend Files Needing Tests
```
No Coverage (0%):
- src/main.tsx
- src/api/client.ts  
- src/hooks/useApi.ts
- src/providers/QueryProvider.tsx

Low Coverage (<50%):
- src/context/AppContext.tsx (16.54%)
- src/components/common/LoadingSpinner.tsx (0%)
- src/components/common/NotificationSystem.tsx (53.65%)

Good Coverage (>90%):
- src/App.tsx (100%) ✅
- src/pages/DashboardPage.tsx (100%) ✅
- src/components/common/ErrorBoundary.tsx (92.77%) ✅
- src/components/layout/AppLayout.tsx (98.03%) ✅
```

## 📋 Definition of Done
- [ ] All new code has ≥80% test coverage
- [ ] CI/CD pipeline enforces coverage thresholds
- [ ] Test execution time <5 minutes total
- [ ] All tests pass consistently in CI environment
- [ ] Coverage reports integrated in PR reviews
- [ ] Testing documentation updated
- [ ] Code review includes test quality assessment

## 🔗 Dependencies
- **Blocks**: Production deployment (quality gate)
- **Enables**: Confident refactoring and feature development
- **Requires**: Testing infrastructure setup

## 📊 Success Metrics
- **Coverage Increase**: Backend 35%→80%, Frontend 55.8%→80%
- **Bug Reduction**: 50% fewer production issues
- **Development Velocity**: Faster feature development with confidence
- **Refactoring Safety**: Ability to refactor without fear

---

📅 **Creado**: 2025-07-28  
🎯 **Sprint**: Sprint 3 (Technical Debt)  
👤 **Asignado**: Full Stack Team  
🔄 **Estado**: 📋 Ready for Development  
⏰ **Estimación**: XL (13 puntos)  
🏷️ **Labels**: technical-debt, testing, quality, backend, frontend