# DentalTrack Backend - Claude Context

## Project Overview
DentalTrack backend is a .NET 8 Web API implementing Clean Architecture for dental practice management, specifically focused on teeth whitening treatments.

## Architecture & Tech Stack
- **Framework**: .NET 8 Web API
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, API layers)
- **ORM**: Entity Framework Core 8
- **Database**: SQL Server (Azure SQL Database in production)
- **Pattern**: CQRS with MediatR
- **Authentication**: JWT + Google OAuth 2.0
- **Testing**: xUnit + Moq + FluentAssertions
- **Documentation**: Swagger/OpenAPI

## Commands
```bash
# Development
dotnet restore                              # Restore dependencies
dotnet build                               # Build solution
dotnet run --project src/DentalTrack.Api  # Start API server

# Database
dotnet ef migrations add <MigrationName> --project src/DentalTrack.Infrastructure --startup-project src/DentalTrack.Api
dotnet ef database update --project src/DentalTrack.Infrastructure --startup-project src/DentalTrack.Api

# Testing
dotnet test                                # Run all tests
dotnet test --collect:"XPlat Code Coverage" # Run tests with coverage
```

## Project Structure
```
backend/
├── src/
│   ├── DentalTrack.Api/                   # Presentation Layer
│   │   ├── Controllers/                   # API Controllers
│   │   ├── Middleware/                    # Custom middleware
│   │   ├── Extensions/                    # Service registration extensions
│   │   ├── Program.cs                     # Application entry point
│   │   └── appsettings.json              # Configuration
│   ├── DentalTrack.Application/           # Business Logic Layer
│   │   ├── Commands/                      # CQRS Commands
│   │   ├── Queries/                       # CQRS Queries
│   │   ├── Handlers/                      # Command/Query handlers
│   │   ├── Services/                      # Application services
│   │   ├── DTOs/                         # Data Transfer Objects
│   │   ├── Interfaces/                    # Application interfaces
│   │   └── Validators/                    # FluentValidation validators
│   ├── DentalTrack.Domain/                # Domain Layer
│   │   ├── Entities/                      # Domain entities
│   │   ├── ValueObjects/                  # Value objects
│   │   ├── Enums/                        # Domain enumerations
│   │   ├── Events/                       # Domain events
│   │   └── Interfaces/                    # Domain interfaces
│   └── DentalTrack.Infrastructure/        # Infrastructure Layer
│       ├── Data/                         # Database context & configurations
│       ├── Repositories/                 # Repository implementations
│       ├── Services/                     # External service implementations
│       ├── Migrations/                   # EF Core migrations
│       └── Extensions/                   # Infrastructure extensions
└── tests/                                # Test Projects
    ├── DentalTrack.Api.Tests/            # API integration tests
    ├── DentalTrack.Application.Tests/    # Application layer tests
    ├── DentalTrack.Domain.Tests/         # Domain layer tests
    └── DentalTrack.Infrastructure.Tests/ # Infrastructure tests
```

## Implemented Features

### US-005: Clean Architecture Implementation ✅
- **Domain Layer**: Complete entity models with proper encapsulation
  - `Patient`: Personal info, medical history, contact details
  - `Treatment`: Whitening treatments with progress tracking
  - `Photo`: Before/during/after photos with metadata
  - `TreatmentSession`: Individual treatment sessions
  - `User`: System users with role-based access

- **Application Layer**: CQRS pattern with MediatR
  - Command handlers for write operations
  - Query handlers for read operations
  - FluentValidation for input validation
  - Application services for business logic

- **Infrastructure Layer**: Data access and external services
  - Entity Framework Core with repository pattern
  - Database configurations and migrations
  - External service integrations (Azure services)

- **API Layer**: RESTful controllers with proper separation
  - Dependency injection configuration
  - Middleware pipeline setup
  - Swagger documentation

### US-006: Entity Framework Core Setup ✅
- **Database Context**: Complete `DentalTrackDbContext` configuration
- **Entity Configurations**: Fluent API configurations for all entities
- **Repository Pattern**: Generic repository with specific implementations
- **Migrations**: Database schema with proper relationships
- **Connection Management**: Azure SQL Database integration
- **Performance**: Optimized queries with proper indexing

## Database Schema
```sql
-- Core entities with relationships
Users (Id, Email, FirstName, LastName, Role, CreatedAt, UpdatedAt)
Patients (Id, FirstName, LastName, Email, Phone, DateOfBirth, MedicalHistory, UserId, CreatedAt, UpdatedAt)
Treatments (Id, PatientId, Type, StartDate, EndDate, Status, Notes, ExpectedSessions, CompletedSessions, CreatedAt, UpdatedAt)
TreatmentSessions (Id, TreatmentId, SessionNumber, Date, Notes, BeforePhotoUrl, AfterPhotoUrl, CreatedAt, UpdatedAt)
Photos (Id, TreatmentId, SessionId, Url, Type, UploadDate, Metadata, QualityScore, CreatedAt, UpdatedAt)
```

## Key Design Patterns

### Clean Architecture Layers
1. **Domain**: Business entities and rules (no dependencies)
2. **Application**: Use cases and business logic (depends on Domain)
3. **Infrastructure**: Data access and external services (depends on Application)
4. **API**: Controllers and presentation (depends on Application)

