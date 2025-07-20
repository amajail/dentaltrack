# 🤖 Claude Code Prompts - DentalTrack MVP

## 📋 Uso de Prompts

Estos prompts están diseñados para ser **copy-paste directo** en Claude Code. Cada prompt es **autocontenido** y **completo** para implementar la user story correspondiente.

### 🔧 Instrucciones de Uso:
1. Copiar el prompt completo de la user story deseada
2. Pegar en Claude Code sin modificaciones
3. Claude Code implementará toda la funcionalidad
4. Revisar y validar el código generado
5. Proceder con testing y deployment

---

## 🏗️ ÉPICA 1: SETUP INICIAL

### US-001: Configurar monorepo con estructura Clean Architecture

```
Configurar monorepo DentalTrack con Clean Architecture para MVP web responsive:

ESTRUCTURA DEL PROYECTO:
```
dentaltrack/
├── backend/
│   ├── src/
│   │   ├── DentalTrack.Api/          # Presentation Layer
│   │   ├── DentalTrack.Application/  # Application Layer (MediatR, DTOs)
│   │   ├── DentalTrack.Domain/       # Domain Layer (Entities, Business Rules)
│   │   └── DentalTrack.Infrastructure/ # Infrastructure (EF Core, External APIs)
│   ├── tests/
│   │   ├── DentalTrack.Api.Tests/
│   │   ├── DentalTrack.Application.Tests/
│   │   ├── DentalTrack.Domain.Tests/
│   │   └── DentalTrack.Infrastructure.Tests/
│   └── DentalTrack.sln
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── hooks/
│   │   ├── services/
│   │   ├── types/
│   │   └── utils/
│   ├── public/
│   └── package.json
├── docs/
├── docker-compose.yml
├── .gitignore
└── README.md
```

BACKEND (.NET 8 CLEAN ARCHITECTURE):
1. Crear solución .NET 8 con proyectos:
   - DentalTrack.Api (ASP.NET Core Web API)
   - DentalTrack.Application (Class Library)
   - DentalTrack.Domain (Class Library)
   - DentalTrack.Infrastructure (Class Library)

2. Configurar referencias entre proyectos:
   - Api → Application + Infrastructure
   - Application → Domain
   - Infrastructure → Domain + Application

3. Agregar paquetes NuGet esenciales:
   - Microsoft.EntityFrameworkCore.SqlServer
   - MediatR
   - AutoMapper
   - FluentValidation
   - Serilog.AspNetCore
   - Microsoft.AspNetCore.Authentication.JwtBearer
   - Google.Apis.Auth

4. Configurar estructura base:
   - Program.cs con dependency injection
   - appsettings.json con configuraciones
   - Carpetas: Controllers, Models, Services en Api
   - Carpetas: Commands, Queries, DTOs en Application
   - Carpetas: Entities, ValueObjects, Interfaces en Domain
   - Carpetas: Data, Services, Repositories en Infrastructure

FRONTEND (REACT + TYPESCRIPT):
1. Crear proyecto React con Vite:
   - Template TypeScript
   - Configuración ESLint + Prettier
   - Configuración Husky para pre-commit hooks

2. Instalar dependencias principales:
   - @mui/material @emotion/react @emotion/styled
   - @mui/icons-material
   - @tanstack/react-query
   - react-router-dom
   - axios
   - react-hook-form @hookform/resolvers
   - zod (validación)

3. Configurar estructura base:
   - src/components/ (componentes reutilizables)
   - src/pages/ (páginas principales)
   - src/hooks/ (custom hooks)
   - src/services/ (APIs y servicios)
   - src/types/ (TypeScript types)
   - src/utils/ (utilidades)
   - src/context/ (React contexts)

4. Configuración responsive design:
   - Material-UI theme configurado
   - Breakpoints: mobile (0-767px), tablet (768-1023px), desktop (1024px+)
   - Configuración base para responsive components

DOCKER Y DESARROLLO:
1. Crear docker-compose.yml con:
   - SQL Server para desarrollo
   - Redis para caching
   - Servicios para API y Frontend

2. Scripts de desarrollo:
   - npm run dev (frontend)
   - dotnet run (backend)
   - docker-compose up (base de datos)

CONFIGURACIÓN DE CALIDAD:
1. .gitignore completo:
   - Archivos .NET (bin/, obj/, etc.)
   - Archivos Node.js (node_modules/, dist/, etc.)
   - Archivos IDE y OS
   - Variables de entorno (.env)

2. EditorConfig para consistencia
3. Git hooks con Husky:
   - Pre-commit: lint, format, tests
   - Pre-push: build validation

DOCUMENTACIÓN:
1. README.md principal con:
   - Descripción del proyecto
   - Requisitos de sistema
   - Instrucciones de setup
   - Comandos de desarrollo
   - Estructura del proyecto

2. README.md en backend/ y frontend/ con instrucciones específicas

NOTAS IMPORTANTES:
- Enfoque en MVP web responsive únicamente
- No incluir configuración para mobile nativo o desktop
- Preparar para Azure deployment
- Configurar para Google OAuth desde el inicio
- Estructura escalable para futuras características
- Seguir mejores prácticas de Clean Architecture
- TypeScript estricto en frontend
- Logging y monitoring desde el inicio

ENTREGABLES:
- Proyecto completamente configurado y funcional
- Builds exitosos en backend y frontend
- Docker compose funcionando
- Git repository inicializado
- Documentación completa de setup
```

