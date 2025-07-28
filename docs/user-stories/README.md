# 📋 DentalTrack - Backlog & User Stories

## 🎯 MVP Backlog Overview

**Total User Stories**: 25 (29 with extras)  
**Total Épicas**: 8  
**Timeline**: 7 sprints (5 meses)  
**Platform**: Web Responsive Only

## 📊 Epic Overview

| Epic | User Stories | Sprint | Priority | Status |
|------|-------------|--------|----------|--------|
| [🔧 Setup Inicial](#epic-1-setup-inicial) | 4 | Sprint 0 | High | 🔄 In Progress (2/4 completed) |
| [🌐 Web API](#epic-2-web-api) | 3 | Sprint 1 | High | 📋 Ready |
| [⚛️ React Web App](#epic-3-react-web-app) | 3 | Sprint 1-2 | High | 📋 Ready |
| [🔐 Autenticación](#epic-6-autenticación) | 3 | Sprint 1-2 | High | 📋 Ready |
| [👥 Gestión Pacientes](#epic-7-gestión-pacientes) | 3 | Sprint 2-3 | High | 📋 Ready |
| [📸 Gestión Fotos](#epic-8-gestión-fotos) | 3 | Sprint 3-4 | High | 📋 Ready |
| [🤖 IA Básica](#epic-9-ia-básica) | 3 | Sprint 4-5 | Medium | 📋 Ready |
| [📊 Reportes](#epic-10-reportes) | 3 | Sprint 5-6 | Medium | 📋 Ready |

## 🔧 Epic 1: Setup Inicial

**Objetivo**: Configurar infraestructura completa del proyecto

### User Stories

| ID | Título | Estimación | Dependencias | Status |
|----|--------|------------|--------------|--------|
| [US-001](./epic-1-setup/US-001.md) | Configurar monorepo con estructura Clean Architecture | L | - | ✅ **COMPLETED** |
| [US-002](./epic-1-setup/US-002.md) | Configurar GitHub repository y Projects | M | US-001 | 📋 Ready |
| [US-003](./epic-1-setup/US-003.md) | Configurar GitHub Actions CI/CD pipeline | L | US-001, US-002 | ✅ **COMPLETED** |
| [US-004](./epic-1-setup/US-004.md) | Configurar infraestructura Azure completa | XL | US-001 | 📋 Ready |
| [US-004b](./epic-1-setup/US-004b.md) | Activar y validar CI/CD pipeline | S | US-003 | 📋 Ready |

**Sprint**: Sprint 0  
**Duración estimada**: 2 semanas  
**Progreso**: 2/5 user stories completed (40%)

---

## 🌐 Epic 2: Web API (.NET)

**Objetivo**: Crear API backend con Clean Architecture

### User Stories

| ID | Título | Estimación | Dependencias |
|----|--------|------------|--------------|
| [US-005](./epic-2-api/US-005.md) | Configurar Clean Architecture backend completa | XL | US-001, US-004 |
| [US-006](./epic-2-api/US-006.md) | Configurar Entity Framework y base de datos | L | US-005 |
| [US-007](./epic-2-api/US-007.md) | Implementar endpoints básicos y documentación | M | US-005, US-006 |

**Sprint**: Sprint 1  
**Duración estimada**: 2 semanas

---

## ⚛️ Epic 3: React Web App

**Objetivo**: Crear frontend web responsive con React

### User Stories

| ID | Título | Estimación | Dependencias |
|----|--------|------------|--------------|
| [US-008](./epic-3-react/US-008.md) | Configurar React base responsive con Material-UI | L | US-001 |
| [US-009](./epic-3-react/US-009.md) | Configurar comunicación con API y state management | M | US-007, US-008 |
| [US-010](./epic-3-react/US-010.md) | Crear interface responsive moderna y navegación | L | US-008, US-009 |

**Sprint**: Sprint 1-2  
**Duración estimada**: 3 semanas

---

## 🔐 Epic 6: Autenticación Google

**Objetivo**: Implementar autenticación segura con Google OAuth

### User Stories

| ID | Título | Estimación | Dependencias |
|----|--------|------------|--------------|
| [US-015](./epic-6-auth/US-015.md) | Implementar Google OAuth web responsive | L | US-005, US-010 |
| [US-016](./epic-6-auth/US-016.md) | Crear sistema de roles y permisos | M | US-015 |
| [US-017](./epic-6-auth/US-017.md) | Crear panel administración usuarios | M | US-016 |

**Sprint**: Sprint 1-2  
**Duración estimada**: 2 semanas

---

## 👥 Epic 7: Gestión de Pacientes

**Objetivo**: CRUD completo de pacientes y tratamientos

### User Stories

| ID | Título | Estimación | Dependencias |
|----|--------|------------|--------------|
| [US-018](./epic-7-patients/US-018.md) | Crear y gestionar pacientes con historial médico | L | US-015, US-007 |
| [US-019](./epic-7-patients/US-019.md) | Crear y gestionar tratamientos de blanqueamiento | M | US-018 |
| [US-020](./epic-7-patients/US-020.md) | Dashboard gestión de tratamientos activos | M | US-019 |

**Sprint**: Sprint 2-3  
**Duración estimada**: 3 semanas

---

## 📸 Epic 8: Gestión de Fotos

**Objetivo**: Captura, organización y comparación de fotos dentales

### User Stories

| ID | Título | Estimación | Dependencias |
|----|--------|------------|--------------|
| [US-021](./epic-8-photos/US-021.md) | Capturar fotos desde navegador web/mobile | L | US-019, US-004 |
| [US-022](./epic-8-photos/US-022.md) | Organizar fotos por sesiones y tratamientos | M | US-021 |
| [US-023](./epic-8-photos/US-023.md) | Comparar fotos before/after touch-friendly | M | US-022 |

**Sprint**: Sprint 3-4  
**Duración estimada**: 4 semanas

---

## 🤖 Epic 9: IA Básica

**Objetivo**: Análisis automático básico con Azure Cognitive Services

### User Stories

| ID | Título | Estimación | Dependencias |
|----|--------|------------|--------------|
| [US-024](./epic-9-ai/US-024.md) | Análisis automático básico de progreso dental | L | US-023, US-004 |
| [US-025](./epic-9-ai/US-025.md) | Generar reportes básicos con análisis IA | M | US-024 |
| [US-026](./epic-9-ai/US-026.md) | Validar y ajustar resultados de IA manualmente | M | US-025 |

**Sprint**: Sprint 4-5  
**Duración estimada**: 4 semanas

---

## 📊 Epic 10: Reportes

**Objetivo**: Generación de reportes y dashboard analytics

### User Stories

| ID | Título | Estimación | Dependencias |
|----|--------|------------|--------------|
| [US-027](./epic-10-reports/US-027.md) | Generar reportes de progreso por paciente | M | US-026 |
| [US-028](./epic-10-reports/US-028.md) | Dashboard analíticas de tratamientos | M | US-027 |
| [US-029](./epic-10-reports/US-029.md) | Portal básico de paciente para ver progreso | L | US-027 |

**Sprint**: Sprint 5-6  
**Duración estimada**: 3 semanas

---

## 📈 Backlog Prioritization

### 🔥 Must Have (MVP Core)
- Epic 1: Setup Inicial ✅
- Epic 2: Web API ✅  
- Epic 3: React Web App ✅
- Epic 6: Autenticación ✅
- Epic 7: Gestión Pacientes ✅
- Epic 8: Gestión Fotos ✅

### 🎯 Should Have (MVP Enhanced)
- Epic 9: IA Básica
- Epic 10: Reportes (básicos)

### 💡 Could Have (Post-MVP)
- Advanced AI features
- Mobile native apps
- Desktop application
- Multi-treatment support

### 🚫 Won't Have (This Release)
- EMR/EHR integrations
- Payment processing
- Multi-tenant architecture
- Advanced AI training

## 📋 Definition of Ready

Cada User Story debe tener:
- [ ] Criterios de aceptación claros y testeable
- [ ] Mockups/wireframes (cuando aplique)
- [ ] Dependencias identificadas y resueltas
- [ ] Estimación completada por el equipo
- [ ] Claude Code prompt preparado y validado
- [ ] Aspectos de seguridad considerados
- [ ] Impacto responsive design documentado

## ✅ Definition of Done

Cada User Story se considera completa cuando:
- [ ] Código implementado siguiendo estándares
- [ ] Code review completado y aprobado
- [ ] Tests unitarios >80% coverage
- [ ] Tests de integración passing
- [ ] Responsive design validado (mobile/tablet/desktop)
- [ ] Performance optimizado (<2s carga)
- [ ] Documentación técnica actualizada
- [ ] Deploy en staging exitoso
- [ ] Acceptance criteria validados
- [ ] Security review completado

## 🔄 Backlog Maintenance

### 📅 Refinement Schedule
- **Backlog Refinement**: Viernes semanales
- **Sprint Planning**: Cada 2-3 semanas
- **Sprint Review**: Final de cada sprint
- **Retrospective**: Final de cada sprint

### 📊 Tracking Metrics
- **Velocity**: Story points por sprint
- **Burndown**: Progreso vs tiempo
- **Cycle Time**: Tiempo por story
- **Quality**: Defects por sprint

### 🔄 Backlog Updates
- Stories pueden ser refinadas basado en feedback
- Prioridades pueden cambiar según necesidades del negocio
- Nuevas stories pueden agregarse al backlog
- Estimaciones pueden actualizarse con mejor información

---

📅 **Creado**: 2025-01-20  
👥 **Product Owner**: DentalTrack Team  
🎯 **Estado**: Ready for Sprint 0  
📋 **Total Story Points**: ~150 points