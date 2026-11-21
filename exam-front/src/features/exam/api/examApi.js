import { apiFetch } from '@/lib/apiFetch'

// exam-back: QuestionsController, ExamResultsController
export const examApi = {
  /** ข้อสอบ (ไม่มีเฉลย): `[{ id, no, questionText, choices: [{ id, choiceText }] }]` */
  questions: () => apiFetch('/api/questions'),

  /** `{ fullName, answers: [{ questionId, choiceId }] }` → ExamResultDto */
  submit: (body) => apiFetch('/api/exam-results', { method: 'POST', body }),

  /** `{ id, fullName, score, totalQuestions, createAt, answers: [{ questionId, choiceId }] }` */
  result: (id) => apiFetch(`/api/exam-results/${encodeURIComponent(id)}`),
}
