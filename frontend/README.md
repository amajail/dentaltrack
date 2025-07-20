# 🌐 DentalTrack Frontend

Aplicación web responsive desarrollada en React 18+ con TypeScript para el sistema DentalTrack.

## 🎯 Características

- **📱 100% Responsive**: Diseño mobile-first que funciona en todos los dispositivos
- **⚡ Performance**: Carga rápida con Vite y lazy loading
- **🎨 Material Design**: UI consistente con Material-UI
- **🔐 Autenticación**: Google OAuth 2.0 integrado
- **📊 Estado Global**: React Query para cache y sincronización
- **✅ Validación**: React Hook Form + Zod schemas

## 🏗️ Arquitectura

```
frontend/
├── src/
│   ├── components/           # 🧱 Componentes reutilizables
│   │   ├── common/          # Componentes base (Button, Input, etc.)
│   │   ├── forms/           # Formularios específicos
│   │   └── layout/          # Layout components (Header, Sidebar)
│   ├── pages/               # 📄 Páginas principales
│   │   ├── auth/            # Login, registro
│   │   ├── patients/        # Gestión de pacientes
│   │   ├── treatments/      # Gestión de tratamientos
│   │   └── dashboard/       # Dashboard principal
│   ├── hooks/               # 🪝 Custom React hooks
│   │   ├── useAuth.ts       # Hook de autenticación
│   │   └── useApi.ts        # Hook para API calls
│   ├── services/            # 🔌 Servicios y API
│   │   ├── api.ts           # Cliente HTTP (Axios)
│   │   ├── auth.service.ts  # Servicio de autenticación
│   │   └── patients.service.ts # Servicio de pacientes
│   ├── types/               # 📝 TypeScript types
│   │   ├── api.types.ts     # Tipos de API
│   │   └── auth.types.ts    # Tipos de autenticación
│   ├── utils/               # 🛠️ Utilidades
│   │   ├── constants.ts     # Constantes
│   │   ├── helpers.ts       # Funciones helper
│   │   └── validation.ts    # Schemas de validación
│   └── context/             # 🌍 React contexts
│       ├── AuthContext.tsx  # Contexto de autenticación
│       └── ThemeContext.tsx # Contexto de tema
├── public/                  # 📁 Archivos estáticos
└── dist/                    # 📦 Build de producción
```

## 🚀 Quick Start

