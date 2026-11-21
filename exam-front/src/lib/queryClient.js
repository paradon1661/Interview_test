import { QueryClient } from '@tanstack/vue-query'

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 60_000,
      refetchOnWindowFocus: false,
      // 4xx won't fix itself; only retry network / 5xx once.
      retry: (count, error) => count < 1 && !(error?.status >= 400 && error?.status < 500),
    },
  },
})
