# 🛠️ DentalTrack - Technical Stack

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                       │
├─────────────────────────────────────────────────────────────┤
│  React 18+ Web App (Responsive)                            │
│  • TypeScript                                              │
│  • Material-UI                                             │
│  • React Query                                             │
│  • Vite                                                    │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼ HTTPS/REST API
┌─────────────────────────────────────────────────────────────┐
│                    APPLICATION LAYER                        │
├─────────────────────────────────────────────────────────────┤
│  .NET 8 Web API (Clean Architecture)                       │
│  • ASP.NET Core                                            │
│  • MediatR (CQRS)                                          │
│  • AutoMapper                                              │
│  • FluentValidation                                        │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                     DATA LAYER                             │
├─────────────────────────────────────────────────────────────┤
│  • Entity Framework Core                                   │
│  • Azure SQL Database                                      │
│  • Azure Blob Storage                                      │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                  EXTERNAL SERVICES                         │
├─────────────────────────────────────────────────────────────┤
│  • Google OAuth 2.0                                        │
│  • Azure Cognitive Services                                │
│  • Azure Application Insights                              │
└─────────────────────────────────────────────────────────────┘
```

## 🖥️ Frontend Stack

### ⚛️ React 18+ Web Application

#### Core Framework
```typescript
- React 18.2+
- TypeScript 5.0+
- Vite 5.0+ (Build tool)
- ESLint + Prettier (Code quality)
```

#### UI & Styling
```typescript
- Material-UI (MUI) 5.14+
- Emotion (CSS-in-JS)
- Material Icons
- Responsive Grid System
- Theme Provider (Light/Dark modes)
```

#### State Management
```typescript
- React Query (Server state)
- React Context (Global state)
- React Hook Form (Form state)
- Zustand (Client state - if needed)
```

#### Routing & Navigation
```typescript
- React Router DOM 6.0+
- Protected Routes
- Lazy Loading
- Browser History
```

#### HTTP Client
```typescript
- Axios
- Interceptors for auth
- Error handling
- Request/Response transformers
```

#### Testing
```typescript
- Vitest (Test runner)
- React Testing Library
- MSW (Mock Service Worker)
- Playwright (E2E - future)
```

### 📱 Responsive Design Requirements

#### Breakpoints
```css
- Mobile: 320px - 767px
- Tablet: 768px - 1023px  
- Desktop: 1024px+
- Large Desktop: 1440px+
```

#### Design System
```typescript
- Material Design 3.0
- Consistent spacing (8px grid)
- Typography scale
- Color palette (primary, secondary, etc.)
- Component library
```

## 🔧 Backend Stack

### 🏗️ .NET 8 Web API (Clean Architecture)

#### Core Framework
```csharp
- .NET 8.0
- ASP.NET Core Web API
- C# 12
- Minimal APIs (where appropriate)
```

#### Architecture Layers
```
src/
├── DentalTrack.Api/           # Presentation Layer
├── DentalTrack.Application/   # Application Layer  
├── DentalTrack.Domain/        # Domain Layer
└── DentalTrack.Infrastructure/ # Infrastructure Layer
```

#### Application Layer
```csharp
- MediatR (CQRS pattern)
- FluentValidation
- AutoMapper
- Application Services
- DTOs and Commands/Queries
```

#### Domain Layer
```csharp
- Domain Entities
- Value Objects
- Domain Events
- Business Rules
- Repository Interfaces
```

#### Infrastructure Layer
```csharp
- Entity Framework Core 8.0
- Repository Pattern
- Unit of Work
- External Service Integrations
- Data Access Layer
```

#### API Features
```csharp
- RESTful endpoints
- OpenAPI/Swagger documentation
- CORS configuration
- Rate limiting
- Health checks
- Logging (Serilog)
```

### 🗄️ Database & Storage

#### Azure SQL Database
```sql
- SQL Server (Azure)
- EF Core Code First
- Migrations
- Connection pooling
- Automatic backups
```

#### Azure Blob Storage
```
- Image storage
- CDN integration
- SAS tokens for secure access
- Hierarchical organization
- Lifecycle management
```

## ☁️ Cloud Infrastructure (Azure)

### 🌐 Hosting
```yaml
Frontend:
  - Azure Static Web Apps
  - Global CDN
  - Custom domain
  - SSL certificates

Backend:
  - Azure App Service
  - Auto-scaling
  - Deployment slots
  - Application Insights
```

### 🗄️ Data Services
```yaml
Database:
  - Azure SQL Database (S1 Standard)
  - Automatic backups
  - Point-in-time restore
  - Geo-replication (future)

Storage:
  - Azure Blob Storage
  - Hot tier for active images
  - Cool tier for archived images
  - CDN endpoint
```

### 🔐 Security Services
```yaml
Authentication:
  - Azure AD B2C (future)
  - Google OAuth 2.0 integration
  