### Prerrequisitos
- [Node.js 20+](https://nodejs.org/)
- [npm](https://www.npmjs.com/) o [yarn](https://yarnpkg.com/)

### 1. Instalar dependencias
```bash
npm install
```

### 2. Configurar variables de entorno
```bash
# Crear archivo .env.local
cp .env.example .env.local

# Editar con tus valores
VITE_API_BASE_URL=http://localhost:5000
VITE_GOOGLE_CLIENT_ID=your-google-client-id
```

### 3. Ejecutar en desarrollo
```bash
npm run dev
```

La aplicación estará disponible en:
- **Development**: http://localhost:3000
- **Preview**: http://localhost:4173 (después de `npm run preview`)

## 📦 Tecnologías

| Categoría | Tecnología | Versión | Propósito |
|-----------|------------|---------|-----------|
| **Framework** | React | 19.1.0 | UI Framework |
| **Language** | TypeScript | 5.8.3 | Type safety |
| **Build Tool** | Vite | 7.0.4 | Build & dev server |
| **UI Library** | Material-UI | 7.2.0 | Component library |
| **Icons** | MUI Icons | 7.2.0 | Iconografía |
| **State Management** | React Query | 5.83.0 | Server state |
| **Forms** | React Hook Form | 7.60.0 | Form management |
| **Validation** | Zod | 4.0.5 | Schema validation |
| **HTTP Client** | Axios | 1.10.0 | API requests |
| **Routing** | React Router | 7.7.0 | Client-side routing |
| **Date Picker** | MUI X Date Pickers | 8.9.0 | Date components |

## 📱 Responsive Design

### Breakpoints
```typescript
const breakpoints = {
  mobile: '0px-767px',    // Móviles
  tablet: '768px-1023px', // Tablets  
  desktop: '1024px+'      // Desktop
};
```

### Guidelines
- **Mobile First**: Diseño inicial para móviles
- **Touch Friendly**: Botones de 44px+ para touch
- **Readable Text**: Mínimo 16px en móvil
- **Responsive Images**: Usando srcSet y sizes
- **Flexible Layouts**: CSS Grid y Flexbox

## 🎨 Design System

### Colores Principales
```typescript
const theme = {
  primary: '#1976d2',      // Azul principal
  secondary: '#dc004e',    // Rojo accent
  success: '#2e7d32',      // Verde success
  warning: '#ed6c02',      // Naranja warning
  error: '#d32f2f',        // Rojo error
  background: '#f5f5f5',   // Fondo gris claro
};
```

### Tipografía
- **Heading**: Roboto Bold
- **Body**: Roboto Regular  
- **Code**: Roboto Mono

## 🧪 Testing

```bash
# Ejecutar tests (cuando estén configurados)
npm test

# Coverage
npm run test:coverage

# E2E tests (futuro)
npm run test:e2e
```

## 🔧 Scripts Disponibles

```bash
# Desarrollo
npm run dev              # Servidor de desarrollo

# Build
npm run build           # Build de producción
npm run preview         # Preview del build

# Linting
npm run lint            # ESLint check
npm run lint:fix        # ESLint fix

# Type checking
npm run type-check      # TypeScript check
```

## 📁 Estructura de Componentes

### Organización
```typescript
// Componente base
components/
├── Button/
│   ├── Button.tsx
│   ├── Button.types.ts
│   ├── Button.styles.ts
│   └── index.ts

// Página completa
pages/
├── Dashboard/
│   ├── Dashboard.tsx
│   ├── Dashboard.hooks.ts
│   ├── Dashboard.types.ts
│   └── index.ts
```

### Convenciones
- **PascalCase** para componentes
- **camelCase** para hooks y utilities
- **kebab-case** para archivos CSS
- **UPPER_CASE** para constantes

## 🔐 Autenticación

### Google OAuth Setup
```typescript
// En .env.local
VITE_GOOGLE_CLIENT_ID=your-google-client-id

// Uso en componente
const { login, logout, user } = useAuth();
```

### Protected Routes
```typescript
// Proteger rutas
<ProtectedRoute>
  <PatientsList />
</ProtectedRoute>
```

## 🌍 Variables de Entorno

```bash
# API Configuration
VITE_API_BASE_URL=http://localhost:5000
VITE_API_TIMEOUT=30000

# Authentication
VITE_GOOGLE_CLIENT_ID=your-google-client-id

# Features Flags
VITE_ENABLE_AI_ANALYSIS=true
VITE_ENABLE_PHOTO_CAPTURE=true

# Environment
VITE_ENVIRONMENT=development
```

## 📊 Performance Targets

- **First Contentful Paint**: <2s
- **Largest Contentful Paint**: <2.5s
- **Time to Interactive**: <3s
- **Cumulative Layout Shift**: <0.1
- **Bundle Size**: <500KB gzipped

## 🔄 State Management

### React Query
```typescript
// Queries para data fetching
const { data, loading, error } = useQuery({
  queryKey: ['patients'],
  queryFn: fetchPatients
});

// Mutations para updates
const mutation = useMutation({
  mutationFn: createPatient,
  onSuccess: () => queryClient.invalidateQueries(['patients'])
});
```

### Local State
```typescript
// Formularios con React Hook Form
const { register, handleSubmit, formState } = useForm({
  resolver: zodResolver(patientSchema)
});
```

## 📱 PWA Ready (Futuro)

- Service Worker configurado
- Manifest.json para instalación
- Offline functionality
- Push notifications

## 🔗 Links Relacionados

- [🔧 Backend README](../backend/README.md)
- [📋 User Stories](../docs/user-stories/README.md)
- [🎨 Design System](../docs/design-system.md)
- [☁️ Deployment Guide](../docs/deployment/azure-setup.md)

---

**🦷 DentalTrack Frontend** - Desarrollado con React 18+ y Material Design
