# 🦷 DentalTrack

Sistema integral de gestión de tratamientos dentales con análisis de IA, enfocado inicialmente en blanqueamiento dental para MVP.

## 📋 Descripción

DentalTrack es una plataforma completa que permite a los profesionales dentales y pacientes gestionar, monitorear y analizar tratamientos dentales utilizando inteligencia artificial. El sistema proporciona herramientas avanzadas para el seguimiento de progreso, análisis de imágenes y recomendaciones personalizadas.

### 🎯 Características Principales

- **📊 Gestión de Pacientes**: Registro completo de pacientes y historial médico
- **🔬 Análisis de IA**: Análisis automático de imágenes dentales
- **📈 Seguimiento de Progreso**: Monitoreo en tiempo real del tratamiento
- **📱 Multiplataforma**: Web, móvil y desktop
- **🔒 HIPAA Compliant**: Cumple con estándares de privacidad médica
- **📋 Reportes Avanzados**: Generación automática de informes detallados

## 🏗️ Arquitectura del Sistema

### Backend (.NET 8 + Clean Architecture)
```
backend/
├── src/
│   ├── DentalTrack.Api/          # API REST
│   ├── DentalTrack.Application/  # Lógica de negocio
│   ├── DentalTrack.Domain/       # Entidades y reglas de dominio
│   └── DentalTrack.Infrastructure/ # Acceso a datos y servicios externos
└── tests/                        # Pruebas unitarias e integración
```

### Frontend Web (React + TypeScript + Material-UI)
```
frontend/
├── src/
│   ├── components/     # Componentes reutilizables
│   ├── pages/         # Páginas de la aplicación
│   ├── services/      # Servicios de API
│   ├── hooks/         # Custom hooks
│   └── types/         # Tipos TypeScript
└── public/            # Archivos estáticos
```

### Mobile (React Native + Expo)
```
mobile/
├── src/
│   ├── screens/       # Pantallas de la app
│   ├── components/    # Componentes reutilizables
│   ├── navigation/    # Configuración de navegación
│   └── services/      # Servicios de API
└── assets/            # Imágenes y recursos
```

### Desktop (.NET Console - Base para MAUI)
```
desktop/
└── DentalTrack.Desktop/  # Aplicación de escritorio
```

## 🚀 Configuración del Entorno de Desarrollo

### Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker](https://www.docker.com/)
- [SQL Server](https://www.microsoft.com/sql-server) o Docker container

### 🐳 Configuración con Docker (Recomendado)

1. **Clonar el repositorio:**
```bash
git clone https://github.com/amajail/dentaltrack.git
cd dentaltrack
```

2. **Ejecutar con Docker Compose:**
```bash
docker-compose up -d
```

Esto iniciará:
- 🗄️ SQL Server (puerto 1433)
- 🔴 Redis (puerto 6379)
- 🌐 API Backend (puerto 5000)
- 💻 Frontend Web (puerto 3000)

### 🔧 Configuración Manual

#### Backend
```bash
cd backend
dotnet restore
dotnet build
dotnet run --project src/DentalTrack.Api
```

#### Frontend
```bash
cd frontend
npm install
npm run dev
```

#### Mobile
```bash
cd mobile
npm install
npm start
```

## 📚 API Documentation

Una vez que el backend esté ejecutándose, accede a la documentación de la API:

- **Swagger UI**: http://localhost:5000/swagger
- **ReDoc**: http://localhost:5000/redoc

## 🧪 Testing

### Backend
```bash
cd backend
dotnet test
```

### Frontend
```bash
cd frontend
npm test
npm run test:coverage
```

### Mobile
```bash
cd mobile
npm test
```

## 📦 Deployment

### CI/CD Pipeline

El proyecto incluye GitHub Actions para:
- ✅ Pruebas automatizadas
- 🔍 Análisis de código
- 🐳 Build y push de imágenes Docker
- 🔒 Escaneo de seguridad

### Ambientes

- **Development**: Desarrollo local
- **Staging**: Pre-producción
- **Production**: Producción

## 🗺️ Roadmap

### MVP (Blanqueamiento Dental)
- [ ] Registro de pacientes
- [ ] Captura de imágenes dentales
- [ ] Análisis básico de IA
- [ ] Seguimiento de progreso
- [ ] Reportes simples

### Fase 2
- [ ] Más tipos de tratamientos
- [ ] IA avanzada
- [ ] Integración con dispositivos IoT
- [ ] Dashboard analytics

### Fase 3
- [ ] Telemedicina
- [ ] Marketplace de productos
- [ ] Integración con clínicas
- [ ] App para pacientes

## 🤝 Contribución

1. Fork el proyecto
2. Crea tu feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

### 📝 Convenciones de Código

- **Backend**: Seguir convenciones de C# y Clean Architecture
- **Frontend**: ESLint + Prettier para React/TypeScript
- **Mobile**: React Native best practices
- **Commits**: Conventional Commits

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

## 👥 Equipo

- **Backend**: Clean Architecture, Entity Framework, MediatR
- **Frontend**: React, Material-UI, TypeScript
- **Mobile**: React Native, Expo
- **DevOps**: Docker, GitHub Actions, Azure/AWS
- **AI/ML**: TensorFlow, OpenCV, Azure Cognitive Services

## 🆘 Soporte

- 📧 Email: support@dentaltrack.com
- 💬 Discord: [DentalTrack Community](https://discord.gg/dentaltrack)
- 📖 Documentation: [docs.dentaltrack.com](https://docs.dentaltrack.com)
- 🐛 Issues: [GitHub Issues](https://github.com/amajail/dentaltrack/issues)

## 🔗 Links Útiles

- [📊 Project Board](https://github.com/amajail/dentaltrack/projects)
- [📋 Issues](https://github.com/amajail/dentaltrack/issues)
- [🔄 Pull Requests](https://github.com/amajail/dentaltrack/pulls)
- [📈 Analytics](https://github.com/amajail/dentaltrack/pulse)

---

**DentalTrack** - Transformando el cuidado dental con tecnología 🦷✨