### US-002: Configurar GitHub repository y Projects

```
Configurar GitHub repository y Projects para DentalTrack MVP:

REPOSITORY SETUP:
1. Crear repository público 'dentaltrack'
2. Configurar repository settings:
   - Description: "DentalTrack - Sistema de gestión de tratamientos dentales con IA"
   - Topics: dental, healthcare, ai, react, dotnet, azure, mvp
   - Features: Issues, Projects, Wiki, Discussions
   - Allow merge commits, squash merging, rebase merging

BRANCH PROTECTION:
1. Configurar branch protection para 'main':
   - Require pull request reviews before merging
   - Require status checks to pass before merging
   - Require branches to be up to date before merging
   - Include administrators in restrictions
   - Allow force pushes: false
   - Allow deletions: false

GITHUB PROJECTS SETUP:
1. Crear nuevo Project (beta):
   - Name: "DentalTrack MVP Development"
   - Description: "MVP development tracking for dental treatment management system"
   - Visibility: Public

2. Configurar views:
   - Board View (Kanban): Backlog, Todo, In Progress, Review, Done
   - Timeline View: Sprint planning y deadlines
   - Table View: Detailed tracking con custom fields

3. Custom fields:
   - Epic (select): Setup, API, React, Auth, Patients, Photos, AI, Reports
   - Sprint (select): Sprint 0, Sprint 1, Sprint 2, etc.
   - Estimation (select): XS, S, M, L, XL
   - Platform (select): Backend, Frontend, DevOps, Documentation

ISSUE TEMPLATES (.github/ISSUE_TEMPLATE/):
[Crear templates completos para user-story.md, bug-report.md, feature-request.md, medical-issue.md]

PULL REQUEST TEMPLATE (.github/PULL_REQUEST_TEMPLATE.md):
[Crear template completo con checklist de responsive design]

LABELS CONFIGURATION:
Crear labels organizados por Priority, Type, Epic, Status

MILESTONES:
Crear milestones para cada sprint con fechas

AUTOMATION RULES:
1. Cuando issue es creado → agregar a project
2. Cuando PR es creado → mover issue a "In Progress"
3. Cuando PR es merged → mover issue a "Done"

ENTREGABLES:
- Repository público configurado completamente
- GitHub Project funcional con todas las vistas
- Issue templates funcionales
- PR template configurado
- Labels y milestones organizados
- Branch protection activo
- Documentación de workflow para el equipo
```

### US-003: Configurar GitHub Actions CI/CD pipeline

```
Configurar CI/CD completo para DentalTrack MVP con GitHub Actions:

ESTRUCTURA DE WORKFLOWS (.github/workflows/):

1. ci.yml - Continuous Integration:
[Workflow completo con backend CI, frontend CI, security scan]

2. cd-staging.yml - Staging Deployment:
[Workflow para deployment automático a Azure staging]

3. cd-production.yml - Production Deployment:
[Workflow para deployment manual a production con approvals]

4. quality-gate.yml - Quality Gates:
[Workflow para quality checks en PRs]

CONFIGURACIÓN DE SECRETS:
En GitHub repository settings > Secrets and variables > Actions:
- AZURE_WEBAPP_PUBLISH_PROFILE_STAGING
- AZURE_WEBAPP_PUBLISH_PROFILE_PROD
- AZURE_STATIC_WEB_APPS_API_TOKEN_STAGING
- AZURE_STATIC_WEB_APPS_API_TOKEN_PROD

BRANCH PROTECTION RULES:
main branch con required status checks

SCRIPTS DE PACKAGE.JSON (frontend):
[Scripts completos para build, test, lint]

CONFIGURACIÓN DE TESTING:
Backend: xUnit, Moq, FluentAssertions, TestContainers
Frontend: Vitest, React Testing Library, MSW

MONITORING Y NOTIFICATIONS:
Status badges en README.md y Slack notifications opcionales

PERFORMANCE BUDGETS:
Frontend y Backend performance requirements

ENTREGABLES:
- CI pipeline funcional con quality gates
- CD pipeline a staging automático
- CD pipeline a production con approval manual
- Security scanning automatizado
- Coverage reports integrados
- Performance monitoring básico
```

