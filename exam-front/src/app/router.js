import { createRouter, createWebHistory } from 'vue-router'
import ExamLayout from '@/components/layout/ExamLayout.vue'
import { env } from '@/config/env'
import { ROUTES } from '@/config/routes'

/** Every page loads on first visit. `meta.title` is the code in the layout's title bar (IT 10-1 / IT 10-2). */
const routes = [
  {
    path: '/',
    component: ExamLayout,
    children: [
      {
        path: ROUTES.EXAM,
        name: 'exam',
        component: () => import('@/features/exam/pages/ExamPage.vue'),
        meta: { title: 'IT 10-1' },
      },
      {
        path: ROUTES.EXAM_RESULT(),
        name: 'exam-result',
        component: () => import('@/features/exam/pages/ExamResultPage.vue'),
        props: true,
        meta: { title: 'IT 10-2' },
      },
    ],
  },
  { path: '/:pathMatch(.*)*', component: () => import('@/pages/NotFoundPage.vue') },
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.afterEach((to) => {
  document.title = to.meta.title ? `${to.meta.title} · ${env.appName}` : env.appName
})
