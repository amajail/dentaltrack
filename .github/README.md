# 📋 GitHub Configuration Guide

Este documento guía la configuración completa del repositorio GitHub y Projects para DentalTrack MVP.

## 🏷️ Labels Configuration

### Crear los siguientes labels en el repositorio:

#### Priority Labels
```
priority: critical - #d73027 (rojo)
priority: high - #fc8d59 (naranja)
priority: medium - #fee08b (amarillo)
priority: low - #91cf60 (verde)
```

#### Type Labels  
```
bug - #d73027 (rojo)
enhancement - #4575b4 (azul)
user-story - #6a4c93 (morado)
documentation - #17a2b8 (cyan)
medical - #e83e8c (rosa)
```

#### Epic Labels
```
epic: setup - #6c757d (gris)
epic: api - #007bff (azul)
epic: react - #17a2b8 (cyan)
epic: auth - #ffc107 (amarillo)
epic: patients - #28a745 (verde)
epic: photos - #fd7e14 (naranja)
epic: ai - #6f42c1 (morado)
epic: reports - #e83e8c (rosa)
```

#### Status Labels
```
needs-triage - #ffc107 (amarillo)
in-progress - #007bff (azul)
needs-review - #fd7e14 (naranja)
blocked - #dc3545 (rojo)
```

## 🎯 Milestones

### Crear los siguientes milestones:

1. **Sprint 0 - Setup** (2 semanas)
   - Descripción: "Configuración inicial del monorepo y herramientas de desarrollo"
   - Due Date: +2 semanas desde inicio

2. **Sprint 1 - API & Auth** (2 semanas)
   - Descripción: "API base y sistema de autenticación con Google OAuth"
   - Due Date: +4 semanas desde inicio

3. **Sprint 2 - Frontend & Patients** (2 semanas)
   - Descripción: "Frontend base y gestión de pacientes"
   - Due Date: +6 semanas desde inicio

4. **Sprint 3 - Photos** (3 semanas)
   - Descripción: "Sistema de captura y almacenamiento de fotos"
   - Due Date: +9 semanas desde inicio

5. **Sprint 4 - AI** (3 semanas)
   - Descripción: "Integración de IA para análisis de imágenes dentales"
   - Due Date: +12 semanas desde inicio

6. **Sprint 5 - Reports** (2 semanas)
   - Descripción: "Sistema de reportes y seguimiento"
   - Due Date: +14 semanas desde inicio

7. **Sprint 6 - Testing** (2 semanas)
   - Descripción: "Testing integral y optimización"
   - Due Date: +16 semanas desde inicio

8. **Sprint 7 - Deploy** (2 semanas)
   - Descripción: "Deployment y lanzamiento del MVP"
   - Due Date: +18 semanas desde inicio

## 📊 GitHub Project Setup

### 1. Crear nuevo Project (beta)
- **Name**: "DentalTrack MVP Development"
- **Description**: "MVP development tracking for dental treatment management system"
- **Visibility**: Public

### 2. Configurar Views

#### Board View (Kanban)
Columnas:
- **Backlog**: Issues nuevos sin asignar
- **Todo**: Issues priorizados y listos para desarrollo
- **In Progress**: Issues actualmente en desarrollo
- **Review**: Issues en code review
- **Done**: Issues completados

#### Timeline View
- **Sprint planning**: Vista cronológica de sprints
- **Milestone tracking**: Seguimiento de deadlines
- **Epic roadmap**: Vista de alto nivel de épicas

#### Table View
- **Campos personalizados**: Epic, Sprint, Estimation, Platform
- **Filtros**: Por epic, sprint, prioridad
- **Agrupación**: Por epic o sprint

### 3. Custom Fields

#### Epic (Select)
```
- Setup
- API  
- React
- Auth
- Patients
- Photos
- AI
- Reports
```

#### Sprint (Select)
```
- Sprint 0
- Sprint 1
- Sprint 2
- Sprint 3
- Sprint 4
- Sprint 5
- Sprint 6
- Sprint 7
```

#### Estimation (Select)
```
- XS (1 punto)
- S (2 puntos)
- M (3 puntos)
- L (5 puntos)
- XL (8 puntos)
```

#### Platform (Select)
```
- Backend
- Frontend
- Full-stack
- DevOps
- Documentation
```

## 🔧 Repository Settings

### General Settings
- **Description**: "DentalTrack - Sistema de gestión de tratamientos dentales con IA"
- **Topics**: `dental`, `healthcare`, `ai`, `react`, `dotnet`, `azure`, `mvp`
- **Features**: ✅ Issues, ✅ Projects, ✅ Wiki, ✅ Discussions

### Branch Protection (main)
- ✅ Require pull request reviews before merging
- ✅ Require status checks to pass before merging  
- ✅ Require branches to be up to date before merging
- ✅ Include administrators
- ❌ Allow force pushes
- ❌ Allow deletions

### Merge Settings
- ✅ Allow merge commits
- ✅ Allow squash merging
- ✅ Allow rebase merging
- ✅ Auto-delete head branches

## 🤖 Automation Rules

### Para configurar en GitHub Project:

1. **Issue created** → Add to project
2. **PR opened** → Move linked issue to "In Progress"
3. **PR merged** → Move linked issue to "Done"
4. **Issue labeled "blocked"** → Move to "Blocked" column
5. **Issue labeled "needs-review"** → Move to "Review" column

## 👥 Team Configuration

### Collaborators
- Configurar permisos apropiados para cada miembro del equipo
- Asignar code owners para diferentes áreas del código

### CODEOWNERS (.github/CODEOWNERS)
```
# Global owners
* @team-lead @senior-dev

# Frontend specific
/frontend/ @frontend-dev @ui-ux-designer

# Backend specific  
/backend/ @backend-dev @senior-dev

# Documentation
/docs/ @product-owner @technical-writer

# CI/CD
/.github/ @devops-engineer @team-lead

# Medical/Clinical features
/backend/src/**/Medical/ @clinical-advisor @senior-dev
```

## 📝 Workflow Documentation

### Para el equipo:

1. **Issues**: Usar templates apropiados para cada tipo de issue
2. **Branches**: Seguir convención `feature/US-XXX-description`
3. **Commits**: Usar conventional commits con emojis
4. **PRs**: Completar checklist completo antes de solicitar review
5. **Reviews**: Al menos 1 reviewer requerido, 2 para cambios críticos
6. **Merge**: Squash merge preferido para mantener historia limpia

### Comandos útiles:
```bash
# Crear branch para nueva US
git checkout main
git pull origin main
git checkout -b feature/US-XXX-description

# Commit con formato estándar
git commit -m "✨ Add patient registration form (US-XXX)"

# Push y crear PR
git push -u origin feature/US-XXX-description
gh pr create --title "✨ US-XXX: Patient Registration" --assignee @me
```

## 🔗 Links Importantes

- **Repository**: https://github.com/[username]/dentaltrack
- **Project**: https://github.com/[username]/dentaltrack/projects/1
- **Issues**: https://github.com/[username]/dentaltrack/issues
- **Actions**: https://github.com/[username]/dentaltrack/actions
- **Wiki**: https://github.com/[username]/dentaltrack/wiki

---

📅 **Actualizado**: 2025-07-20  
👤 **Responsable**: Product Owner + Dev Lead  
🔄 **Estado**: Ready for Manual Configuration