import react from '@vitejs/plugin-react'
import { defineConfig } from 'vitest/config'

export default defineConfig({
  plugins: [react()],
  server: {
    // The API runs on its own port; the browser only ever talks to this origin.
    proxy: { '/api': 'http://localhost:5080' },
  },
  test: {
    environment: 'jsdom',
  },
})
