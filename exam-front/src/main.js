import { createApp } from 'vue'
import App from '@/app/App.vue'
import { installProviders } from '@/app/providers'
import '@/styles/index.css'

const app = createApp(App)
installProviders(app)
app.mount('#app')
