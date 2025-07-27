# DentalTrack Frontend - Claude Context

## Project Overview
DentalTrack is a dental practice management application built with React, TypeScript, and Material-UI.

## Architecture & Tech Stack
- **Frontend**: React 19.1.0 + TypeScript + Vite
- **UI Library**: Material-UI (MUI) v7.2.0
- **State Management**: React Query (@tanstack/react-query) + React Context
- **HTTP Client**: Axios v1.11.0
- **Routing**: React Router Dom v7.7.0
- **Form Handling**: React Hook Form + Zod validation
- **Testing**: Vitest + React Testing Library

## Commands
```bash
# Development
npm run dev

# Build & Quality Checks
npm run build          # TypeScript compilation + Vite build
npm run lint           # ESLint checking
npm run type-check     # TypeScript type checking only

# Testing
npm run test           # Run Vitest tests
npm run test:coverage  # Run tests with coverage
```

## Project Structure
```
src/
├── api/                 # API client and configuration
│   ├── client.ts       # Axios instance with interceptors
│   ├── endpoints.ts    # API endpoint constants
│   ├── types.ts        # API response/error types
│   └── index.ts        # API exports
├── components/
│   └── common/         # Reusable components
│       ├── ErrorBoundary.tsx
│       ├── LoadingSpinner.tsx
│       └── NotificationSystem.tsx
├── context/            # React Context providers
│   └── AppContext.tsx  # Global app state management
├── hooks/              # Custom React hooks
│   └── useApi.ts       # API interaction hooks
├── providers/          # Provider components
│   └── QueryProvider.tsx # React Query configuration
├── App.tsx
└── main.tsx
```

## Implemented Features

### US-009: API State Management ✅
- **API Client**: Configured Axios with base URL, interceptors, and error handling
- **React Query Integration**: Caching, background updates, retry logic
- **Custom Hooks**:
  - `useApiQuery` - GET requests with caching
  - `useApiMutation` - POST requests with cache invalidation
  - `useApiUpdateMutation` - PUT requests
  - `useApiDeleteMutation` - DELETE requests
  - `useOptimisticUpdate` - Optimistic UI updates
- **Global State**: App context with reducer pattern for UI state
- **Notification System**: Toast notifications with auto-hide
- **Error Handling**: Global error boundary and error types
- **TypeScript**: Fully typed API responses and errors

## Code Standards
- **TypeScript**: Strict mode enabled, no `any` types allowed
- **Imports**: Use type-only imports for types (`import type { ... }`)
- **Linting**: ESLint with TypeScript rules, no warnings allowed
- **Components**: Functional components with TypeScript interfaces
- **State**: Prefer React Query for server state, Context for UI state

## API Configuration
- Base URL configured via environment variables
- Request/response interceptors for auth and error handling
- Automatic retry logic for failed requests (3 attempts with exponential backoff)
- Cache configuration: 5min stale time, 10min garbage collection

## Testing Guidelines
- Unit tests with Vitest and React Testing Library
- Component testing with user interactions
- API hook testing with MSW (Mock Service Worker)
- Coverage reports available via `npm run test:coverage`

## Recent PRs
- [#7](https://github.com/amajail/dentaltrack/pull/7) - feat: Implement US-009 API State Management

## Development Notes
- React Query DevTools enabled in development mode
- Hot module replacement configured with Vite
- Material-UI theme system ready for customization
- Form validation with Zod schemas
- Responsive design with Material-UI breakpoints