### US-004: Configurar infraestructura Azure completa

```
Configurar infraestructura Azure completa para DentalTrack MVP:

ARQUITECTURA AZURE:
[Diagrama completo de recursos Azure]

RESOURCE GROUPS:
1. rg-dentaltrack-staging-eastus
2. rg-dentaltrack-production-eastus

AZURE SQL DATABASE:
Staging y Production con configuraciones específicas
[Scripts completos de az cli]

AZURE BLOB STORAGE:
Configuración para staging y production
[Scripts de configuración con CORS y lifecycle]

AZURE APP SERVICE:
[Configuración completa con auto-scaling]

AZURE STATIC WEB APPS:
[Configuración con staticwebapp.config.json]

AZURE COGNITIVE SERVICES:
Computer Vision para análisis de imágenes

AZURE KEY VAULT:
[Configuración completa con secrets]

AZURE APPLICATION INSIGHTS:
Monitoring y alerts

NETWORK SECURITY:
NSGs y private endpoints para production

TERRAFORM SCRIPTS (Infrastructure as Code):
[Scripts completos de Terraform]

DEPLOYMENT SCRIPTS:
[Scripts bash para deployment automatizado]

ENTREGABLES:
- Infraestructura Azure completa para staging y production
- Terraform scripts para Infrastructure as Code
- Azure Key Vault configurado con secrets
- Monitoring y alerts configurados
- Scripts de deployment automatizados
```

---

## 🔐 ÉPICA 6: AUTENTICACIÓN GOOGLE

### US-015: Implementar Google OAuth web responsive

```
Implementar Google OAuth authentication para DentalTrack MVP (Web Responsive Only):

BACKEND (.NET API) - AUTHENTICATION:

1. Configurar Google OAuth 2.0 en Program.cs:
[Configuración completa de JWT authentication]

2. Crear AuthController.cs:
[Controller completo con endpoints de login, refresh, logout]

3. Crear GoogleAuthService.cs:
[Servicio completo para validación de tokens Google]

4. Crear modelos de datos:
[Todos los DTOs y modelos necesarios]

FRONTEND WEB (REACT) - RESPONSIVE AUTHENTICATION:

1. Instalar dependencias:
npm install @google-cloud/local-auth react-google-login

2. Crear AuthContext.tsx:
[Context completo para manejo de estado de autenticación]

3. Crear GoogleLoginButton.tsx (Responsive):
[Componente responsive con Material-UI]

4. Crear LoginPage.tsx (Responsive):
[Página completa de login responsive]

5. Crear ProtectedRoute.tsx:
[Componente para proteger rutas]

CONFIGURACIÓN DE VARIABLES DE ENTORNO:
Frontend (.env) y Backend (appsettings.json)

GOOGLE CLOUD CONSOLE SETUP:
1. Create new project in Google Cloud Console
2. Enable Google+ API
3. Create OAuth 2.0 credentials
4. Configure authorized origins

RESPONSIVE DESIGN CONSIDERATIONS:
- Touch-friendly button sizes (minimum 44px height)
- Readable text on small screens
- Proper spacing for thumb navigation
- Loading states visible on all screen sizes

SECURITY CONSIDERATIONS:
- HTTPS only for production
- Secure JWT storage
- Token expiration handling
- CSRF protection

NO INCLUIR:
- React Native components
- .NET MAUI authentication
- Expo configuration
- Native mobile OAuth flows

TESTING:
- Unit tests for auth service
- Integration tests for login flow
- Responsive design testing

ENTREGABLES:
- Backend authentication API completo
- Frontend login/logout flow responsive
- JWT token management
- Google OAuth integration funcional
- Error handling robusto
- Security best practices implementadas
```

---

## 📝 ÉPICA 7: GESTIÓN DE PACIENTES

### US-018: Crear y gestionar pacientes con historial médico

