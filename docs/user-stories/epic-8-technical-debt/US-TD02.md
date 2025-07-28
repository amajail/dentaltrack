# US-TD02: Optimize frontend bundle size and performance

## 📋 User Story
**Como** usuario del sistema  
**Quiero** que la aplicación cargue rápidamente en cualquier dispositivo  
**Para que** tenga una experiencia fluida y eficiente

## ✅ Criterios de Aceptación

### 📦 Bundle Size Optimization
- [ ] JavaScript bundle size: 428KB → ≤300KB (30% reduction)
- [ ] Total bundle size: <500KB (including CSS, images)
- [ ] Gzip compression: <150KB for main bundle
- [ ] Individual chunk sizes: <100KB per lazy-loaded chunk
- [ ] Vendor bundle separation: Libraries in separate chunks

### ⚡ Performance Metrics
- [ ] First Contentful Paint (FCP): <1.5s
- [ ] Largest Contentful Paint (LCP): <2.5s  
- [ ] Time to Interactive (TTI): <3.5s
- [ ] Total Blocking Time (TBT): <200ms
- [ ] Cumulative Layout Shift (CLS): <0.1

### 🚀 Loading Optimization
- [ ] Code splitting: Route-based lazy loading
- [ ] Tree shaking: Remove unused library code
- [ ] Dynamic imports: Load components on demand
- [ ] Image optimization: WebP format, lazy loading
- [ ] Font optimization: Preload critical fonts

## 🛠️ Technical Implementation

### Bundle Analysis
```bash
# Current bundle breakdown (428KB):
- Material-UI: ~180KB (42%)
- React + React DOM: ~120KB (28%)
- React Query: ~45KB (10.5%)
- Lucide Icons: ~25KB (5.8%)
- Other dependencies: ~58KB (13.5%)
```

### Optimization Strategies

#### 1. Code Splitting & Lazy Loading
```typescript
// Route-based code splitting
const DashboardPage = lazy(() => import('../pages/DashboardPage'));
const PatientsPage = lazy(() => import('../pages/PatientsPage'));
const TreatmentsPage = lazy(() => import('../pages/TreatmentsPage'));

// Component-based lazy loading
const HeavyChart = lazy(() => import('../components/charts/HeavyChart'));
```

#### 2. Material-UI Optimization
```typescript
// Tree shake Material-UI imports
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
// Instead of: import { Button, TextField } from '@mui/material';

// Custom Material-UI build with only needed components
import { createTheme } from '@mui/material/styles';
// Configure theme to include only required components
```

#### 3. Icon Optimization
```typescript
// Replace Lucide icons with Material-UI icons where possible
import RefreshIcon from '@mui/icons-material/Refresh';
// Instead of: import { RefreshCcw } from 'lucide-react';

// Dynamic icon imports
const IconComponent = lazy(() => import(`@mui/icons-material/${iconName}`));
```

#### 4. Vendor Bundle Splitting
```typescript
// vite.config.ts
export default defineConfig({
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['react', 'react-dom'],
          mui: ['@mui/material', '@mui/icons-material'],
          query: ['@tanstack/react-query'],
        }
      }
    }
  }
});
```

#### 5. Image & Asset Optimization
```typescript
// WebP image format with fallbacks
<picture>
  <source srcSet="image.webp" type="image/webp" />
  <img src="image.jpg" alt="Description" loading="lazy" />
</picture>

// SVG optimization and inlining for small icons
import { ReactComponent as LogoIcon } from '../assets/logo.svg';
```

## 📊 Current Performance Analysis

### Bundle Composition (428KB)
```
🏗️ Core Framework (120KB - 28%):
- React: ~45KB
- React DOM: ~75KB

🎨 UI Library (180KB - 42%):
- Material-UI Core: ~130KB
- Material-UI Icons: ~35KB
- Material-UI System: ~15KB

🔄 State Management (45KB - 10.5%):
- React Query: ~35KB
- Axios: ~10KB

🎯 Icons & Graphics (25KB - 5.8%):
- Lucide React: ~25KB

📦 Other Dependencies (58KB - 13.5%):
- TypeScript helpers: ~15KB
- Utilities & polyfills: ~43KB
```

### Performance Bottlenecks
```
❌ Large Bundles:
- Material-UI over-inclusion
- Unused icon imports
- No code splitting

❌ Loading Issues:
- Single large bundle
- No progressive loading
- Heavy initial load

❌ Asset Issues:
- Unoptimized images
- No lazy loading
- Large font files
```

## 🎯 Optimization Plan

### Phase 1: Bundle Analysis (2 days)
- [ ] Install bundle analyzer tools
- [ ] Identify unused dependencies
- [ ] Map component usage patterns
- [ ] Analyze Material-UI usage

### Phase 2: Code Splitting (3 days)
- [ ] Implement route-based lazy loading
- [ ] Split vendor bundles
- [ ] Create component lazy loading
- [ ] Configure Vite chunk optimization

### Phase 3: Dependency Optimization (3 days)  
- [ ] Optimize Material-UI imports
- [ ] Replace heavy dependencies
- [ ] Remove unused icons
- [ ] Tree shake all libraries

### Phase 4: Asset Optimization (2 days)
- [ ] Convert images to WebP
- [ ] Implement image lazy loading
- [ ] Optimize font loading
- [ ] Compress static assets

### Phase 5: Performance Testing (2 days)
- [ ] Lighthouse performance audits
- [ ] Bundle size verification
- [ ] Load time testing
- [ ] Mobile performance validation

## 📈 Expected Results

### Bundle Size Reduction
```
Before Optimization:
- Total: 428KB
- Gzipped: 134KB

After Optimization:
- Total: ≤300KB (-30%)
- Gzipped: ≤100KB (-25%)
- Initial chunk: ≤150KB
- Vendor chunk: ≤100KB
- Route chunks: ≤50KB each
```

### Performance Improvements
```
Metrics Improvement:
- FCP: 2.1s → <1.5s (-29%)
- LCP: 3.2s → <2.5s (-22%)
- TTI: 4.5s → <3.5s (-22%)
- Bundle load: 850ms → <500ms (-41%)
```

## 🧪 Testing Strategy

### Performance Testing
- [ ] Lighthouse CI integration
- [ ] Bundle size monitoring in CI/CD
- [ ] Real User Monitoring (RUM) setup
- [ ] Performance regression detection

### Device Testing
- [ ] Mobile performance (3G network)
- [ ] Tablet optimization
- [ ] Desktop performance
- [ ] Cross-browser validation

## 📋 Definition of Done
- [ ] JavaScript bundle ≤300KB
- [ ] All Lighthouse scores >90
- [ ] Performance budget enforced in CI/CD
- [ ] No performance regressions
- [ ] Mobile performance optimized
- [ ] Documentation updated

## 🔗 Dependencies
- **Requires**: Bundle analysis tools
- **Enables**: Better user experience, mobile performance
- **Blocks**: None (enhancement)

---

📅 **Creado**: 2025-07-28  
🎯 **Sprint**: Sprint 4 (Performance)  
👤 **Asignado**: Frontend Team  
🔄 **Estado**: 📋 Ready for Development  
⏰ **Estimación**: L (5 puntos)  
🏷️ **Labels**: technical-debt, performance, frontend, optimization