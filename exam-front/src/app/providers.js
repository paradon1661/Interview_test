import { VueQueryPlugin } from '@tanstack/vue-query'
import { queryClient } from '@/lib/queryClient'
import { router } from './router'

/** Everything the app plugs in, in one place (ememo-front: app/providers.jsx). */
export function installProviders(app) {
  app.use(router)
  app.use(VueQueryPlugin, { queryClient })
}