```
Implementar gestión completa de pacientes para DentalTrack MVP:

BACKEND (.NET API) - PATIENT MANAGEMENT:

1. Crear entidades de dominio:
[Patient entity con propiedades completas]
[MedicalHistory value object]
[Address value object]

2. Crear Patient repository y services:
[Repository pattern con EF Core]
[Application services con MediatR]

3. Crear API endpoints:
[CRUD completo con validación]
[Search y filtering capabilities]

4. Configurar Entity Framework:
[DbContext configuration]
[Migrations para patient tables]

FRONTEND (REACT) - RESPONSIVE PATIENT MANAGEMENT:

1. Crear Patient types y interfaces:
[TypeScript types completos]

2. Crear PatientService:
[API service con axios]

3. Crear componentes patient management:
- PatientList (responsive table/cards)
- PatientForm (responsive form)
- PatientDetails (responsive view)
- PatientSearch (touch-friendly)

4. Crear pages:
- PatientsPage (main view)
- AddPatientPage (form page)
- EditPatientPage (edit form)
- PatientDetailsPage (details view)

5. Integrar React Query para state management:
[Queries y mutations completas]

RESPONSIVE DESIGN:
- Table responsive que se convierte en cards en mobile
- Forms optimizados para touch
- Navigation adaptable
- Search functionality touch-friendly

VALIDACIÓN:
Backend: FluentValidation rules
Frontend: Zod schemas con react-hook-form

FEATURES INCLUIDOS:
- CRUD completo de pacientes
- Search y filtering
- Pagination
- Validation robusta
- Error handling
- Loading states

NO INCLUIR:
- Mobile native views
- Desktop specific features
- Advanced reporting (eso es otra épica)

ENTREGABLES:
- Backend API completo para patients
- Frontend responsive para gestión de patients
- Validation en frontend y backend
- Error handling robusto
- Tests unitarios y de integración
```

---

## 📸 ÉPICA 8: GESTIÓN DE FOTOS

### US-021: Capturar fotos desde navegador web/mobile

```
Implementar captura de fotos responsive para DentalTrack MVP:

BACKEND (.NET API) - PHOTO MANAGEMENT:

1. Crear Photo entity y relacionar con Patient:
[Photo entity completa con metadata]
[Relación con Patient y Treatment]

2. Configurar Azure Blob Storage integration:
[Service para upload a Azure Blob]
[Configuration para diferentes containers]

3. Crear Photo API endpoints:
[Upload endpoint con validation]
[Get photos by patient/treatment]
[Delete photo endpoint]

4. Implementar image processing básico:
[Resize y compress automático]
[Thumbnail generation]
[Metadata extraction]

FRONTEND (REACT) - RESPONSIVE PHOTO CAPTURE:

1. Crear PhotoCapture component:
[Camera API integration para web/mobile browsers]
[Responsive camera interface]
[Touch-friendly controls]

2. Crear PhotoGallery component:
[Grid responsive de fotos]
[Lightbox para viewing]
[Touch gestures para mobile]

3. Crear PhotoUpload component:
[Drag & drop para desktop]
[File picker para mobile]
[Progress indicators]

4. Implementar camera access:
[getUserMedia API]
[Multiple camera support]
[Image capture y preview]

RESPONSIVE CONSIDERATIONS:
- Camera interface adaptable a orientación
- Touch controls optimizados
- Gesture support para zoom/pan
- Loading states visibles
- Error handling para camera access

BROWSER COMPATIBILITY:
- Chrome/Safari camera API
- Fallback para browsers sin camera support
- File upload como alternativa

FEATURES:
- Capture desde camera web/mobile
- Upload de archivos existentes
- Preview antes de guardar
- Metadata automático (fecha, device, etc.)
- Progress tracking para uploads
- Error handling robusto

SECURITY:
- File type validation
- Size limits
- Secure blob storage access
- Image processing server-side

NO INCLUIR:
- Native camera apps
- Advanced image editing
- AI analysis (eso es otra épica)

ENTREGABLES:
- Backend photo management API
- Frontend responsive photo capture
- Azure Blob Storage integration
- Camera API integration funcional
- Upload/download functionality
- Error handling completo
```

---

## 🤖 ÉPICA 9: IA BÁSICA

### US-024: Análisis automático básico de progreso dental

