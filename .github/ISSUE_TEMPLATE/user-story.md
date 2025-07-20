---
name: User Story
about: Create a new user story for the DentalTrack backlog
title: 'US-XXX: [Title]'
labels: ['user-story', 'needs-triage']
assignees: ''
---

## 📋 User Story
**Como** [tipo de usuario]  
**Quiero** [objetivo]  
**Para que** [beneficio]

## ✅ Criterios de Aceptación

### 🎯 Functional Requirements
- [ ] Criterio específico y testeable 1
- [ ] Criterio específico y testeable 2
- [ ] Criterio específico y testeable 3

### 📱 Responsive Requirements
- [ ] Funciona correctamente en desktop (1024px+)
- [ ] Funciona correctamente en tablet (768px-1023px)
- [ ] Funciona correctamente en mobile (320px-767px)
- [ ] Touch-friendly en dispositivos móviles

### 🔒 Security Requirements
- [ ] Validación de input en frontend y backend
- [ ] Manejo seguro de datos sensibles
- [ ] Autorización apropiada implementada
- [ ] Error handling que no expone información sensible

## 🛠️ Claude Code Prompt
```
[Incluir aquí el prompt completo y específico para Claude Code]

CONTEXTO: DentalTrack MVP - Web Responsive Only

REQUIREMENTS:
- Platform: Web responsive (desktop/tablet/mobile browsers)
- Tech Stack: .NET 8 API + React 18+ TypeScript
- Design: Material-UI responsive components
- Authentication: Google OAuth with JWT
- Database: Azure SQL with EF Core
- Storage: Azure Blob Storage for images
- AI: Azure Cognitive Services for analysis

RESPONSIVE DESIGN:
- Mobile First approach
- Touch-friendly controls (44px minimum)
- Breakpoints: 320px, 768px, 1024px
- Flexible layouts with Material-UI Grid
- Readable typography on all screen sizes

SECURITY:
- HTTPS only
- Input validation frontend + backend
- JWT tokens with proper expiration
- Secure API endpoints with authorization
- CORS properly configured

NO INCLUDE:
- React Native components
- .NET MAUI desktop features
- Expo mobile app features
- Advanced AI/ML training

[Prompt detallado específico para la implementación]
```

## 📝 Notas Técnicas
- **Epic**: [Nombre de la épica]
- **Dependencias**: [US-XXX, US-YYY] o "Ninguna"
- **Estimación**: [XS/S/M/L/XL] - [1/2/3/5/8] story points
- **Platform**: Web Responsive MVP
- **Sprint**: [Sprint X] (tentativo)

## 🧪 Definition of Done
- [ ] Código implementado siguiendo Clean Architecture
- [ ] Code review completado y aprobado
- [ ] Tests unitarios escritos con >80% coverage
- [ ] Tests de integración pasando
- [ ] Responsive design validado en múltiples dispositivos
- [ ] Cross-browser testing completado (Chrome, Safari, Firefox, Edge)
- [ ] Performance optimizado (<2s carga, <200ms API responses)
- [ ] Security review completado
- [ ] Documentación técnica actualizada
- [ ] Deploy a staging exitoso
- [ ] Acceptance criteria validados por PO
- [ ] No vulnerabilidades críticas en security scan

## 🔗 Links Relacionados
- **Epic**: [Link a la épica en GitHub Project]
- **Documentación**: [Link a docs técnicos relevantes]
- **Mockups**: [Link a diseños si aplica]
- **Dependencies**: [Links a user stories dependientes]

## 📋 Tasks de Implementación
- [ ] [Task específico 1]
- [ ] [Task específico 2]
- [ ] [Task específico 3]
- [ ] [Testing y validation]

## 🎯 Acceptance Test Scenarios

### Scenario 1: [Nombre del escenario]
**Given** [contexto inicial]  
**When** [acción del usuario]  
**Then** [resultado esperado]

### Scenario 2: [Nombre del escenario]
**Given** [contexto inicial]  
**When** [acción del usuario]  
**Then** [resultado esperado]

## 📱 Mobile/Responsive Considerations
- [ ] Touch targets mínimo 44px de altura
- [ ] Text legible sin zoom en mobile
- [ ] Navigation accesible con thumb en mobile
- [ ] Loading states visibles en pantallas pequeñas
- [ ] Error messages apropiadamente sized
- [ ] Forms optimizados para mobile input

---

📅 **Creado**: [Fecha]  
👤 **Reporter**: [Nombre]  
🎯 **Sprint Target**: [Sprint X]  
🔄 **Estado**: Needs Triage