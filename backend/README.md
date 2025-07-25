# DentalTrack Backend - Clean Architecture Implementation

## 🎯 US-005 Implementation Status: ✅ COMPLETED

This implementation provides a complete Clean Architecture backend for the DentalTrack dental practice management system.

## 🏗️ Architecture Overview

```
backend/src/
├── DentalTrack.Domain/          # Core business logic & entities
│   ├── Entities/               # Patient, Treatment, Photo, Analysis
│   ├── ValueObjects/           # Enums and value types
│   └── Interfaces/             # Repository contracts
├── DentalTrack.Application/     # Use cases & business rules
│   ├── DTOs/                   # Data transfer objects
│   ├── Commands/               # CQRS commands
│   ├── Queries/                # CQRS queries
│   ├── Handlers/               # MediatR handlers
│   └── Mappings/               # AutoMapper profiles
├── DentalTrack.Infrastructure/  # Data access & external services
│   ├── Data/                   # EF Core DbContext
│   └── Repositories/           # Repository implementations
└── DentalTrack.Api/            # REST API endpoints
    ├── Controllers/            # API controllers
    └── Middleware/             # Error handling, etc.
```

## 🚀 Quick Start

### Prerrequisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (o Docker)

### 1. Restaurar dependencias
```bash
dotnet restore
```

### 2. Configurar base de datos
```bash
# Con Docker (recomendado)
docker-compose up -d

# O manualmente con SQL Server local
# Actualizar ConnectionString en appsettings.Development.json
```

### 3. Ejecutar migraciones (futuro)
```bash
dotnet ef database update --project src/DentalTrack.Infrastructure
```

### 4. Ejecutar la aplicación
```bash
dotnet run --project src/DentalTrack.Api
```

La API estará disponible en:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger**: http://localhost:5000/swagger

## 📦 Tecnologías

| Categoría | Tecnología | Versión | Propósito |
|-----------|------------|---------|-----------|
| **Framework** | .NET | 8.0 | Runtime principal |
| **ORM** | Entity Framework Core | 9.0.7 | Acceso a datos |
| **CQRS** | MediatR | 13.0.0 | Mediator pattern |
| **Mapping** | AutoMapper | 15.0.1 | Object mapping |
| **Validation** | FluentValidation | 11.3.1 | Validación de modelos |
| **Logging** | Serilog | 9.0.0 | Logging estructurado |
| **Auth** | JWT Bearer | 8.0.17 | Autenticación JWT |
| **OAuth** | Google.Apis.Auth | 1.70.0 | Google OAuth 2.0 |
| **Database** | SQL Server | 2022 | Base de datos principal |

## 🧪 Testing

```bash
# Ejecutar todos los tests
dotnet test

# Con coverage
dotnet test --collect:"XPlat Code Coverage"

# Solo una capa específica
dotnet test tests/DentalTrack.Domain.Tests/
```

## 🔐 Configuración

### Variables de Entorno

```bash
# Base de datos
ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=DentalTrack;User Id=sa;Password=DentalTrack123!;TrustServerCertificate=true;"

# JWT
JwtSettings__SecretKey="your-super-secret-key-here"
JwtSettings__Issuer="DentalTrack"
JwtSettings__Audience="DentalTrack-Users"
JwtSettings__ExpirationInMinutes="60"

# Google OAuth
GoogleAuth__ClientId="your-google-client-id"
GoogleAuth__ClientSecret="your-google-client-secret"

# Azure Cognitive Services (futuro)
AzureCognitive__Endpoint="https://your-region.cognitiveservices.azure.com/"
AzureCognitive__SubscriptionKey="your-subscription-key"
```

## 📁 Estructura de Archivos Clave

```
DentalTrack.Api/
├── Program.cs              # Application entry point
├── appsettings.json        # Configuration
└── Controllers/            # API endpoints

DentalTrack.Application/
├── Commands/               # Write operations (CQRS)
├── Queries/                # Read operations (CQRS)
└── DTOs/                   # Data contracts

DentalTrack.Domain/
├── Entities/               # Business entities
│   ├── Patient.cs          # Patient entity (futuro)
│   ├── Treatment.cs        # Treatment entity (futuro)
│   └── Photo.cs           # Photo entity (futuro)
└── ValueObjects/          # Domain value objects

DentalTrack.Infrastructure/
├── Data/
│   └── DentalTrackDbContext.cs  # EF DbContext (futuro)
└── Repositories/          # Data access implementations
```

## 🔄 Clean Architecture Layers

### 🌐 **API Layer** (`DentalTrack.Api`)
- Controllers para endpoints REST
- Middleware de autenticación
- Configuración de Swagger
- Dependency injection setup

### 📋 **Application Layer** (`DentalTrack.Application`)
- Commands y Queries (CQRS con MediatR)
- DTOs para transferencia de datos
- Validadores con FluentValidation
- Interfaces de servicios

### 💎 **Domain Layer** (`DentalTrack.Domain`)
- Entidades de negocio
- Value Objects
- Domain Events
- Business Rules

### 🔧 **Infrastructure Layer** (`DentalTrack.Infrastructure`)
- Entity Framework DbContext
- Repositorios
- Servicios externos (Azure, Google)
- Configuraciones de EF

## 📋 Comandos Útiles

```bash
# Build
dotnet build

# Watch mode (desarrollo)
dotnet watch run --project src/DentalTrack.Api

# Crear migración (futuro)
dotnet ef migrations add InitialCreate --project src/DentalTrack.Infrastructure

# Actualizar base de datos
dotnet ef database update --project src/DentalTrack.Infrastructure

# Generar script SQL
dotnet ef script --project src/DentalTrack.Infrastructure
```

## 🐳 Docker

```bash
# Solo base de datos
docker-compose up -d

# Con aplicación completa
docker-compose --profile full-stack up
```

## 📊 Métricas de Calidad

- **Code Coverage**: >80% target
- **Performance**: <200ms API response time
- **Security**: JWT + Google OAuth 2.0
- **Architecture**: Clean Architecture compliance

## 🔗 Links Relacionados

- [📋 User Stories](../docs/user-stories/README.md)
- [🛠️ Technical Stack](../docs/technical-stack.md)
- [☁️ Azure Deployment](../docs/deployment/azure-setup.md)
- [🌐 Frontend README](../frontend/README.md)

---

**🦷 DentalTrack Backend** - Desarrollado con .NET 8 y Clean Architecture