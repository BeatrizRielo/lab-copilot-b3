import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Proxy /api para o backend ASP.NET Core em desenvolvimento.
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true
      }
    }
  }
})
