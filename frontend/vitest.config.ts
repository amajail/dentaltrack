import { defineConfig } from 'vitest/config'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts'],
    globals: true,
    coverage: {
      provider: 'v8',
      reporter: ['text', 'json', 'json-summary', 'html'],
      exclude: [
        'coverage/**',
        'dist/**',
        '**/node_modules/**',
        '**/src/test/**',
        '**/*.d.ts',
        '**/*.config.*',
        '**/vite-env.d.ts'
      ],
      statements: 80,
      branches: 80,
      functions: 80,
      lines: 80
    }
  },
})