import tailwindcss from '@tailwindcss/vite'
import vue from '@vitejs/plugin-vue'
import { defineConfig, loadEnv } from 'vite'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')
  const backend = env.BACKEND_URL || 'http://localhost:5080'

  return {
    plugins: [vue(), tailwindcss()],
    resolve: {
      alias: {
        '@': new URL('./src', import.meta.url).pathname,
      },
    },
    server: {
      port: 5173,
      strictPort: true,
      // Same origin for FE and API in dev, so CORS stays off.
      proxy: {
        '/api': { target: backend, changeOrigin: false },
      },
    },
  }
})