Security:
  - Azure Key Vault (secrets)
  - Application Gateway (WAF)
  - Private endpoints
  - VNet integration (future)
```

### 🤖 AI Services
```yaml
Cognitive Services:
  - Computer Vision API
  - Custom Vision (future)
  - Face API (optional)
  
Analytics:
  - Application Insights
  - Log Analytics
  - Custom metrics
```

## 🔐 Authentication & Authorization

### 🔑 Google OAuth 2.0 Integration

#### Frontend Flow
```typescript
1. User clicks "Login with Google"
2. Redirect to Google OAuth
3. Receive authorization code
4. Exchange for ID token
5. Send ID token to backend
6. Receive JWT access token
7. Store token securely
```

#### Backend Validation
```csharp
1. Receive Google ID token
2. Validate with Google APIs
3. Extract user information
4. Create/update user in database
5. Generate JWT access token
6. Return token + user info
```

#### Security Features
```
- HTTPS only
- Secure cookie storage
- JWT with short expiration
- Refresh token rotation
- CSRF protection
```

## 🤖 AI & Machine Learning

### 🧠 Azure Cognitive Services

#### Computer Vision API
```yaml
Features:
  - Color analysis
  - Object detection
  - OCR (future)
  - Face detection (optional)

Use Cases:
  - Dental color analysis
  - Progress comparison
  - Anomaly detection
  - Quality assessment
```

#### Custom Vision (Future)
```yaml
Planned Features:
  - Custom model training
  - Dental-specific detection
  - Progress scoring
  - Treatment recommendations
```

## 🔧 Development Tools

### 💻 Local Development
```yaml
IDE:
  - Visual Studio 2022 / VS Code
  - .NET 8 SDK
  - Node.js 20+
  - Docker Desktop

Database:
  - SQL Server LocalDB
  - Azure Data Studio
  - EF Core Tools
```

### 🐳 Containerization
```yaml
Docker:
  - Multi-stage builds
  - Development containers
  - Docker Compose
  - Azure Container Registry
```

### 🔄 CI/CD Pipeline
```yaml
GitHub Actions:
  - Build and test
  - Security scanning
  - Deploy to staging
  - Deploy to production
  - Rollback capabilities
```

## 📊 Monitoring & Observability

### 📈 Application Monitoring
```yaml
Azure Application Insights:
  - Performance monitoring
  - Error tracking
  - User analytics
  - Custom metrics
  - Alerts and notifications
```

### 📋 Logging
```yaml
Serilog:
  - Structured logging
  - Multiple sinks
  - Log correlation
  - Performance counters
```

### 🚨 Health Checks
```yaml
Health Monitoring:
  - Database connectivity
  - External service status
  - Memory usage
  - Response times
```

## 🧪 Testing Strategy

### 🔬 Backend Testing
```csharp
Unit Tests:
  - xUnit
  - FluentAssertions
  - Moq (mocking)
  - >80% code coverage

Integration Tests:
  - TestContainers
  - In-memory database
  - API testing
  - End-to-end scenarios
```

### 🔬 Frontend Testing
```typescript
Unit Tests:
  - Vitest
  - React Testing Library
  - Jest DOM matchers
  - Component testing

E2E Tests (Future):
  - Playwright
  - User journey testing
  - Cross-browser testing
```

## 📦 Package Management

### 📚 Backend Packages
```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" />
<PackageReference Include="MediatR" />
<PackageReference Include="AutoMapper" />
<PackageReference Include="FluentValidation" />
<PackageReference Include="Serilog" />
<PackageReference Include="Google.Apis.Auth" />
```

### 📚 Frontend Packages
```json
{
  "@mui/material": "^5.14.0",
  "@tanstack/react-query": "^4.0.0",
  "react-router-dom": "^6.0.0",
  "axios": "^1.0.0",
  "react-hook-form": "^7.0.0",
  "@hookform/resolvers": "^3.0.0"
}
```

## 🚀 Performance Requirements

### ⚡ Frontend Performance
```
- First Contentful Paint: <1.5s
- Largest Contentful Paint: <2.5s
- Cumulative Layout Shift: <0.1
- First Input Delay: <100ms
- Bundle size: <500KB initial
```

### ⚡ Backend Performance
```
- API response time: <200ms (P95)
- Database query time: <100ms (P95)
- Image upload: <5s for 10MB
- Concurrent users: 100+
- Uptime: >99.5%
```

## 🔒 Security Requirements

### 🛡️ Data Security
```
- Encryption at rest (AES-256)
- Encryption in transit (TLS 1.3)
- PII data anonymization
- Secure key management
- Regular security audits
```

### 🛡️ Application Security
```
- OWASP Top 10 compliance
- Input validation
- SQL injection prevention
- XSS protection
- CSRF protection
```

---

📅 **Última actualización**: 2025-01-20  
🔧 **Stack Version**: MVP 1.0  
🎯 **Target**: Web Responsive MVP