# 📋 DentalTrack Documentation

## 📚 Índice de Documentación

### 🎯 Project Overview
- [Project Overview](./project-overview.md) - Descripción general del proyecto
- [Technical Stack](./technical-stack.md) - Stack tecnológico completo
- [Roadmap](./roadmap.md) - Hoja de ruta del proyecto

### 👥 User Stories & Backlog
- [Backlog Completo](./user-stories/README.md) - Backlog organizado por épicas
- [Claude Code Prompts](./user-stories/claude-prompts.md) - Prompts completos para implementación

#### 📦 Épicas del MVP

1. **[Setup Inicial](./user-stories/epic-1-setup/)** - Configuración base del proyecto
2. **[Web API (.NET)](./user-stories/epic-2-api/)** - Backend API con Clean Architecture
3. **[React Web App](./user-stories/epic-3-react/)** - Frontend web responsive
4. **[Autenticación Google](./user-stories/epic-6-auth/)** - Sistema de autenticación
5. **[Gestión de Pacientes](./user-stories/epic-7-patients/)** - CRUD de pacientes y tratamientos
6. **[Gestión de Fotos](./user-stories/epic-8-photos/)** - Captura y organización de fotos
7. **[IA Básica](./user-stories/epic-9-ai/)** - Análisis automático con Azure Cognitive Services
8. **[Reportes](./user-stories/epic-10-reports/)** - Generación de reportes y estadísticas

### 🚀 Sprint Planning
- [Sprint 0](./sprints/sprint-0.md) - Setup inicial y configuración
- [Sprint 1](./sprints/sprint-1.md) - API base y autenticación
- [Sprint 2](./sprints/sprint-2.md) - Frontend base y pacientes
- [Sprint 3](./sprints/sprint-3.md) - Gestión de fotos
- [Sprint 4](./sprints/sprint-4.md) - IA básica y análisis
- [Sprint 5](./sprints/sprint-5.md) - Reportes y dashboard
- [Sprint 6](./sprints/sprint-6.md) - Testing y optimización
- [Sprint 7](./sprints/sprint-7.md) - Deploy y documentación

### 🚀 Deployment
- [Azure Setup](./deployment/azure-setup.md) - Configuración de infraestructura Azure
- [CI/CD Pipeline](./deployment/ci-cd.md) - Pipeline de integración continua

## 🎯 MVP Scope

**Objetivo**: Sistema web responsive para gestión de tratamientos dentales con IA básica

**Plataforma**: Solo Web (Responsive Design)
- Desktop: 1920x1080+
- Tablet: 768x1024
- Mobile: 375x667+

**Timeline**: 7 sprints (5 meses)

**Stack Principal**:
- Backend: .NET 8 Web API + Clean Architecture
- Frontend: React 18+ + TypeScript + Material-UI
- Cloud: Azure (App Service, SQL Database, Blob Storage)
- Auth: Google OAuth 2.0
- AI: Azure Cognitive Services

## 🏗️ Arquitectura del MVP

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   React Web     │    │   .NET Web API  │    │   Azure SQL     │
│   (Responsive)  │───▶│ Clean Arch      │───▶│   Database      │
│   Material-UI   │    │   EF Core       │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
                                │
                                ▼
                       ┌─────────────────┐
                       │ Azure Cognitive │
                       │   Services      │
                       │  (Computer      │
                       │   Vision)       │
                       └─────────────────┘
```

## 📋 Quick Start

1. **Clonar el repositorio**:
   ```bash
   git clone https://github.com/amajail/dentaltrack.git
   cd dentaltrack
   ```

2. **Leer la documentación**:
   - Comenzar con [Project Overview](./project-overview.md)
   - Revisar [Technical Stack](./technical-stack.md)
   - Consultar [User Stories](./user-stories/README.md)

3. **Implementación**:
   - Usar [Claude Code Prompts](./user-stories/claude-prompts.md)
   - Seguir sprint planning en orden
   - Cada user story incluye prompts completos

## 🎯 Roles y Usuarios

### 👨‍⚕️ Dentist (Usuario Principal)
- Gestiona pacientes y tratamientos
- Captura y analiza fotos dentales
- Genera reportes de progreso
- Configura parámetros de tratamiento

### 👩‍💼 Dental Assistant
- Asiste en captura de fotos
- Gestiona citas y seguimiento
- Acceso limitado a funcionalidades

### 👩‍💻 Admin
- Gestiona usuarios del sistema
- Configura parámetros globales
- Accede a estadísticas generales
- Administra suscripciones (futuro)

## 📊 Progress Tracking

- **Epic Progress**: Ver carpetas individuales de épicas
- **Sprint Progress**: Revisar archivos de sprint planning
- **GitHub Projects**: [Link al proyecto en GitHub](https://github.com/amajail/dentaltrack/projects)

## 🤝 Contributing

1. Revisar user story correspondiente
2. Usar prompt de Claude Code proporcionado
3. Seguir definition of done
4. Crear PR con template

## 📞 Support

- **Documentation Issues**: Crear issue con template
- **Technical Questions**: Consultar technical-stack.md
- **Project Questions**: Revisar project-overview.md

---

📅 **Última actualización**: 2025-01-20  
🎯 **Estado actual**: MVP Planning Complete  
🚀 **Próximo milestone**: Sprint 0 - Setup Inicial