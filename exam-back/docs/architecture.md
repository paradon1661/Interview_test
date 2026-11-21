# Exam backend — architecture

Same shape as ememo-back: one ASP.NET Core project, split into **modules**, with the schema owned by SQL migrations.

## 1. Shape

```
Browser (exam-front, Vue)
   │  same-origin /api (Vite proxy in dev)
   ▼
Exam_Api (.NET 10) ──── one transaction: result + answers ────► PostgreSQL 17
```

## 2. Folders

```
Exam_Api/
├── Program.cs                 DI, CORS, middleware order, health checks
├── GlobalUsings.cs            every Exam namespace visible everywhere
├── migrations/                001–003 *.sql — the schema (EF only maps it)
├── script/                    start-db, run-migrations, reset-db
└── src/
    ├── Infrastructure/  Data (ApplicationDbContext + EF Configurations), Repositories (BaseRepository<T>)
    ├── Modules/<Module>/  Controllers/ DTOs/ Entities/ Interfaces/ Repositories/ Services/ Validators/
    │     Questions · ExamResults
    └── Shared/          Middleware (correlation id, exceptions), Wrappers (Result<T>), Exceptions, Filters, Services (Clock)
tests/Exam.Tests           unit tests: grading rule, request validator
```

**Request flow:** Controller → Service (interface) → Repository (interface) → ApplicationDbContext.
Controllers stay thin; services orchestrate; business rules live on the entity.

**Where the grading rule lives.** `Modules/ExamResults/Entities/ExamResult.cs` → `ExamResult.Grade(...)`:
- 1 ข้อ ตอบได้ 1 คำตอบ (`duplicate_answer`)
- ตัวเลือกต้องเป็นของข้อนั้น (`invalid_answer`)
- ข้อที่ไม่ได้ตอบ = ผิด; คะแนนเต็ม = จำนวนข้อสอบทั้งหมด
- the score is computed on the server only, and the answer key (`is_correct`) is never sent to the browser.

It is unit tested without a database (`tests/Exam.Tests/ExamGradingTests.cs`).

**Response wrapper.** Every JSON response is a `Result<T>`:
- success: `{ success: true, data }`
- failure: `{ success: false, error: { code, message, correlationId, details } }`

The HTTP status still carries the meaning: 400 validation, 404 not found, 422 business rule, 500 unexpected.
`exam-front/src/lib/apiFetch.js` unwraps `data` and throws `ApiError` otherwise.

## 3. API

| Method | Path | What |
|---|---|---|
| GET | `/api/questions` | ข้อสอบ + ตัวเลือก (ไม่มีเฉลย) — IT 10-1 |
| POST | `/api/exam-results` | `{ fullName, answers: [{ questionId, choiceId }] }` → ตรวจ คิดคะแนน บันทึก — returns the result |
| GET | `/api/exam-results/{id}` | ผลสอบที่บันทึกไว้ — IT 10-2 (refresh-safe) |
| GET | `/health/live`, `/health/ready` | process up / Postgres reachable |

Swagger UI (Development): http://localhost:5080/swagger

## 4. Database

Built only by `migrations/NNN_*.sql`, run by `script/run-migrations.sh`.
- Each file runs in its own transaction and is recorded with a checksum in `schema_migrations`; editing an applied file is refused.
- EF Core does mapping only (snake_case naming), so a new column means a new SQL file *and* an entity change.

| Table | What |
|---|---|
| `tb_m_question` | ข้อสอบ (master): text, sort order, active flag |
| `tb_m_question_choice` | ตัวเลือก + `is_correct`; partial unique index → ถูกได้ตัวเลือกเดียวต่อข้อ |
| `tb_exam_result` | ผลสอบ: ชื่อ-สกุล, คะแนน, จำนวนข้อ, เวลาส่ง (uuid v7 id) |
| `tb_exam_answer` | คำตอบที่เลือก; unique `(exam_result_id, question_id)` → 1 ข้อ 1 คำตอบ; `is_correct` snapshot ณ เวลาตรวจ |

Mock data: `003_seed_questions.sql`.