### CQRS Pattern
- **Commands**: Handle write operations with validation
- **Queries**: Handle read operations with optimized projections
- **Handlers**: Implement business logic for commands and queries
- **MediatR**: Mediator pattern for decoupled communication

### Repository Pattern
- **Generic Repository**: Common CRUD operations
- **Specific Repositories**: Domain-specific query methods
- **Unit of Work**: Transaction management across repositories

## Configuration

### Development Environment
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DentalTrackDev;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;"
  },
  "JwtSettings": {
    "SecretKey": "development-secret-key",
    "Issuer": "DentalTrack.Api",
    "Audience": "DentalTrack.Client",
    "ExpirationMinutes": 60
  }
}
```

### Production Environment
- Azure SQL Database connection string
- Azure Key Vault for secrets management
- Application Insights for logging and monitoring
- Environment-specific configurations

## API Endpoints

### Authentication
- `POST /api/auth/login` - User authentication
- `POST /api/auth/refresh` - Token refresh
- `POST /api/auth/logout` - User logout

### Patients
- `GET /api/patients` - List patients with pagination
- `GET /api/patients/{id}` - Get patient details
- `POST /api/patients` - Create new patient
- `PUT /api/patients/{id}` - Update patient
- `DELETE /api/patients/{id}` - Delete patient

### Treatments
- `GET /api/treatments` - List treatments
- `GET /api/treatments/{id}` - Get treatment details
- `POST /api/treatments` - Create new treatment
- `PUT /api/treatments/{id}` - Update treatment
- `POST /api/treatments/{id}/sessions` - Add treatment session

### Photos
- `POST /api/photos/upload` - Upload treatment photos
- `GET /api/photos/{id}` - Get photo details
- `DELETE /api/photos/{id}` - Delete photo

## Testing Strategy

### Unit Tests
- **Domain Tests**: Entity validation and business rules
- **Application Tests**: Command/query handler testing with mocks
- **Repository Tests**: Database operations with in-memory database

### Integration Tests
- **API Tests**: Full request/response testing with TestServer
- **Database Tests**: EF Core operations with test database
- **Service Tests**: External service integration testing

### Test Configuration
```csharp
// Example test setup
public class TestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    
    public TestBase(WebApplicationFactory<Program> factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }
}
```

## Security Implementation

### Authentication & Authorization
- **JWT Tokens**: Secure token-based authentication
- **Role-Based Access**: Dentist, Assistant, Admin roles
- **Google OAuth**: Social login integration
- **Password Security**: BCrypt hashing for local accounts

### Data Protection
- **Input Validation**: FluentValidation for all inputs
- **SQL Injection Prevention**: EF Core parameterized queries
- **CORS Configuration**: Secure cross-origin requests
- **HTTPS Enforcement**: SSL/TLS for all communications

## Performance Considerations

### Database Optimization
- **Indexing**: Proper indexes on frequently queried columns
- **Query Optimization**: Efficient LINQ queries with projections
- **Connection Pooling**: EF Core connection pool configuration
- **Async Operations**: All database operations are async

### Caching Strategy
- **Memory Caching**: In-memory cache for frequently accessed data
- **Response Caching**: HTTP response caching for static data
- **Entity Tracking**: No-tracking queries for read-only operations

## Error Handling

### Global Exception Handling
```csharp
// Custom middleware for global exception handling
public class GlobalExceptionMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
}
```

### Custom Exceptions
- `DomainException`: Business rule violations
- `NotFoundException`: Entity not found errors
- `ValidationException`: Input validation errors
- `UnauthorizedException`: Authorization failures

## Monitoring & Logging

### Application Insights
- **Request Tracking**: HTTP request/response monitoring
- **Dependency Tracking**: Database and external service calls
- **Exception Tracking**: Automatic exception logging
- **Custom Metrics**: Business-specific metrics

### Structured Logging
```csharp
// Example logging with Serilog
Log.Information("Patient {PatientId} treatment {TreatmentId} started", 
    patientId, treatmentId);
```

## Development Guidelines

### Code Standards
- **C# Conventions**: Microsoft C# coding standards
- **Async/Await**: Use async patterns for I/O operations
- **SOLID Principles**: Follow SOLID design principles
- **Clean Code**: Readable, maintainable code with proper naming

### Git Workflow
- **Feature Branches**: Create feature branches for new development
- **Pull Requests**: Code review through PRs
- **Quality Gates**: Automated testing and quality checks
- **Conventional Commits**: Use conventional commit messages

## Recent Implementations
- Complete Clean Architecture setup with all layers
- Entity Framework Core with repository pattern
- CQRS implementation with MediatR
- JWT authentication with role-based authorization
- Comprehensive test suite with 80%+ coverage
- API documentation with Swagger/OpenAPI

## Development Notes
- Use `dotnet watch run` for hot reload during development
- Entity Framework migrations are auto-generated on model changes
- API versioning is configured for future compatibility
- Health checks are implemented for monitoring
- Docker support available for containerized deployment

## Future Enhancements
- AI integration for photo analysis (Azure Cognitive Services)
- Real-time notifications with SignalR
- Advanced reporting with custom analytics
- Mobile API optimizations
- Performance monitoring and optimization