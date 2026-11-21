# Interview Test — ระบบทำข้อสอบ (IT 10-1 / IT 10-2)

| Folder | What |
|---|---|
| [`exam-back`](exam-back) | C# ASP.NET Core (.NET 10), modular like ememo-back — [architecture](exam-back/docs/architecture.md) |
| [`exam-front`](exam-front) | Vue 3 + Vite + Tailwind, feature folders like ememo-front |
| `exam-back/docker-compose.yml` | PostgreSQL 17 (port 5434) |

## Run

```bash
# 1) Database: Postgres 17 + migrations (schema + mock questions)
exam-back/Exam_Api/script/start-db.sh

# 2) API: http://localhost:5080
cd exam-back && dotnet run --project Exam_Api --launch-profile http

# 3) Web: http://localhost:5173
cd exam-front && cp .env.example .env && npm install && npm run dev
```

## Requirements → where

1. ข้อสอบแต่ละข้อเลือกคำตอบได้ข้อเดียว — radio group per question (`QuestionItem.vue`), re-checked on the server (`ExamResult.Grade`) and in the DB (unique `exam_result_id, question_id`).
2. ส่งข้อสอบ → บันทึกชื่อและคะแนนลงฐานข้อมูล (`tb_exam_result`, `tb_exam_answer`) → แสดงหน้า IT 10-2.
3. สอบอีกครั้ง → เคลียร์ข้อมูลและเริ่มสอบใหม่ (`/`).
