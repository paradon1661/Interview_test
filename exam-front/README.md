# exam-front

Vue 3 + Vite + Tailwind 4 + TanStack Query. Structure follows ememo-front (feature folders).

```bash
cp .env.example .env
npm install
npm run dev     # http://localhost:5173 — /api is proxied to BACKEND_URL (exam-back)
npm run lint
```

```
src/
├── main.js
├── app/             App.vue, router.js (routes + page titles), providers.js (router, vue-query)
├── config/          env.js, routes.js (ROUTES constants)
├── lib/             apiFetch.js (the only way to call the BE; unwraps Result<T>), queryClient.js, cn.js
├── styles/          index.css — design tokens → Tailwind theme
├── components/
│   ├── ui/          BaseButton, TextInput, RadioOption, FullPageMessage (index.js re-exports)
│   └── layout/      ExamLayout (green title bar: IT 10-1 / IT 10-2)
├── features/exam/
│   ├── api/         examApi.js
│   ├── composables/ useQuestions, useSubmitExam, useExamResult, queryKeys
│   ├── components/  ExamSheet (shared by both pages), QuestionItem
│   ├── pages/       ExamPage (IT 10-1), ExamResultPage (IT 10-2)
│   └── exam.js      answer map ↔ API list helpers
└── pages/           NotFoundPage
```

| Route | Page |
|---|---|
| `/` | IT 10-1 — กรอกชื่อ เลือกคำตอบ (radio: ข้อละ 1 คำตอบ) → ส่งข้อสอบ |
| `/results/:id` | IT 10-2 — คำตอบที่ส่ง (read-only) + `คุณ <ชื่อ> สอบได้คะแนน : x/y`; สอบอีกครั้ง → `/` with a cleared sheet |
