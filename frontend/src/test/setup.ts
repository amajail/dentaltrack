import '@testing-library/jest-dom'

// Mock SVG imports
vi.mock('/vite.svg', () => ({
  default: 'test-file-stub',
}))

vi.mock('./assets/react.svg', () => ({
  default: 'test-file-stub',
}))

// Mock CSS imports
vi.mock('./App.css', () => ({}))
vi.mock('./index.css', () => ({}))