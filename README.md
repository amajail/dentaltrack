# 🦷 DentalTrack

[![CI](https://github.com/amajail/dentaltrack/workflows/CI%20Pipeline/badge.svg)](https://github.com/amajail/dentaltrack/actions)
[![Coverage](https://codecov.io/gh/amajail/dentaltrack/branch/main/graph/badge.svg)](https://codecov.io/gh/amajail/dentaltrack)
[![Security](https://github.com/amajail/dentaltrack/workflows/Security%20Scan/badge.svg)](https://github.com/amajail/dentaltrack/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

> Sistema integral de gestión de tratamientos dentales con análisis de IA, optimizado para blanqueamiento dental MVP.

## 🎯 Descripción

DentalTrack es una plataforma web responsive que permite a profesionales dentales gestionar, monitorear y analizar tratamientos de blanqueamiento dental utilizando inteligencia artificial. El sistema proporciona herramientas avanzadas para el seguimiento de progreso, análisis automático de imágenes y generación de reportes profesionales.

### ✨ Características Principales

- **👨‍⚕️ Gestión de Pacientes**: CRUD completo con historial médico dental
- **📸 Captura de Fotos**: Captura optimizada desde navegador (desktop/mobile)
- **🤖 Análisis IA**: Análisis automático de progreso con Azure Cognitive Services
- **📊 Reportes Visuales**: Generación automática de reportes profesionales
- **🔐 Autenticación Segura**: Google OAuth 2.0 con JWT tokens
- **📱 100% Responsive**: Funciona perfectamente en desktop, tablet y mobile

## 🏗️ Arquitectura

### Stack Tecnológico

```
┌─────────────────────────────────────────────────────────────┐
│                    FRONTEND WEB                             │
│  React 18+ │ TypeScript │ Material-UI │ Vite │ React Query │
└─────────────────────────────────────────────────────────────┘
                              │ HTTPS/REST API
┌─────────────────────────────────────────────────────────────┐
│                    BACKEND API                              │
│   .NET 8 │ Clean Architecture │ MediatR │ EF Core │ JWT    │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                    AZURE CLOUD                             │
│ SQL Database │ Blob Storage │ App Service │ Cognitive Svcs │
└─────────────────────────────────────────────────────────────┘
```

### Estructura del Proyecto

```
dentaltrack/
├── 📁 backend/                 # .NET 8 Web API (Clean Architecture)
│   ├── src/
│   │   ├── DentalTrack.Api/           # Presentation Layer
│   │   ├── DentalTrack.Application/   # Business Logic (MediatR)
│   │   ├── DentalTrack.Domain/        # Domain Entities
│   │   └── DentalTrack.Infrastructure/ # Data Access (EF Core)
│   └── tests/                         # Unit & Integration Tests
├── 📁 frontend/                # React 18+ TypeScript App
│   ├── src/
│   │   ├── components/               # Reusable Components
│   │   ├── pages/                    # Application Pages
│   │   ├── services/                 # API Services
│   │   └── hooks/                    # Custom React Hooks
│   └── public/                       # Static Assets
├── 📁 docs/                    # Comprehensive Documentation
├── 📁 .github/                 # GitHub Actions & Templates
└── 📄 docker-compose.yml       # Local Development Stack
```

## 🚀 Quick Start

### Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### 🐳 Setup con Docker (Recomendado)

```bash
# 1. Clonar el repositorio
git clone https://github.com/amajail/dentaltrack.git
cd dentaltrack

# 2. Iniciar servicios de base de datos
docker-compose up -d

# 3. Ejecutar backend
cd backend
dotnet run --project src/DentalTrack.Api

# 4. Ejecutar frontend (nueva terminal)
cd frontend
npm install
npm run dev
```

### 🌐 URLs de Desarrollo

- **Frontend**: http://localhost:3000
- **API Backend**: http://localhost:5000
- **API Documentation**: http://localhost:5000/swagger
- **SQL Server**: localhost:1433

### ⚙️ Setup Manual

<details>
<summary>Expandir para instrucciones de setup manual</summary>

#### Backend (.NET 8)
```bash
cd backend
dotnet restore
dotnet build
dotnet run --project src/DentalTrack.Api
```

#### Frontend (React + TypeScript)
```bash
cd frontend
npm install
npm run dev
```

#### Base de Datos
```bash
# SQL Server con Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=DentalTrack123!" \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

</details>

## 📚 Documentación

### 🎯 Para Developers
- [📋 Project Overview](./docs/project-overview.md) - Visión general y objetivos
- [🛠️ Technical Stack](./docs/technical-stack.md) - Stack tecnológico detallado
- [🗺️ Roadmap](./docs/roadmap.md) - Hoja de ruta del producto

### 📋 Backlog y User Stories
- [📝 Backlog Completo](./docs/user-stories/README.md) - Todas las user stories organizadas
- [🤖 Claude Code Prompts](./docs/user-stories/claude-prompts.md) - Prompts listos para implementación

### 🚀 Sprint Planning
- [Sprint 0: Setup Inicial](./docs/sprints/sprint-0.md)
- [Sprint 1: API & Auth](./docs/sprints/sprint-1.md)
- [Sprint 2: Frontend & Pacientes](./docs/sprints/sprint-2.md)
- [Todos los Sprints →](./docs/sprints/)

### ☁️ Deployment
- [Azure Setup Guide](./docs/deployment/azure-setup.md)
- [CI/CD Pipeline](./docs/deployment/ci-cd.md)

## 🎭 Roles de Usuario

### 👨‍⚕️ Dentist (Usuario Principal)
- Gestiona pacientes y tratamientos de blanqueamiento
- Captura y analiza fotos dentales
- Genera reportes de progreso profesionales
- Configura parámetros de tratamiento

### 👩‍💼 Dental Assistant
- Asiste en captura de fotos y datos
- Gestiona citas y seguimiento básico
- Acceso limitado según permisos

### 👩‍💻 Admin
- Gestiona usuarios del sistema
- Configura parámetros globales
- Accede a analíticas generales

## 🧪 Testing

```bash
# Backend Tests
cd backend
dotnet test

# Frontend Tests
cd frontend
npm test
npm run test:coverage

# E2E Tests (futuro)
npm run test:e2e
```

## 🤝 Contributing

### 📋 Workflow de Desarrollo

1. **Crear Issue**: Usar [User Story template](.github/ISSUE_TEMPLATE/user-story.md)
2. **Branch**: Crear desde `main` con nombre descriptivo
3. **Implementar**: Usar [Claude Code Prompts](./docs/user-stories/claude-prompts.md)
4. **Testing**: Validar responsive design en múltiples dispositivos
5. **PR**: Usar [PR template](.github/PULL_REQUEST_TEMPLATE.md)
6. **Review**: Code review con focus en responsive y security
7. **Merge**: Squash merge después de approval

### 🎨 Standards

- **Backend**: Clean Architecture, SOLID principles, >80% test coverage
- **Frontend**: Material-UI components, responsive-first design
- **Mobile**: Touch-friendly (44px+ targets), readable text
- **Performance**: <2s load times, <200ms API responses
- **Security**: Input validation, JWT tokens, HTTPS only

## 📊 Status del Proyecto

### 🎯 MVP Progress
- ✅ **Setup Inicial**: Infraestructura y CI/CD completos
- 🔄 **Sprint 1**: API base y autenticación (en progreso)
- 📋 **Sprint 2**: Gestión de pacientes (planeado)
- 📋 **Sprint 3**: Captura de fotos (planeado)
- 📋 **Sprint 4**: Análisis IA básico (planeado)

### 📈 Metrics
- **Code Coverage**: 85%+ target
- **Performance**: <2s load time
- **Security**: Zero critical vulnerabilities
- **Responsive**: 100% mobile compatibility

## 🌐 Ambientes

| Ambiente | URL | Status | Uso |
|----------|-----|--------|-----|
| **Development** | http://localhost:3000 | 🟢 Active | Local development |
| **Staging** | https://dentaltrack-staging.azurestaticapps.net | 🟡 Pending | Testing & validation |
| **Production** | https://www.dentaltrack.com | 🔴 Not deployed | Live system (future) |

## 📞 Support

### 🐛 Issues y Bugs
- [Crear Bug Report](.github/ISSUE_TEMPLATE/bug_report.md)
- [Ver Issues Abiertas](https://github.com/amajail/dentaltrack/issues)

### 💡 Feature Requests
- [Crear Feature Request](.github/ISSUE_TEMPLATE/feature_request.md)
- [Roadmap del Proyecto](./docs/roadmap.md)

### 📋 Project Management
- [GitHub Projects](https://github.com/amajail/dentaltrack/projects)
- [Milestones](https://github.com/amajail/dentaltrack/milestones)

## 📄 License

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

## 🙏 Acknowledgments

- **Material-UI** por el sistema de diseño responsive
- **Azure** por la infraestructura cloud confiable
- **Claude Code** por acelerar el desarrollo con IA

---

## 🎯 MVP Scope

**Objetivo**: Sistema web responsive para gestión de tratamientos de blanqueamiento dental con IA básica

**Plataforma**: Solo Web (funciona en mobile browsers)  
**Timeline**: 7 sprints (5 meses)  
**Launch**: Q2 2025  

### 🚫 Fuera del MVP
- ❌ Apps móviles nativas (iOS/Android)
- ❌ Aplicación desktop (.NET MAUI)
- ❌ Otros tratamientos dentales
- ❌ IA avanzada/entrenamiento custom
- ❌ Integraciones EMR/EHR

---

<div align="center">

**🦷 DentalTrack** - Transformando el cuidado dental con tecnología

[🌐 Website](https://www.dentaltrack.com) • [📧 Contact](mailto:support@dentaltrack.com) • [📱 Demo](https://dentaltrack-staging.azurestaticapps.net)

</div>