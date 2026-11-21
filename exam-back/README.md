# exam-back

ASP.NET Core (.NET 10) API for the IT exam. Structure follows ememo-back — see [docs/architecture.md](docs/architecture.md).

```bash
# Postgres 17 on localhost:5434 + migrations
Exam_Api/script/start-db.sh

# API on http://localhost:5080 (Swagger: /swagger)
dotnet run --project Exam_Api --launch-profile http

# Unit tests
dotnet test

# Wipe and rebuild the local database
Exam_Api/script/reset-db.sh
```