```
Implementar análisis básico de IA para progreso dental con Azure Cognitive Services:

BACKEND (.NET API) - AI ANALYSIS:

1. Configurar Azure Cognitive Services:
[Computer Vision API integration]
[Configuration y secrets management]

2. Crear AI Analysis service:
[Service para análisis de color dental]
[Progress calculation algorithms]
[Result storage y caching]

3. Crear Analysis entity y endpoints:
[Analysis entity con results]
[API endpoints para trigger analysis]
[Get analysis results]

4. Implementar análisis básico:
[Color extraction from dental photos]
[Comparison algorithms]
[Progress scoring]

FRONTEND (REACT) - AI RESULTS DISPLAY:

1. Crear AI Analysis components:
[AnalysisResults responsive display]
[ProgressChart responsive charts]
[BeforeAfterComparison touch-friendly]

2. Crear análisis triggers:
[Button para trigger analysis]
[Loading states durante processing]
[Results display cuando completo]

3. Implementar visualización de resultados:
[Charts responsive con Chart.js]
[Color visualization]
[Progress indicators]

AI FEATURES:
- Análisis automático de color dental
- Calculation de progreso entre fotos
- Visual comparison tools
- Progress scoring (0-100)
- Trend analysis básico

AZURE COGNITIVE SERVICES INTEGRATION:
- Computer Vision API para color analysis
- Custom analysis para dental specific metrics
- Batch processing para múltiples imágenes
- Result caching para performance

RESPONSIVE DESIGN:
- Charts adaptables a screen size
- Touch-friendly comparison tools
- Mobile-optimized result display
- Gesture support para image comparison

ERROR HANDLING:
- AI service failures
- Invalid image formats
- Network connectivity issues
- Graceful degradation

NO INCLUIR:
- Machine learning training
- Advanced AI models
- Real-time analysis
- Video analysis

ENTREGABLES:
- Azure Cognitive Services integration
- Basic dental progress analysis
- Responsive results visualization
- Error handling robusto
- Performance optimization
```

---

## 📊 ÉPICA 10: REPORTES

### US-027: Generar reportes de progreso por paciente

```
Implementar sistema de reportes para DentalTrack MVP:

BACKEND (.NET API) - REPORTING:

1. Crear Report entity y services:
[Report entity con metadata]
[ReportGenerator service]
[PDF generation con iTextSharp]

2. Crear reporting endpoints:
[Generate report API]
[Get available reports]
[Download report PDF]

3. Implementar report templates:
[Patient progress report template]
[Treatment summary template]
[Before/after comparison template]

FRONTEND (REACT) - RESPONSIVE REPORTS:

1. Crear Report components:
[ReportViewer responsive component]
[ReportGenerator form]
[ReportList con filters]

2. Crear print-friendly layouts:
[CSS print media queries]
[PDF preview component]
[Export functionality]

3. Implementar dashboard básico:
[KPI cards responsive]
[Charts para treatment statistics]
[Patient progress summaries]

REPORT FEATURES:
- Patient progress reports
- Treatment summaries
- Before/after comparisons
- Progress charts y graphs
- Export to PDF
- Print functionality

RESPONSIVE DESIGN:
- Reports adaptables a screen size
- Print-friendly layouts
- Mobile report viewing
- Touch-friendly controls

CHARTS Y VISUALIZATION:
- Progress charts con Chart.js
- Before/after image comparisons
- Treatment timeline visualization
- KPI displays

NO INCLUIR:
- Advanced analytics
- Custom report builder
- Real-time dashboards
- Multi-tenant reporting

ENTREGABLES:
- Report generation system
- PDF export functionality
- Responsive report viewing
- Basic dashboard con KPIs
- Print-friendly layouts
```

---

## 🎯 Instrucciones Finales

### ✅ Checklist para cada Prompt:
- [ ] Copiar prompt completo sin modificaciones
- [ ] Verificar que el código generado compile sin errores
- [ ] Validar responsive design en múltiples dispositivos
- [ ] Ejecutar tests unitarios
- [ ] Verificar security best practices
- [ ] Documentar cualquier customización necesaria

### 📱 Responsive Requirements (Todos los Prompts):
- **Mobile First**: 375px+ width support
- **Touch Friendly**: 44px minimum touch targets
- **Gesture Support**: Swipe, pinch, tap donde aplique
- **Performance**: <2s load times en mobile
- **Accessibility**: Basic ARIA support

### 🔐 Security Requirements (Todos los Prompts):
- **HTTPS Only**: All authentication y sensitive data
- **Input Validation**: Frontend y backend validation
- **Error Handling**: No sensitive data in error messages
- **JWT Security**: Proper token expiration y refresh
- **CORS Configuration**: Proper origins configuration

### 🧪 Testing Requirements (Todos los Prompts):
- **Unit Tests**: >80% coverage para business logic
- **Integration Tests**: API endpoint testing
- **Responsive Testing**: Multiple device sizes
- **Cross-browser**: Chrome, Safari, Firefox, Edge
- **Performance Testing**: Load time validation

---

📅 **Última actualización**: 2025-01-20  
🎯 **Versión**: MVP 1.0  
🤖 **Compatible con**: Claude Code latest version